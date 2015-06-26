using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Threading.Tasks;

namespace RecipeManager.Base.DataAccess {

    internal sealed class PropagatorBase {
        private readonly ConnectionBase connection;
        private readonly ClassAccessor[] accessors;
        private readonly string tableName;

        public PropagatorBase(Func<DbConnection> createConnection,
            IEnumerable<ClassAccessor> accessors,
            string connectionString,
            string tableName) {
            this.connection = new ConnectionBase(createConnection, connectionString);
            this.accessors = accessors.ToArray();
            this.tableName = tableName;
        }

        public void SaveChanges(IEnumerable<object> updateRecords, IEnumerable<object> insertRecords, Action<DbCommand, object, int> mapParameters, string whereClause) {
            var actions = new Action<DbConnection>[] {
                    createUpdateAction(updateRecords, mapParameters, whereClause),
                    createInsertAction(insertRecords, mapParameters)
                };

            this.connection.Connect(actions);
        }

        private Action<DbConnection> createUpdateAction(IEnumerable<object> records, Action<DbCommand, object, int> mapParameters, string whereClause) {
            var bindings = Enumerable.Range(0, this.accessors.Length).Select(x => string.Format("{0} = @p{1}", this.accessors[x].Name, x));
            var keys = Enumerable.Range(0, this.accessors.Length).Where(x => this.accessors[x].IsPrimaryKey)
                .Select(x => string.Format("{0} = @p{1}", this.accessors[x].Name, this.accessors.Length + x));
            var builder = new System.Text.StringBuilder();
            var where = string.IsNullOrEmpty(whereClause) ? " WHERE " + string.Join(" AND ", keys) : whereClause;

            builder.Append(string.Format("UPDATE {0} SET {1}{2}", this.tableName, string.Join(", ", bindings), where));

            return this.createAction(records, mapParameters, bindings.Count() + keys.Count(), builder.ToString());
        }

        private Action<DbConnection> createInsertAction(IEnumerable<object> records, Action<DbCommand, object, int> mapParameters) {
            var tableColumns = this.accessors.Select(x => x.Name);
            var bindings = Enumerable.Range(0, this.accessors.Length).Select(x => "@p" + x);
            var builder = new System.Text.StringBuilder();

            builder.Append(string.Format("INSERT INTO {0} ({1}) VALUES ({2})", this.tableName, string.Join(", ", tableColumns), string.Join(", ", bindings)));

            return this.createAction(records, mapParameters, bindings.Count(), builder.ToString());
        }

        private Action<DbConnection> createAction(IEnumerable<object> records, Action<DbCommand, object, int> mapParameters, int bindingCount, string sql) {
            Action<DbConnection> action = connection => {
                if (records.Count() == 0)
                    return;

                var tasks = records.Select(x => new Action(() => {
                    using (var command = connection.CreateCommand()) {
                        command.CommandText = sql;
                        mapParameters(command, x, bindingCount);
                        command.ExecuteNonQuery();
                    }
                })).Select(x => Task.Factory.StartNew(x));

                using (var transaction = connection.BeginTransaction()) {
                    try {
                        Task.WaitAll(tasks.ToArray());
                        transaction.Commit();
                    } catch {
                        transaction.Rollback();
                    }
                }
            };

            return action;
        }
    }
}