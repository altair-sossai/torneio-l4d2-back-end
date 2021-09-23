using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using TorneioLeft4Dead2.Jogadores.Commands;

namespace TorneioLeft4Dead2.Jogadores.Extensions
{
    public static class JogadorCommandExtensions
    {
        public static string Login(this JogadorCommand command)
        {
            var patterns = new[]
            {
                @"(^[^\/ ]+$)",
                @"https?:\/\/steamcommunity.com\/id\/([^\/ ]+)/?$"
            };

            return command.MatchValue(patterns);
        }

        public static string SteamId(this JogadorCommand command)
        {
            var patterns = new[]
            {
                @"(^\d+$)",
                @"^https?:\/\/steamcommunity\.com\/profiles\/(\d+)\/?$"
            };

            return command.MatchValue(patterns);
        }

        private static string MatchValue(this JogadorCommand command, IEnumerable<string> patterns)
        {
            var pattern = patterns.FirstOrDefault(pattern => Regex.IsMatch(command.User, pattern));

            if (string.IsNullOrEmpty(pattern))
                return null;

            var match = Regex.Match(command.User, pattern);
            var group = match.Groups[1];

            return group.Value;
        }
    }
}