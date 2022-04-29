using Microsoft.Azure.Cosmos.Table;

namespace TorneioLeft4Dead2.Storage.UnitOfWork.Commands
{
    public class QueryCommand
    {
        private string _where;
        public static QueryCommand Default => new();

        public QueryCommand Where(string property, string value)
        {
            var where = TableQuery.GenerateFilterCondition(property, QueryComparisons.Equal, value);

            return Where(where);
        }

        private QueryCommand Where(string where)
        {
            _where = where;

            return this;
        }

        public TableQuery<T> BuildTableQuery<T>()
        {
            var tableQuery = new TableQuery<T>();

            if (!string.IsNullOrEmpty(_where))
                tableQuery = tableQuery.Where(_where);

            return tableQuery;
        }
    }
}