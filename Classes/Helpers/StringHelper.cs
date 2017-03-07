using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;

namespace Announcer.Classes.Helpers
{
    public class StringHelper
    {
        public static string RemoveByArray(string str, List<string> array)
        {
            var sb = new StringBuilder(str.Trim());

            array.ForEach(x => sb.Replace(x, ""));

            return sb.ToString();
        }

        public static string StripSlashes(string str)
        {
            try
            {
                return Regex.Replace(str, @"(\\)([\000\010\011\012\015\032\042\047\134\140])", "$2");
            }
            catch
            {
                return str;
            }
        }

        public static string EncodeGiantBombSpecialCharacters(string str)
        {
            var chars = new string[] { ":", ";", "," };

            foreach (var @char in chars)
            {
                str = Regex.Replace(str, @char, WebUtility.HtmlEncode(@char));
            }

            return str;
        }

        /// <summary>
        /// Calculate similarity between two strings.
        /// </summary>
        /// <param name="source"></param>
        /// <param name="target"></param>
        /// <returns></returns>
        public static double CalculateSimilarity(string source, string target)
        {
            if ((source == null) || (target == null)) return 0.0;
            if ((source.Length == 0) || (target.Length == 0)) return 0.0;
            if (source == target) return 1.0;

            int stepsToSame = ComputeLevenshteinDistance(source, target);
            return (1.0 - ((double)stepsToSame / (double)Math.Max(source.Length, target.Length)));
        }

        /// <summary>
        /// Calculate Levenshtein distance between two strings.
        /// </summary>
        /// <param name="source"></param>
        /// <param name="target"></param>
        /// <returns></returns>
        public static int ComputeLevenshteinDistance(string source, string target)
        {
            if ((source == null) || (target == null)) return 0;
            if ((source.Length == 0) || (target.Length == 0)) return 0;
            if (source == target) return source.Length;

            int sourceWordCount = source.Length;
            int targetWordCount = target.Length;

            // Step 1
            if (sourceWordCount == 0)
                return targetWordCount;

            if (targetWordCount == 0)
                return sourceWordCount;

            int[,] distance = new int[sourceWordCount + 1, targetWordCount + 1];

            // Step 2
            for (int i = 0; i <= sourceWordCount; distance[i, 0] = i++) ;
            for (int j = 0; j <= targetWordCount; distance[0, j] = j++) ;

            for (int i = 1; i <= sourceWordCount; i++)
            {
                for (int j = 1; j <= targetWordCount; j++)
                {
                    // Step 3
                    int cost = (target[j - 1] == source[i - 1]) ? 0 : 1;

                    // Step 4
                    distance[i, j] = Math.Min(Math.Min(distance[i - 1, j] + 1, distance[i, j - 1] + 1), distance[i - 1, j - 1] + cost);
                }
            }

            return distance[sourceWordCount, targetWordCount];
        }
    }
}
