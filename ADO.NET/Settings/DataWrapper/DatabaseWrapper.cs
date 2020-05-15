using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace ADO.NET.Settings.DataWrapper
{
    public class DatabaseWrapper
    {
        /// Gets the default wrapper singleton instance for SQL Server interactions.
        public static DatabaseWrapper Default => Singleton<DatabaseWrapper>.Instance;

        /// Gets the reference for Database Connection Wrapper instance.
        public DatabaseConnection Connection { get; private set; }

        /// Initializes the instance with SQL Server Connection String.
        public void InitializeWithConfigurationFile(string connectionString)
        {
            if (connectionString != string.Empty)
            {
                Connection = new DatabaseConnection();
                Connection.Initialize(connectionString);
            }
            else
            {
                throw new ArgumentNullException(string.Format(DataAccessWrapperConstants.EXCEPTION_MESSAGE_CONNECTION_STRING_MISSING,connectionString));
            }
        }

        /// Creates new wrapper instance for SQL Server command using given Query Text.
        public DatabaseCommand CreateCommand(string sql)
        {
            var command = new DatabaseCommand();
            command.Initialize(Connection, CommandType.Text, sql);
            return command;
        }

        /// Creates new wrapper instance for SQL Server command using given Stored Procedure name.
        public DatabaseCommand CreateStoredProcedureCommand(string storedProcedure)
        {
            var command = new DatabaseCommand();
            command.Initialize(Connection, CommandType.StoredProcedure, storedProcedure);
            return command;
        }
    }
}
