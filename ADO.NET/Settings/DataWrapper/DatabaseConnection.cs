using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using System.Threading.Tasks;

namespace ADO.NET.Settings.DataWrapper
{
    /// Represents custom wrapper class used for interacting with SQL Server Connection.
    public class DatabaseConnection : IDisposable
    {
        /// Represents object to hold reference for SQL Server Connection.
        private SqlConnection connection;

        /// Gets a value indicating whether this instance is initialized.
        /// <c>true</c> if this instance is initialized; otherwise, <c>false</c>.
        public bool IsInitialized { get; private set; }

        /// Gets the connection string value.
        public string ConnectionString { get; private set; }

        /// Gets the Connection State.
        public ConnectionState State => connection.State;

        /// Initializes the specified connection string using specified connection string value.
        public void Initialize(string connectionString)
        {
            connection = new SqlConnection(connectionString);

            ConnectionString = connectionString;

            IsInitialized = !string.IsNullOrEmpty(connection.ConnectionString);
        }

        /// Creates a new instance to store reference for SQL Server Command.
        public SqlCommand CreateCommand()
        {
            if (!IsInitialized)
            {
                throw new ArgumentException(DataAccessWrapperConstants.EXCEPTION_MESSAGE_CANNOT_CREATE_COMMAND);
            }

            var command = connection.CreateCommand();

            return command;
        }

        /// Opens SQL Server connection for various purposes.
        public void Open()
        {
            if (!IsInitialized)
            {
                throw new ArgumentException(DataAccessWrapperConstants.EXCEPTION_MESSAGE_CANNOT_OPEN_CONNECTION);
            }

            try
            {
                if (connection.State != ConnectionState.Open)
                {
                    connection.Open();
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// Opens the asynchronous connection.
        public async Task OpenAsync()
        {
            if (!IsInitialized)
            {
                throw new ArgumentException(DataAccessWrapperConstants.EXCEPTION_MESSAGE_CANNOT_OPEN_CONNECTION);
            }

            try
            {
                if (connection.State != ConnectionState.Open)
                {
                    await connection.OpenAsync();
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// Closes connection to Database.
        public void Close()
        {
            try
            {
                if (connection != null)
                {
                    connection.Close();
                }
            }
            catch (Exception)
            {
                throw;
            }
        }
         
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
        /// Releases unmanaged and - optionally - managed resources.
        /// <c>true</c> to release both managed and unmanaged resources; <c>false</c> to release only unmanaged resources.
        protected virtual void Dispose(bool disposing)
        {
            Close();

            if (disposing)
            {
                connection = null;
            }
        }
    }
}
