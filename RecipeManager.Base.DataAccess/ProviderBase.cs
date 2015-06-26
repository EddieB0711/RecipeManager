using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;

namespace RecipeManager.Base.DataAccess {

    internal sealed class ProviderBase {
        private readonly ConnectionBase connection;
        private readonly ClassAccessor[] accessors;
        private readonly Func<object> createModel;
        private readonly string tableName;

        public ProviderBase(Func<DbConnection> createConnection, 
            string connectionString,
            string tableName,
            IEnumerable<ClassAccessor> accessors,
            Func<object> createModel) {
            this.connection = new ConnectionBase(createConnection, connectionString);
            this.accessors = accessors.ToArray();
            this.createModel = createModel;
            this.tableName = tableName;
        }

        public IEnumerable<object> GetRecords(string whereClause) {
            var records = new List<object>();
            var actions = new Action<DbConnection>[] {
                new Action<DbConnection>(connection => {
                    using (var command = connection.CreateCommand()) {
                        var sb = new System.Text.StringBuilder();

                        sb.Append(string.Format("SELECT {0} FROM {1} {2} ", 
                            string.Join(", ", this.accessors.Select(accessor => accessor.Name)), 
                            this.tableName, whereClause));

                        command.CommandText = sb.ToString();
                        using (var reader = command.ExecuteReader()) {
                            while (reader.Read()) {
                                var model = this.createModel();
                                mapValues(reader, model);
                                records.Add(model);
                            }
                        }
                    }
                })
            };

            this.connection.Connect(actions);

            return records;
        }

        private void mapValues(DbDataReader reader, object record) {
            foreach (var accessor in this.accessors) {
                var index = reader.GetOrdinal(accessor.Name);

                if (reader.IsDBNull(index)) {
                    continue;
                }

                if (accessor.PropertyType == typeof(string)) {
                    accessor.Set(record, reader.GetString(index));
                } else if (accessor.PropertyType == typeof(int) || accessor.PropertyType == typeof(int?)) {
                    accessor.Set(record, reader.GetInt32(index));
                } else if (accessor.PropertyType == typeof(long) || accessor.PropertyType == typeof(long?)) {
                    accessor.Set(record, reader.GetInt64(index));
                } else if (accessor.PropertyType == typeof(DateTime) || accessor.PropertyType == typeof(DateTime?)) {
                    accessor.Set(record, reader.GetDateTime(index));
                } else if (accessor.PropertyType == typeof(char) || accessor.PropertyType == typeof(char?)) {
                    accessor.Set(record, reader.GetChar(index));
                }
            }
        }
    }
}