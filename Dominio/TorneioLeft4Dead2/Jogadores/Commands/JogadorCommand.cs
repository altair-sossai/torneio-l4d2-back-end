using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace TorneioLeft4Dead2.Jogadores.Commands
{
    public class JogadorCommand
    {
        public string User { get; set; }

        public string Login
        {
            get
            {
                var patterns = new[]
                {
                    @"(^[^\/ ]+$)",
                    @"https?:\/\/steamcommunity.com\/id\/([^\/ ]+)/?$"
                };

                return MatchValue(patterns);
            }
        }

        public string SteamId
        {
            get
            {
                var patterns = new[]
                {
                    @"(^\d+$)",
                    @"^https?:\/\/steamcommunity\.com\/profiles\/(\d+)\/?$"
                };

                return MatchValue(patterns);
            }
        }

        private string MatchValue(IEnumerable<string> patterns)
        {
            var pattern = patterns.FirstOrDefault(pattern => Regex.IsMatch(User, pattern));

            if (string.IsNullOrEmpty(pattern))
                return null;

            var match = Regex.Match(User, pattern);
            var group = match.Groups[1];

            return group.Value;
        }
    }
}