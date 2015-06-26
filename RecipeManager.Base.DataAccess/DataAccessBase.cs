using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;

namespace RecipeManager.Base.DataAccess {
    public class DataAccessBase {
        private readonly PropagatorBase propagator;
        private readonly ProviderBase provider;
        private readonly IEnumerable<ClassAccessor> accessors;
        private readonly Action<DbCommand, string, object> createParameter;

        public DataAccessBase(Func<DbConnection> createConnection,
            IEnumerable<ClassAccessor> accessors,
            Action<DbCommand, string, object> createParameter,
            string connectionString,
            string tableName,
            Func<object> createModel) {
            this.accessors = accessors;
            this.createParameter = createParameter;
            this.propagator = new PropagatorBase(createConnection, accessors, connectionString, tableName);
            this.provider = new ProviderBase(createConnection, connectionString, tableName, accessors, createModel);
        }

        public void SaveChanges(IEnumerable<object> updateRecords, IEnumerable<object> insertRecords, string whereClause) {
            Action<DbCommand, object, int> mapParameters = (command, record, bindingCount) => {
                int i = 0;
                foreach (var accessor in this.accessors) {
                    this.createParameter(command, "@p" + i, accessor.Get(record));
                    i++;
                }

                if (bindingCount > this.accessors.Count()) {
                    var keys = this.accessors.Where(x => x.IsPrimaryKey);

                    foreach (var key in keys) {
                        this.createParameter(command, "@p" + i, key.Get(record));
                        i++;
                    }
                }
            };
            this.propagator.SaveChanges(updateRecords, insertRecords, mapParameters, whereClause);
        }

        public IEnumerable<object> GetRecords(string whereClause) {
            return this.provider.GetRecords(whereClause);
        }
    }
}