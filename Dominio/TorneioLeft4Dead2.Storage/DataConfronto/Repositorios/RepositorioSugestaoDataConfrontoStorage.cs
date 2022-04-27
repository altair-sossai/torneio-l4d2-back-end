using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TorneioLeft4Dead2.DataConfronto.Entidades;
using TorneioLeft4Dead2.DataConfronto.Repositorios;
using TorneioLeft4Dead2.Storage.UnitOfWork;
using TorneioLeft4Dead2.Storage.UnitOfWork.Commands;
using TorneioLeft4Dead2.Storage.UnitOfWork.Repositories;

namespace TorneioLeft4Dead2.Storage.DataConfronto.Repositorios
{
    public class RepositorioSugestaoDataConfrontoStorage : BaseRepository<SugestaoDataConfrontoEntity>, IRepositorioSugestaoDataConfronto
    {
        private const string TableName = "SugestoesDatasConfrontos";

        public RepositorioSugestaoDataConfrontoStorage(UnitOfWorkStorage unitOfWork)
            : base(unitOfWork, TableName)
        {
        }

        public async Task<List<SugestaoDataConfrontoEntity>> ObterPorConfrontoAsync(Guid confrontoId)
        {
            const string nome = nameof(SugestaoDataConfrontoEntity.Data);

            var queryCommand = QueryCommand.Default
                .OrderBy(nome);

            return await GetAllAsync(queryCommand);
        }

        public async Task<SugestaoDataConfrontoEntity> SalvarAsync(SugestaoDataConfrontoEntity entity)
        {
            return await InsertOrMergeAsync(entity);
        }

        public async Task ExcluirPorConfrontoAsync(Guid confrontoId)
        {
            await DeleteAsync(confrontoId);
        }
    }
}