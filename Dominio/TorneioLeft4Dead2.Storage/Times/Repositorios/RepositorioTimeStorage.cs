using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TorneioLeft4Dead2.Storage.Jogadores.Repositorios.Base;
using TorneioLeft4Dead2.Storage.UnitOfWork;
using TorneioLeft4Dead2.Times.Entidades;
using TorneioLeft4Dead2.Times.Repositorios;

namespace TorneioLeft4Dead2.Storage.Times.Repositorios
{
    public class RepositorioTimeStorage : BaseRepository<TimeEntity>, IRepositorioTime
    {
        private const string TableName = "Times";
        private readonly IRepositorioTimeJogador _repositorioTimeJogador;

        public RepositorioTimeStorage(UnitOfWorkStorage unitOfWork,
            IRepositorioTimeJogador repositorioTimeJogador)
            : base(unitOfWork, TableName)
        {
            _repositorioTimeJogador = repositorioTimeJogador;
        }

        public async Task<TimeEntity> ObterPorCodigoAsync(string codigo)
        {
            return await GetByRowKeyAsync(codigo);
        }

        public async Task<List<TimeEntity>> ObterTimesAsync()
        {
            var entities = await GetAllAsync();

            return entities.OrderBy(o => o.Codigo).ToList();
        }

        public async Task<TimeEntity> SalvarAsync(TimeEntity entity)
        {
            return await InsertOrMergeAsync(entity);
        }

        public async Task ExcluirAsync(string codigo)
        {
            await DeleteAsync(codigo);
            await _repositorioTimeJogador.ExcluirPorTimeAsync(codigo);
        }
    }
}