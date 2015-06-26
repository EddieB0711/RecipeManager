using System.Linq.Expressions;

namespace RecipeManager.Generic.DataAccess {

    public class DbQueryProvider<T> : QueryProvider {
        private readonly GenericDataAccess<T> dataAccess;

        public DbQueryProvider(string connectionString, string tableName)
            : base(connectionString, tableName) {
            this.dataAccess = new GenericDataAccess<T>(connectionString, tableName);
        }

        public override object Execute(Expression expression) {
            return dataAccess.GetRecords(translate(expression));
        }

        public override string GetQueryText(Expression expression) {
            return this.translate(expression);
        }

        public void Add(T record) {
            dataAccess.Add(record);
        }

        public void Attach(T record) {
            dataAccess.Attach(record);
        }

        private string translate(Expression expression) {
            return new QueryTranslator().Translate(expression);
        }
    }
}