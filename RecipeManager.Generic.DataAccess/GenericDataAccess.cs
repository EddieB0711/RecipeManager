using MySql.Data.MySqlClient;
using RecipeManager.Base.DataAccess;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Reflection;

namespace RecipeManager.Generic.DataAccess {

    public class GenericDataAccess<T> {
        private readonly DataAccessBase baseAccess;
        private readonly ICollection<object> loadedRecords;
        private readonly ICollection<object> addedRecods;
        private readonly Func<DbConnection> createConnection = () => new MySqlConnection();

        private readonly Action<DbCommand, string, object> createParameter = (command, name, value) => {
            var newCommand = command as MySqlCommand;
            newCommand.Parameters.AddWithValue(name, value);
        };

        private readonly Func<object> createModel = () => Activator.CreateInstance(typeof(T));

        public GenericDataAccess(string connectionString, string tableName) {
            this.baseAccess = new DataAccessBase(createConnection, createClassAccessors(), createParameter,
                connectionString, tableName, createModel);
            this.loadedRecords = new List<object>();
            this.addedRecods = new List<object>();
        }

        public void Add(object record) {
            this.addedRecods.Add(record);
        }

        public void Attach(object record) {
            if (!this.loadedRecords.Contains(record)) {
                this.loadedRecords.Add(record);
            }
        }

        public void SaveChanges(string whereClause) {
            this.baseAccess.SaveChanges(loadedRecords, addedRecods, whereClause);
            this.addedRecods.Clear();
        }

        public IEnumerable<T> GetRecords(string whereClause) {
            if (string.IsNullOrEmpty(whereClause) && loadedRecords.Count > 0) {
                return this.loadedRecords.Cast<T>();
            } else {
                return this.baseAccess.GetRecords(whereClause).Cast<T>();
            }
        }

        private IEnumerable<ClassAccessor> createClassAccessors() {
            var properties = typeof(T).GetProperties();
            var accessors = properties.Select(x => {
                return x.GetCustomAttribute(typeof(PrimaryKey)) == null ? new ClassAccessor(x) : new ClassAccessor(x) { IsPrimaryKey = true };
            });

            return accessors;
        }
    }
}