using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Caching.Memory;
using TorneioLeft4Dead2.Storage.UnitOfWork;
using TorneioLeft4Dead2.Storage.UnitOfWork.Commands;
using TorneioLeft4Dead2.Storage.UnitOfWork.Repositories;
using TorneioLeft4Dead2.Times.Entidades;
using TorneioLeft4Dead2.Times.Repositorios;

namespace TorneioLeft4Dead2.Storage.Times.Repositorios
{
    public class RepositorioTimeJogadorStorage : BaseRepository<TimeJogadorEntity>, IRepositorioTimeJogador
    {
        private const string TableName = "TimesJogadores";

        public RepositorioTimeJogadorStorage(UnitOfWorkStorage unitOfWork, IMemoryCache memoryCache)
            : base(unitOfWork, TableName, memoryCache)
        {
        }

        public async Task<List<TimeJogadorEntity>> ObterTodosAsync()
        {
            return (await GetAllAsync(QueryCommand.Default))
                .OrderBy(o => o.Time)
                .ThenBy(t => t.Ordem)
                .ToList();
        }

        public async Task<List<TimeJogadorEntity>> ObterPorTimeAsync(string codigo)
        {
            const string time = nameof(TimeJogadorEntity.Time);

            var queryCommand = QueryCommand.Default
                .Where(time, codigo);

            var entities = await GetAllAsync(queryCommand);

            return entities.OrderBy(o => o.Ordem).ToList();
        }

        public async Task<TimeJogadorEntity> SalvarAsync(TimeJogadorEntity entity)
        {
            return await InsertOrMergeAsync(entity);
        }

        public async Task DesvincularJogadorAsync(string codigo, string steamId)
        {
            var rowKey = $"{codigo}_{steamId}";

            await DeleteAsync(rowKey);
        }

        public async Task ExcluirPorJogadorAsync(string steamId)
        {
            const string jogador = nameof(TimeJogadorEntity.Jogador);

            var queryCommand = QueryCommand.Default
                .Where(jogador, steamId);

            foreach (var entity in await GetAllAsync(queryCommand))
                await DeleteAsync(entity);
        }

        public async Task ExcluirPorTimeAsync(string codigo)
        {
            const string time = nameof(TimeJogadorEntity.Time);

            var queryCommand = QueryCommand.Default
                .Where(time, codigo);

            foreach (var entity in await GetAllAsync(queryCommand))
                await DeleteAsync(entity);
        }
    }
}