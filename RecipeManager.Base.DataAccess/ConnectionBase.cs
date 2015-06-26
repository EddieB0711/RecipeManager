using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Threading.Tasks;

namespace RecipeManager.Base.DataAccess {
    internal sealed class ConnectionBase {
        private readonly Func<DbConnection> createConnection;
        private readonly string connectionString;

        public ConnectionBase(Func<DbConnection> createConnection, string connectionString) {
            if (createConnection == null)
                throw new ArgumentNullException("createConnection");
            if (string.IsNullOrEmpty(connectionString))
                throw new ArgumentNullException("connectionString");

            this.createConnection = createConnection;
            this.connectionString = connectionString;
        }
        /// <summary>
        /// For each action, create and open a connection and perform
        /// operations on that connection.
        /// </summary>
        /// <param name="actions">Operations to be performed</param>
        public void Connect(IEnumerable<Action<DbConnection>> actions) {
            var tasks = actions.Select(x => new Action(() => {
                using (var connection = this.createConnection()) {
                    connection.ConnectionString = this.connectionString;
                    connection.Open();

                    x(connection);
                }
            })).Select(x => Task.Factory.StartNew(x)).ToArray();

            Task.WaitAll(tasks);
        }
    }
}