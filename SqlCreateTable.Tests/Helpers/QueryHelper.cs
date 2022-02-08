using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace SqlCreateTable.Tests.Helpers
{
    internal static class QueryHelper
    {
        private const string BlockComments = @"/\*(.*?)\*/";
        private const string LineComments = @"--(.*$?)";

        public static string[] GetAllQueries(string fileName)
        {
            if (!File.Exists(fileName))
                return Array.Empty<string>();

            var rawData = File.ReadAllText(fileName);
            rawData = RemoveComments(rawData);
            return ParseQueries(rawData);
        }

        public static string ComposeErrorMessage(string query, Exception ex = null, string message = null)
        {
            var exMessage = ex == null ? "" : $"\nException message: {ex.Message}";
            return $"{message}{exMessage}\nQuery with error: {query}";
        }

        public static string ComposeErrorMessage(string query, string message = null)
        {
            return ComposeErrorMessage(query, null, message);
        }
        
        public static bool IsQueryCorrect(IEnumerable<string> queries, Func<string, bool> isCorrect) => queries.Any(isCorrect);

        private static string RemoveComments(string rawData)
        {
            rawData = Regex.Replace(rawData, LineComments, "");
            rawData = Regex.Replace(rawData, BlockComments, "", RegexOptions.Multiline);
            rawData = Regex.Replace(rawData, @"\s+", " ");
            return rawData;
        }

        private static string[] ParseQueries(string rawData) =>
            rawData.Split(";", StringSplitOptions.RemoveEmptyEntries)
                .Where(s => !string.IsNullOrWhiteSpace(s))
                .ToArray();
    }
}
