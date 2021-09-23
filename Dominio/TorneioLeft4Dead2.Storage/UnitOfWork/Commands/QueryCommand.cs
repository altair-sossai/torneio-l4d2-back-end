using Microsoft.Azure.Cosmos.Table;

namespace TorneioLeft4Dead2.Storage.UnitOfWork.Commands
{
    public class QueryCommand
    {
        private string _orderBy;
        private string _where;
        public static QueryCommand Default => new();

        public QueryCommand Where(string property, string value)
        {
            var where = TableQuery.GenerateFilterCondition(property, QueryComparisons.Equal, value);

            return Where(where);
        }

        public QueryCommand Where(string where)
        {
            _where = where;

            return this;
        }

        public QueryCommand OrderBy(string orderBy)
        {
            _orderBy = orderBy;

            return this;
        }

        public TableQuery<T> BuildTableQuery<T>()
        {
            var tableQuery = new TableQuery<T>();

            if (string.IsNullOrEmpty(_where))
                tableQuery = tableQuery.Where(_where);

            if (string.IsNullOrEmpty(_orderBy))
                tableQuery = tableQuery.OrderBy(_orderBy);

            return tableQuery;
        }
    }
}