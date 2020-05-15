using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using System.Threading.Tasks;

namespace ADO.NET.Settings.DataWrapper
{
    public class DatabaseCommand
    {
        /// Represents variable to map to Generic class for reading records.
        private readonly IMapper mapper;

        /// Represents instance of Database Connection used to interact with SQL Server connection.
        private DatabaseConnection connection;

        /// Represents object to hold value of Stored Procedure or Query to be executed.
        private SqlCommand command;

        /// Initializes a new instance of the <see cref="DatabaseCommand" /> cla
        public DatabaseCommand() : this(Mapper.Default)
        {
        }

        /// Initializes a new instance of the <see cref="DatabaseCommand" /> class using Strong Entity Mapper.
        public DatabaseCommand(IMapper mapper)
        {
            this.mapper = mapper;
        }

        /// Gets a value indicating whether Command is initialized by passing either Stored Procedure Name or Query.
        ///   <c>true</c> if this instance is initialized; otherwise, <c>false</c>.
        public bool IsInitialized { get; private set; }

        /// Initializes SQL Server command with connection, Command Type (Stored Procedure or Table) and
        /// Stored Procedure name or Query Text.
        public void Initialize(DatabaseConnection connection, CommandType commandType, string commandText)
        {
            if (!connection.IsInitialized)
            {
                throw new ArgumentException(
                    DataAccessWrapperConstants.EXCEPTION_MESSAGE_REQUIRES_INITIALIZED_CONNECTION);
            }

            this.connection = connection;

            command = this.connection.CreateCommand();
            command.CommandType = commandType;
            command.CommandText = commandText;
            command.CommandTimeout = 300; // hardcode updated
            this.IsInitialized = command.Connection != null
                            && !string.IsNullOrEmpty(command.Connection.ConnectionString);
        }

        /// Adds new parameter for current command.
        public DatabaseCommand AddParameter(string parameterName, object value, bool isOutput = false)
        {
            if (!IsInitialized)
            {
                throw new ArgumentException(DataAccessWrapperConstants.EXCEPTION_MESSAGE_ADD_PARAMETER_FAILURE);
            }

            command.Parameters.Add(CreateParameter(parameterName, value, isOutput));

            return this;
        }

        /// Asynchronously reads all records.
        public async Task<IEnumerable<T>> ReadAllAsync<T>(Func<SqlDataReader, T> mapDataToObject)
        {
            var result = new List<T>();

            await connection.OpenAsync();

            using (var reader = await command.ExecuteReaderAsync())
            {
                while (await reader.ReadAsync())
                {
                    result.Add(mapDataToObject(reader));
                }
            }

            connection.Close();
            return result;
        }

        /// Asynchronously reads single record.
        public async Task<T> ReadOneAsync<T>(Func<SqlDataReader, T> mapDataToObject)
        {
            await connection.OpenAsync();

            using (var reader = await command.ExecuteReaderAsync())
            {
                if (await reader.ReadAsync())
                {
                    return mapDataToObject(reader);
                }
            }

            connection.Close();

            return default(T);
        }

        /// Asynchronously maps all records to generic type specified.
        public async Task<IEnumerable<T>> ReadAllMapToAsync<T>()
            where T : class, new()
        {
            return await ReadAllAsync(mapper.MapDataToObject<T>);
        }

        /// Asynchronously maps single record to generic type specified.
        public async Task<T> ReadOneMapToAsync<T>()
            where T : class, new()
        {
            return await ReadOneAsync(mapper.MapDataToObject<T>);
        }

        /// Asynchronously executes a Transact-SQL statement against the connection
        /// and returns the number of rows affected.
        public async Task<int> NonQueryAsync()
        {
            connection.Open();
            var response = await command.ExecuteNonQueryAsync();
            connection.Close();
            return response;
        }

        /// Asynchronously inserts a new record in Database and returns the value of 1st Primary Key field if available.
        public async Task<object> InsertRecordAsync()
        {
            await connection.OpenAsync();
            await command.ExecuteNonQueryAsync();
            connection.Close();
            foreach (SqlParameter currentRecord in command.Parameters)
            {
                if (currentRecord.Direction == ParameterDirection.Output)
                {
                    return currentRecord.Value;
                }
            }

            return null;
        }

        /// Asynchronously executes the query, and returns the first column of the
        /// first row in the result set returned by the query.
        /// Additional columns or rows are ignored.
        public async Task<object> ScalarAsync()
        {
            await connection.OpenAsync();
            var result = await command.ExecuteScalarAsync();
            connection.Close();
            return result;
        }

        /// <summary>
        /// Asynchronously executes the query, and returns the handle for SqlDataReader with Open connection.
        /// </summary>
        /// <returns>
        /// The reference to SqlDataReader with Open connection enabling access to multiple recordsets.
        /// </returns>
        public async Task<SqlDataReader> ReaderAsync()
        {
            await connection.OpenAsync();
            var result = command.ExecuteReader();
            return result;
        }

        /// Creates a new parameter with Parameter name and value.
        private SqlParameter CreateParameter(string parameterName, object value, bool isOutput)
        {
            var parameter = command.CreateParameter();

            parameter.ParameterName = parameterName;
            parameter.Value = value;

            if (isOutput)
            {
                parameter.Direction = ParameterDirection.Output;
            }

            return parameter;
        }
    }
}
