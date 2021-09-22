using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TorneioLeft4Dead2.Campanhas.Entidades;

namespace TorneioLeft4Dead2.Campanhas.Repositorios
{
    public class RepositorioCampanha : IRepositorioCampanha
    {
        private static readonly List<CampanhaEntity> Campanhas = new()
        {
            new CampanhaEntity
            {
                Codigo = 1,
                Nome = "Shopping da morte",
                NomeOriginal = "Dead center",
                PontuacaoMaxima = 3000
            },
            new CampanhaEntity
            {
                Codigo = 2,
                Nome = "A passagem",
                NomeOriginal = "The passing",
                PontuacaoMaxima = 2100
            },
            new CampanhaEntity
            {
                Codigo = 3,
                Nome = "Parque sombrio",
                NomeOriginal = "Dark carnival",
                PontuacaoMaxima = 3500
            },
            new CampanhaEntity
            {
                Codigo = 4,
                Nome = "Febre do pântano",
                NomeOriginal = "Swamp fever",
                PontuacaoMaxima = 3000
            },
            new CampanhaEntity
            {
                Codigo = 5,
                Nome = "Tempestade",
                NomeOriginal = "Hard rain",
                PontuacaoMaxima = 3500
            },
            new CampanhaEntity
            {
                Codigo = 6,
                Nome = "A paróquia",
                NomeOriginal = "The parish",
                PontuacaoMaxima = 3500
            },
            new CampanhaEntity
            {
                Codigo = 7,
                Nome = "Córregos gelados",
                NomeOriginal = "Cold stream",
                PontuacaoMaxima = 3000
            },
            new CampanhaEntity
            {
                Codigo = 8,
                Nome = "Sem misericórdia",
                NomeOriginal = "No mercy",
                PontuacaoMaxima = 3500
            },
            new CampanhaEntity
            {
                Codigo = 9,
                Nome = "Contagem de corpos",
                NomeOriginal = "Death tool",
                PontuacaoMaxima = 3500
            },
            new CampanhaEntity
            {
                Codigo = 10,
                Nome = "Aeromorto",
                NomeOriginal = "Dead air",
                PontuacaoMaxima = 3500
            },
            new CampanhaEntity
            {
                Codigo = 11,
                Nome = "Colheita de sangue",
                NomeOriginal = "Blood harvest",
                PontuacaoMaxima = 3500
            },
            new CampanhaEntity
            {
                Codigo = 12,
                Nome = "Rota de colisão",
                NomeOriginal = "Crash course",
                PontuacaoMaxima = 1300
            },
            new CampanhaEntity
            {
                Codigo = 13,
                Nome = "O sacrifício",
                NomeOriginal = "The sacrifice",
                PontuacaoMaxima = 2100
            },
            new CampanhaEntity
            {
                Codigo = 14,
                Nome = "O último confronto",
                NomeOriginal = "The last stand",
                PontuacaoMaxima = 2000
            }
        };

        public Task<CampanhaEntity> ObterPorIdAsync(int codigo)
        {
            var entity = Campanhas.FirstOrDefault(f => f.Codigo == codigo);

            return Task.FromResult(entity);
        }

        public Task<List<CampanhaEntity>> ObterCampanhasAsync()
        {
            return Task.FromResult(Campanhas);
        }
    }
}