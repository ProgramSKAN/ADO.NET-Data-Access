using ADO.NET.ManagerClasses;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text;

namespace ADO.NET.Settings.Connect_and_Submit_Sql_To_Db
{
    public class BuilderClass
    {
        const string ConnectionString = Config.ConnectionString;
        string ResultText;
        public string BreakApartConnectionString()
        {
            StringBuilder sb = new StringBuilder(1024);

            // Create a connection string builder object
            SqlConnectionStringBuilder builder =new SqlConnectionStringBuilder(ConnectionString);

            // Access each property of the connection string
            sb.AppendLine("Application Name: " + builder.ApplicationName);
            sb.AppendLine("Data Source: " + builder.DataSource);
            sb.AppendLine("Initial Catalog: " + builder.InitialCatalog);
            sb.AppendLine("User ID: " + builder.UserID);
            sb.AppendLine("Password: " + builder.Password);
            sb.AppendLine("Integrated Security: " + builder.IntegratedSecurity);

            ResultText = sb.ToString();
            Console.WriteLine(ResultText);
            return ResultText;
        }
        public string CreateConnectionString()
        {
            SqlConnectionStringBuilder builder =new SqlConnectionStringBuilder
                  {
                      ApplicationName = "A New Application",
                      ConnectTimeout = 5,
                      DataSource = "Localhost",
                      InitialCatalog = "ADONETSamples",
                      UserID = "Tester",
                      Password = "P@ssw0rd"
                  };

            ResultText = builder.ToString();
            Console.WriteLine(ResultText);
            return ResultText;
        }
        public string CreateDataModificationCommands()
        {
            try
            {
                // Create SQL connection object
                using (SqlConnection cnn =new SqlConnection(Config.ConnectionString))
                {
                    // Create a SQL Data Adapter
                    using (SqlDataAdapter da =new SqlDataAdapter(CountryManager.STATE_SQL, cnn))
                    {
                        // Fill DataTable
                        DataTable dt = new DataTable();
                        da.Fill(dt);

                        // Create a command builder
                        using (SqlCommandBuilder builder =new SqlCommandBuilder(da))
                        {
                            // Build INSERT Command
                            // Pass true to generate parameters names matching column names
                            ResultText = "Insert: " + builder.GetInsertCommand(true).CommandText;

                            ResultText += Environment.NewLine;

                            // Build UPDATE command
                            ResultText += "Update: " + builder.GetUpdateCommand(true).CommandText;

                            ResultText += Environment.NewLine;

                            // Build DELETE command
                            ResultText += "Delete: " + builder.GetDeleteCommand(true).CommandText;
                        }

                    }
                }
            }
            catch (Exception ex)
            {
                ResultText = ex.ToString();
            }
            Console.WriteLine(ResultText);
            return ResultText;
        }
        public string InsertUsingDataModificationCommand()
        {
            try
            {
                // Create SQL connection object
                using (SqlConnection cnn =new SqlConnection(Config.ConnectionString))
                {
                    // Create a SQL Data Adapter
                    using (SqlDataAdapter da =new SqlDataAdapter(CountryManager.COUNTRY_SQL, cnn))
                    {
                        // Fill DataTable
                        DataTable dt = new DataTable();
                        da.Fill(dt);

                        // Create a command builder
                        using (SqlCommandBuilder builder =new SqlCommandBuilder(da))
                        {
                            // Build INSERT Command
                            using (SqlCommand cmd = builder.GetInsertCommand(true))
                            {
                                // Set generated parameters with values to insert
                                cmd.Parameters["@CountryId"].Value = 255;
                                cmd.Parameters["@IsDeleted"].Value = false;
                                cmd.Parameters["@CountryAbbreviation"].Value = "IND";
                                cmd.Parameters["@CountryName"].Value = "INDIA";
                                cmd.Parameters["@CountryCallingCode"].Value = "+91";

                                // Set the connection into the command object
                                cmd.Connection = cnn;

                                // Open the connection for inserting
                                cnn.Open();

                                // Execute the command
                                int rowsaffected=cmd.ExecuteNonQuery();
                                ResultText = "Rows affected : " + rowsaffected;
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ResultText = ex.ToString();
            }
            Console.WriteLine(ResultText);
            return ResultText;
        }
    }
}
