using System.Collections.Generic;
using System.Threading.Tasks;
using Refit;
using TorneioLeft4Dead2.PlayStats.Models;

namespace TorneioLeft4Dead2.PlayStats.Services;

public interface IMatchesService
{
    [Get("/api/matches/vanilla4mod/between/{start}/and/{end}")]
    Task<List<Match>> BetweenAsync(string start, string end);
}