using ADO.NET.ManagerClasses;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text;

namespace ADO.NET.Settings.Connect_and_Submit_Sql_To_Db
{
    public static class Exceptions
    {
        public static int SimpleExceptionHandling()
        {
            int RowsAffected = 0;
            string ResultText = null;
            string sql = "INSERT INTO Country(IsDeleted,CountryAbbreviation,CountryName,CountryCallingCode)";
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
        public static int CatchException()
        {
            int RowsAffected = 0;
            string ResultText = null;
            string sql = "Country_Insert";
            Console.WriteLine(sql);
            try
            {
                using (SqlConnection cnn = new SqlConnection(Config.ConnectionString))
                {
                    cnn.Open();
                    using (SqlCommand cmd = new SqlCommand(sql, cnn))
                    {
                        cmd.Parameters.Add(new SqlParameter("@ContryId", 255));//error
                        cmd.Parameters.Add(new SqlParameter("@IsDeleted", false));
                        cmd.Parameters.Add(new SqlParameter("@CountryAbbreviation", "IND"));
                        cmd.Parameters.Add(new SqlParameter("@CountryName", "INDIA"));
                        cmd.Parameters.Add(new SqlParameter("@CountryCallingCode", "+91"));

                        cmd.CommandType = CommandType.StoredProcedure;
                        RowsAffected = cmd.ExecuteNonQuery();

                        ResultText = "Rows Affected: " + RowsAffected.ToString();
                    }
                }
            }
            catch (SqlException ex)
            {
                StringBuilder sb = new StringBuilder();

                for (int i = 0; i < ex.Errors.Count; i++)
                {
                    sb.AppendLine("Index #: " + i.ToString());
                    sb.AppendLine("Type: " + ex.Errors[i].GetType().FullName);
                    sb.AppendLine("Message: " + ex.Errors[i].Message);
                    sb.AppendLine("Source: " + ex.Errors[i].Source);
                    sb.AppendLine("Number: " + ex.Errors[i].Number.ToString());
                    sb.AppendLine("State: " + ex.Errors[i].State.ToString());
                    sb.AppendLine("Class: " + ex.Errors[i].Class.ToString());
                    sb.AppendLine("Server: " + ex.Errors[i].Server);
                    sb.AppendLine("Procedure: " + ex.Errors[i].Procedure);
                    sb.AppendLine("LineNumber: " + ex.Errors[i].LineNumber.ToString());
                }

                ResultText = sb.ToString() + Environment.NewLine + ex.ToString();
            }
            Console.WriteLine(ResultText);
            return RowsAffected;
        }
        public static int GatherExceptionInformation()
        {
            int RowsAffected = 0;
            string ResultText;
            SqlConnection cnn = null;
            SqlCommand cmd = null;

            try
            {
                string sql = "Country_Insert";
                cnn = new SqlConnection(Config.ConnectionString);    
                cmd = new SqlCommand(sql, cnn);//using block is not used here to get reference of cmd in catch block
                cmd.Parameters.Add(new SqlParameter("@ContryId", 255));//error
                cmd.Parameters.Add(new SqlParameter("@IsDeleted", false));
                cmd.Parameters.Add(new SqlParameter("@CountryAbbreviation", "IND"));
                cmd.Parameters.Add(new SqlParameter("@CountryName", "INDIA"));
                cmd.Parameters.Add(new SqlParameter("@CountryCallingCode", "+91"));

                cmd.CommandType = CommandType.StoredProcedure;
                cnn.Open();
                RowsAffected = cmd.ExecuteNonQuery();

                ResultText = "Rows Affected: " + RowsAffected.ToString();
            }
            catch (SqlException ex)
            {
                SqlServerExceptionManager.Instance.Publish(ex, cmd, "Error in ExceptionViewModel.GatherExceptionInformation()");
                ResultText = SqlServerExceptionManager.Instance.LastException.ToString();
                Console.WriteLine(ResultText);
            }
            catch (Exception ex)
            {
                ResultText = ex.ToString();
            }
            finally
            {
                // Must close/dispose here so we have access to info for error handling
                if (cnn != null)
                {
                    cnn.Close();
                    cnn.Dispose();
                }
                if (cmd != null)
                {
                    cmd.Dispose();
                }
            }

            return RowsAffected;
        }
    }
}
