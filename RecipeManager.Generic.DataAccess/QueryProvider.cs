using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace RecipeManager.Generic.DataAccess {
    public abstract class QueryProvider : IQueryProvider {
        private readonly string connectionString;
        private readonly string tableName;

        protected QueryProvider(string connectionString, string tableName) {
            this.connectionString = connectionString;
            this.tableName = tableName;
        }

        IQueryable<S> IQueryProvider.CreateQuery<S>(Expression expression) {
            return new Query<S>(this.connectionString, this.tableName, expression);
        }

        IQueryable IQueryProvider.CreateQuery(Expression expression) {
            Type elementType = TypeSystem.GetElementType(expression.Type);
            try {
                return (IQueryable)Activator.CreateInstance(typeof(Query<>).MakeGenericType(elementType), new object[] { this.connectionString, this.tableName, expression });
            } catch (TargetInvocationException tie) {
                throw tie.InnerException;
            }
        }

        S IQueryProvider.Execute<S>(Expression expression) {
            return ((IEnumerable<S>)this.Execute(expression)).FirstOrDefault();
        }

        object IQueryProvider.Execute(Expression expression) {
            return this.Execute(expression);
        }

        public abstract string GetQueryText(Expression expression);

        public abstract object Execute(Expression expression);
    }
}