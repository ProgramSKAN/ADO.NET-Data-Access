using ADO.NET.EntityClass;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text;

namespace ADO.NET.Settings.Connect_and_Submit_Sql_To_Db
{
    public static class Command
    {
        private static Country _country = new Country { CountryName = "%H%" };
        private static Country _inputCountry = new Country {CountryId=255,IsDeleted=false,CountryAbbreviation="IND",CountryName = "INDIA",CountryCallingCode="+91"};
        public static int GetCountryTableCountScalar()
        {
            int RowsAffected = 0;
            string sql = "SELECT COUNT(*) FROM Country";
            Console.WriteLine("SQL:" + sql);
            using (SqlConnection cnn = new SqlConnection(Config.ConnectionString))
            {
                cnn.Open();
                using (SqlCommand cmd = new SqlCommand(sql, cnn))
                {
                    RowsAffected = (int)cmd.ExecuteScalar();
                }
            }
            return RowsAffected;
        }
        public static int InsertCountry()
        {
            int RowsAffected = 0;
            string ResultText = null;
            string sql = "INSERT INTO Country(CountryId,IsDeleted,CountryAbbreviation,CountryName,CountryCallingCode)";
            sql += "VALUES(255,0,'IND','INDIA',+91)";
            Console.WriteLine("SQL:" + sql);
            try
            {
                using (SqlConnection cnn = new SqlConnection(Config.ConnectionString))
                {
                    cnn.Open();
                    using (SqlCommand cmd = new SqlCommand(sql, cnn))
                    {
                        cmd.CommandType = CommandType.Text;
                        RowsAffected = (int)cmd.ExecuteNonQuery();
                        ResultText = "Rows Affected: " + RowsAffected.ToString();
                    }
                }
            }
            catch (Exception ex)
            {
                ResultText = ex.ToString();
            }
            Console.WriteLine(ResultText);
            return RowsAffected;
        }
        public static int GetCountryCountScalarUsingParameters()
        {
            int RowsAffected = 0;
            string ResultText = null;
            string sql = "SELECT COUNT(*) FROM Country";
            sql += " WHERE CountryName LIKE @CountryName";
            Console.WriteLine(sql);
            using (SqlConnection cnn = new SqlConnection(Config.ConnectionString))
            {
                cnn.Open();
                using (SqlCommand cmd = new SqlCommand(sql, cnn))
                {
                    cmd.Parameters.Add(new SqlParameter("@CountryName", _country.CountryName));
                    RowsAffected = (int)cmd.ExecuteScalar();
                }
            }
            ResultText = "Rows Affected: " + RowsAffected.ToString();
            Console.WriteLine("Country names like '%H%");
            Console.WriteLine(ResultText);
            return RowsAffected;
        }
        public static int InsertCountryUsingParameters()
        {
            int RowsAffected = 0;
            string ResultText = null;
            string sql = "INSERT INTO Country(CountryId,IsDeleted,CountryAbbreviation,CountryName,CountryCallingCode)";
            sql += "VALUES(@CountryId,@IsDeleted,@CountryAbbreviation,@CountryName,@CountryCallingCode)";
            Console.WriteLine(sql);
            try
            {
                using (SqlConnection cnn = new SqlConnection(Config.ConnectionString))
                {
                    cnn.Open();
                    using (SqlCommand cmd = new SqlCommand(sql, cnn))
                    {
                        cmd.Parameters.Add(new SqlParameter("@CountryId", _inputCountry.CountryId));
                        cmd.Parameters.Add(new SqlParameter("@IsDeleted", _inputCountry.IsDeleted));
                        cmd.Parameters.Add(new SqlParameter("@CountryAbbreviation", _inputCountry.CountryAbbreviation));
                        cmd.Parameters.Add(new SqlParameter("@CountryName", _inputCountry.CountryName));
                        cmd.Parameters.Add(new SqlParameter("@CountryCallingCode", _inputCountry.CountryCallingCode));

                        cmd.CommandType = CommandType.Text;
                        RowsAffected = cmd.ExecuteNonQuery();

                        ResultText = "Rows Affected: " + RowsAffected.ToString();
                    }
                }
            }
            catch (Exception ex)
            {
                ResultText = ex.ToString();
            }
            Console.WriteLine(ResultText);
            return RowsAffected;
        }
        public static int InsertCountryOutputParameter()
        {
            int RowsAffected = 0;
            string ResultText = null;
            string sql = "Country_Insert";
            Console.WriteLine("SP:alter PROC Country_Insert @IsDeleted nvarchar(max), @CountryAbbreviation nvarchar(max),@CountryName nvarchar(max),@CountryCallingCode nvarchar(max),@CountryId int output AS BEGIN INSERT INTO Country(IsDeleted, CountryAbbreviation, CountryName, CountryCallingCode) VALUES(@IsDeleted, @CountryAbbreviation, @CountryName, @CountryCallingCode) SELECT @CountryId = SCOPE_IDENTITY(); END");
            try
            {
                using (SqlConnection cnn = new SqlConnection(Config.ConnectionString))
                {
                    cnn.Open();
                    using (SqlCommand cmd = new SqlCommand(sql, cnn))
                    {
                        // Create input parameters
                        cmd.Parameters.Add(new SqlParameter("@IsDeleted", _inputCountry.IsDeleted));
                        cmd.Parameters.Add(new SqlParameter("@CountryAbbreviation", _inputCountry.CountryAbbreviation));
                        cmd.Parameters.Add(new SqlParameter("@CountryName", _inputCountry.CountryName));
                        cmd.Parameters.Add(new SqlParameter("@CountryCallingCode", _inputCountry.CountryCallingCode));

                        // Create OUTPUT parameter
                        cmd.Parameters.Add(new SqlParameter { 
                            ParameterName= "@CountryId",
                            Value=_inputCountry.CountryId,
                            IsNullable=false,
                            DbType=System.Data.DbType.Int32,
                            Direction=ParameterDirection.Output
                        });


                        cmd.CommandType = CommandType.Text;
                        RowsAffected = cmd.ExecuteNonQuery();

                        _inputCountry.CountryId = (int)cmd.Parameters["@CountryId"].Value;
                        Console.WriteLine("output parameter country Id value: " + _inputCountry.CountryId);

                        ResultText = "Rows Affected: " + RowsAffected.ToString();
                    }
                }
            }
            catch (Exception ex)
            {
                ResultText = ex.ToString();
            }
            Console.WriteLine(ResultText);
            return RowsAffected;
        }
        public static int TransactionProcessing()
        {
            int RowsAffected = 0;
            string ResultText = null;
            string sql = "INSERT INTO Country(CountryId,IsDeleted,CountryAbbreviation,CountryName,CountryCallingCode)";
            sql += "VALUES(@CountryId,@IsDeleted,@CountryAbbreviation,@CountryName,@CountryCallingCode)";
            Console.WriteLine(sql);
            try
            {
                using (SqlConnection cnn = new SqlConnection(Config.ConnectionString))
                {
                    cnn.Open();
                    using(SqlTransaction trn = cnn.BeginTransaction())
                    {
                        try
                        {
                            using (SqlCommand cmd = new SqlCommand(sql, cnn))
                            {
                                cmd.Transaction = trn;
                                cmd.Parameters.Add(new SqlParameter("@CountryId", _inputCountry.CountryId));
                                cmd.Parameters.Add(new SqlParameter("@IsDeleted", _inputCountry.IsDeleted));
                                cmd.Parameters.Add(new SqlParameter("@CountryAbbreviation", _inputCountry.CountryAbbreviation));
                                cmd.Parameters.Add(new SqlParameter("@CountryName", _inputCountry.CountryName));
                                cmd.Parameters.Add(new SqlParameter("@CountryCallingCode", _inputCountry.CountryCallingCode));

                                cmd.CommandType = CommandType.Text;
                                RowsAffected = cmd.ExecuteNonQuery();

                                ResultText = "country Rows Affected: " + RowsAffected.ToString();
                                // 2nd transaction
                                sql = "INSERT INTO STATE(StateId,CountryId,IsDeleted,StateAbbreviation,StateName)";
                                sql += " VALUES(@StateId,@CountryId,@IsDeleted,@StateAbbreviation,@StateName)";
                                cmd.CommandText = sql;
                                cmd.Parameters.Clear();
                                cmd.Parameters.Add(new SqlParameter("@StateId", 1));
                                cmd.Parameters.Add(new SqlParameter("@CountryId", 1));
                                cmd.Parameters.Add(new SqlParameter("@IsDeleted", false));
                                cmd.Parameters.Add(new SqlParameter("@StateAbbreviation", "TG"));
                                cmd.Parameters.Add(new SqlParameter("@StateName", "TG"));
                                RowsAffected = cmd.ExecuteNonQuery();
                                ResultText += "state Rows Affected: " + RowsAffected.ToString();

                                trn.Commit();

                            }
                        }
                        catch(Exception ex)
                        {
                            trn.Rollback();
                            ResultText = "Transaction Rolled Back" + Environment.NewLine + ex.ToString();
                        }
                    }
                    
                }
            }
            catch (Exception ex)
            {
                ResultText = ex.ToString();
            }
            Console.WriteLine(ResultText);
            return RowsAffected;
        }
    }
}
