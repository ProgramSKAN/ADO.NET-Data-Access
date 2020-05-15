using ADO.NET.EntityClass;
using ADO.NET.Settings.DataWrapper.Repository;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Text;
using System.Threading.Tasks;

namespace ADO.NET.Settings.DataWrapper
{
    public class UsingDataWrapper:RepositoryBase
    {
        public async Task<object> GetSingleRecordUsingSql()
        {
            object returnValue = new object();
            string sql = "SELECT * FROM Country WHERE CountryId<@Id";
            DatabaseWrapper database = new DatabaseWrapper();
            database.InitializeWithConfigurationFile(Config.ConnectionString);

            try
            {
                returnValue = await database
                    .CreateCommand(sql)
                    .AddParameter("@Id", 20)
                    .ReadOneMapToAsync<Country>();
            }
            catch (SqlException ex)
            {
                returnValue = GetCustomExceptionOn(ex);
            }
            catch (Exception ex)
            {
                returnValue = GetCustomExceptionOn(ex);
            }
            finally
            {
                database.Connection.Close();
            }
            Console.WriteLine(JsonConvert.SerializeObject(returnValue));
            return returnValue;
        }
        public async Task<object> GetAllRecordsUsingSql()
        {
            object returnValue = new object();
            string sql = "SELECT * FROM Country WHERE CountryId<@Id";
            DatabaseWrapper database = new DatabaseWrapper();
            database.InitializeWithConfigurationFile(Config.ConnectionString);

            try
            {
                returnValue = await database
                    .CreateCommand(sql)
                    .AddParameter("@Id", 20)
                    .ReadAllMapToAsync<Country>();
            }
            catch (SqlException ex)
            {
                returnValue = GetCustomExceptionOn(ex);
            }
            catch (Exception ex)
            {
                returnValue = GetCustomExceptionOn(ex);
            }
            finally
            {
                database.Connection.Close();
            }
            Console.WriteLine(JsonConvert.SerializeObject(returnValue));
            return returnValue;
        }
        public async Task<object> InsertRecordReturnsRowsAffected()
        {
            object returnValue = new object();
            string sql = "INSERT INTO Country(CountryId,IsDeleted,CountryAbbreviation,CountryName,CountryCallingCode)";
            sql += "VALUES(@CountryId,@IsDeleted,@CountryAbbreviation,@CountryName,@CountryCallingCode)";
            DatabaseWrapper database = new DatabaseWrapper();
            database.InitializeWithConfigurationFile(Config.ConnectionString);

            try
            {
                returnValue = await database
                    .CreateCommand(sql)
                    .AddParameter("@CountryId", 256)
                    .AddParameter("@IsDeleted", false)
                    .AddParameter("@CountryAbbreviation", "IND")
                    .AddParameter("@CountryName", "INDIA")
                    .AddParameter("@CountryCallingCode", "+91")
                    .NonQueryAsync();
            }
            catch (SqlException ex)
            {
                returnValue = GetCustomExceptionOn(ex);
            }
            catch (Exception ex)
            {
                returnValue = GetCustomExceptionOn(ex);
            }
            finally
            {
                database.Connection.Close();
            }
            Console.WriteLine("Rows affected: "+(int)returnValue);
            return returnValue;
        }
        public async Task<object> InsertRecordReturnsPrimaryKey()
        {
            object returnValue = new object();
            string sql = "State_Insert";
            DatabaseWrapper database = new DatabaseWrapper();
            database.InitializeWithConfigurationFile(Config.ConnectionString);
            int StateId = 0;
            try
            {
                
                returnValue = Convert.ToInt32(await database
                    .CreateStoredProcedureCommand(sql)
                    .AddParameter("@StateId",StateId,true)
                    .AddParameter("@CountryId", 1)
                    .AddParameter("@IsDeleted", false)
                    .AddParameter("@StateAbbreviation", "TG")
                    .AddParameter("@StateName", "TG")
                    .InsertRecordAsync());
            }
            catch (SqlException ex)
            {
                returnValue = GetCustomExceptionOn(ex);
            }
            catch (Exception ex)
            {
                returnValue = GetCustomExceptionOn(ex);
            }
            finally
            {
                database.Connection.Close();
            }
            Console.WriteLine("newly added record Primary Key value: "+returnValue);
            return returnValue;
        }
    }
}
