using Announcer.Interfaces;
using System;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;

namespace Announcer.Classes.Services
{
    public class FileService : ILogWriter
    {
        private static readonly SemaphoreSlim sl = new SemaphoreSlim(initialCount: 1);

        public enum PosterType { game, movie, random }

        private static readonly string[] allowedExtensions = { ".jpg", ".jpeg", ".png", ".gif" };

        public static string PosterPath
        {
            get { return Path.Combine(Path.GetDirectoryName(Assembly.GetEntryAssembly().Location), "posters"); }
        }

        public static string LogPath
        {
            get { return Path.Combine(Path.GetDirectoryName(Assembly.GetEntryAssembly().Location), "logs"); }
        }

        public static string GetPoster(string name, PosterType type, bool randomIfEmpty = true)
        {
            var path = Path.Combine(PosterPath, type.ToString(), name);

            foreach (var extension in allowedExtensions)
            {
                if (File.Exists(path + extension))
                    return path + extension;
            }

            if (randomIfEmpty)
                return GetRandomPoster();

            return "";
        }

        public static string GetRandomPoster()
        {
            var path = Path.Combine(PosterPath, PosterType.random.ToString());

            var files = Directory.GetFiles(path);

            var rand = new Random();
            var file = files[rand.Next(files.Length)];

            return Path.Combine(path, file);
        }

        public static async Task<string> DownloadPoster(string url, string name, PosterType type, bool overwrite = false)
        {
            await sl.WaitAsync();

            checkFolderIntegrity();

            var extension = Path.GetExtension(url);
            extension = !string.IsNullOrEmpty(extension) && allowedExtensions.Any(x => x == extension) ? extension : ".jpg";

            using (var c = GiantBombService.Instance.GetClient())
            {
                var path = Path.Combine(PosterPath, type.ToString(), name + extension);

                if (File.Exists(path) && overwrite == false)
                {
                    sl.Release();

                    return path;
                }

                using (Stream remoteStream = await c.GetStreamAsync(url), localStream = new FileStream(path, FileMode.Create))
                {
                    await remoteStream.CopyToAsync(localStream);

                    sl.Release();

                    return path;
                }
            }
        }

        private static void checkFolderIntegrity()
        {
            if (Directory.Exists(PosterPath) == false)
                Directory.CreateDirectory(PosterPath);

            if (Directory.Exists(LogPath) == false)
                Directory.CreateDirectory(LogPath);

            var posterTypes = Enum.GetValues(typeof(PosterType));

            foreach (var posterType in posterTypes)
            {
                var path = Path.Combine(PosterPath, posterType.ToString());

                if (Directory.Exists(path) == false)
                    Directory.CreateDirectory(path);
            }
        }

        public async void WriteLogFile(string message, Logger.Severity severity)
        {
            await sl.WaitAsync();

            checkFolderIntegrity();

            using (var sw = File.AppendText(Path.Combine(LogPath, string.Format("{0}.txt", severity.ToString()))))
            {
                sw.WriteLine(message);
            }

            sl.Release();
        }
    }
}
