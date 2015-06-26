using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace RecipeManager.Generic.DataAccess {
    public class Query<T> : IQueryable<T>, IQueryable, IEnumerable<T>, IEnumerable, IOrderedQueryable<T>, IOrderedQueryable {
        private DbQueryProvider<T> provider;
        private Expression expression;

        public Query(string connectionString, string tableName) {

            this.provider = new DbQueryProvider<T>(connectionString, tableName);
            this.expression = Expression.Constant(this);
        }

        public Query(string connectionString, string tableName, Expression expression) {

            this.provider = new DbQueryProvider<T>(connectionString, tableName);
            this.expression = expression;
        }

        Expression IQueryable.Expression {
            get { return this.expression; }
        }

        Type IQueryable.ElementType {
            get { return typeof(T); }
        }

        IQueryProvider IQueryable.Provider {
            get { return this.provider; }
        }

        public IEnumerator<T> GetEnumerator() {
            return ((IEnumerable<T>)this.provider.Execute(this.expression)).GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator() {
            return ((IEnumerable)this.provider.Execute(this.expression)).GetEnumerator();
        }

        public override string ToString() {
            return this.provider.GetQueryText(this.expression);
        }

        public void Add(T record) {
            this.provider.Add(record);
        }

        public void Attach(T record) {
            this.provider.Attach(record);
        }
    }
}