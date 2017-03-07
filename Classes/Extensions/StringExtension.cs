using System.Linq;

namespace Announcer.Classes.Extensions
{
    public static class StringExtension
    {
        public static string ToTitleCase(this string str)
        {
            if (string.IsNullOrEmpty(str))
                return str;
            return str.First().ToString().ToUpper() + str.Substring(1);
        }
    }
}
