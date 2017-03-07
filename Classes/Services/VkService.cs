using Announcer.Classes.Helpers;
using Announcer.Interfaces;
using Announcer.Models.Helpers;
using Announcer.Models.Payloads;
using Announcer.Models.Responses.Vk;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Threading.Tasks;

namespace Announcer.Classes.Services
{
    public class VkService : ApiBase, IAnnounceService
    {
        const string API_VERSION = "5.60";
        const string LANGUAGE = "ru";
        const string API_URL = "https://api.vk.com/method/{0}";

        private AnnounceConfig config;

        public VkService(AnnounceConfig config)
        {
            this.config = config;
            this.config.ConfigOverride = config.ConfigOverride ?? Config.GetServiceConfig(AnnounceType.vk.ToString());
        }

        public async Task<uint> Announce(MessagePayload payload)
        {
            if (config.UseRandomPoster && string.IsNullOrEmpty(payload.PosterPath))
                payload.PosterPath = FileService.GetRandomPoster();

            return await Announce(payload.Message, payload.PosterPath, payload.PreviousDashboard?.VkPostId);
        }

        public async Task<uint> Announce(string message, string posterPath)
        {
            return await Announce(message, posterPath, null);
        }

        public async Task<uint> Announce(string message, string posterPath, uint? previousPostId = null)
        {
            var attachments = this.config.Message.Attachements;
            attachments.Add("https://kraken.su/stream/");

            // Upload poster
            if (string.IsNullOrEmpty(posterPath) == false)
            {
                var imageId = await uploadPoster(posterPath);

                if (string.IsNullOrEmpty(imageId) == false)
                    attachments.Add(imageId);
            }

            // Create a post
            var postId = await addPost($"{this.config.Message.Prepend}{message}{this.config.Message.Append}", attachments);

            // Delete previous post
            if (previousPostId != null && config.RemoveOldPosts)
            {
                await removePost(previousPostId.Value, config.RemoveCommentedPosts);
            }

            return postId;
        }

        private async Task<string> uploadPoster(string posterPath)
        {
            var url = await getUploadServer();

            if (string.IsNullOrEmpty(url))
                return null;

            var uploadResponse = await uploadPhoto(url, posterPath);

            if (uploadResponse == null)
                return null;

            return await saveWallPhoto(uploadResponse);
        }

        private async Task<uint> addPost(string message, List<string> attachments)
        {
            var post = await GetRequest<PostCreatedResponse>("wall.post", new NameValueCollection
            {
                { "owner_id", this.config.ConfigOverride.Id },
                { "friends_only", "0" },
                { "from_group", this.config.ConfigOverride.Id.Substring(0, 1) == "-" ? "1" : "0" },
                { "message", message },
                { "services", "twitter" },
                { "signed", "0" },
                { "attachments", string.Join(",", attachments) }
            });

            return post != null ? post.PostId : default(uint);
        }

        private async Task removePost(uint postId, bool ignoreComments)
        {
            var post = await getPost(postId);

            if (post == null) // Post might be deleted already
                return;

            if (post.Comments.Count > 0 && ignoreComments)
                return;

            await GetRequest<int>("wall.delete", new NameValueCollection
            {
                { "owner_id", this.config.ConfigOverride.Id },
                { "post_id", postId.ToString() }
            });
        }

        private async Task<Post> getPost(uint postId)
        {
            var post = await GetRequest<List<Post>>("wall.getById", new NameValueCollection
            {
                { "posts", $"{this.config.ConfigOverride.Id}_{postId}" }
            });

            if (post == null)
                return default(Post);

            return post.FirstOrDefault();
        }

        private async Task<string> saveWallPhoto(MediaUploadResultResponse uploadResult)
        {
            Int32.TryParse(this.config.ConfigOverride.Id, out int groupId);

            var response = await GetRequest<List<Photo>>("photos.saveWallPhoto", new NameValueCollection
            {
                { "server", uploadResult.Server },
                { "hash", uploadResult.Hash },
                { "photo", StringHelper.StripSlashes(uploadResult.Photo) },
                { "group_id", Math.Abs(groupId).ToString() }
            });

            var photo = response?.FirstOrDefault();

            return photo != null ? $"photo{photo.OwnerId}_{photo.Id}" : null;
        }

        private async Task<MediaUploadResultResponse> uploadPhoto(string url, string photoPath)
        {
            var response = await PostRequest<MediaUploadResultResponse>(url, new Dictionary<string, object>(), new NameValueCollection
            {
                { "photo", photoPath }
            });

            return response;
        }

        private async Task<string> getUploadServer()
        {
            Int32.TryParse(this.config.ConfigOverride.Id, out int groupId);

            var response = await GetRequest<MediaUploadServerResponse>("photos.getWallUploadServer", new NameValueCollection
            {
                { "group_id", Math.Abs(groupId).ToString() }
            });

            return response?.UploadUrl;
        }

        #region Overrides

        public override NameValueCollection GetParameters(NameValueCollection parameters)
        {
            parameters = base.GetParameters(parameters);

            parameters.Add("lang", LANGUAGE);
            parameters.Add("v", API_VERSION);
            parameters.Add("access_token", this.config.ConfigOverride.Token);

            return parameters;
        }

        public override async Task<T> GetRequest<T>(string method, NameValueCollection parameters)
        {
            var url = string.Format(API_URL, method);
            REQUEST:
            var response = await base.GetRequest<BaseResponse>(url, parameters);

            if (response == null)
                return default(T);

            if (response.Error != null)
            {
                Logger.Log($"{this.GetType().Name}: {response.Error.Message} ({url}).", Logger.Severity.error);

                if (response.Error.ErrorCode == 6) // Too many requests per second
                {
                    // Wait 1 second and restart the request
                    await Task.Delay(1000);
                    goto REQUEST;
                }
            }
            else
            {
                return JsonConvert.DeserializeObject<T>(response?.Response?.ToString());
            }

            return default(T);
        }

        #endregion
    }
}