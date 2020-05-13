using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Text;

namespace ADO.NET.Settings.Connect_and_Submit_Sql_To_Db
{
    public class Connection
    {
        private static string ResultText;
        public static string Connect(string cnnstring)
        {
            SqlConnection cnn = new SqlConnection(cnnstring);
            cnn.Open();
            ResultText = GetConnectionInformation(cnn);
            cnn.Close();
            cnn.Dispose();
            return ResultText.ToString();
        }
        public static string ConnectUsingBlock(string cnnstring)
        {
            using(SqlConnection cnn = new SqlConnection(cnnstring)){
                cnn.Open();
                ResultText = GetConnectionInformation(cnn);
                return ResultText.ToString();
            }
        }

        private static string GetConnectionInformation(SqlConnection cnn)
        {
            StringBuilder sb = new StringBuilder(1024);
            sb.AppendLine("Connection String: " + cnn.ConnectionString);
            sb.AppendLine("State: " + cnn.State.ToString());
            sb.AppendLine("Connection timeout: " + cnn.ConnectionTimeout.ToString());
            sb.AppendLine("database: " + cnn.Database);
            sb.AppendLine("data source: " + cnn.DataSource);
            sb.AppendLine("server version: " + cnn.ServerVersion);
            sb.AppendLine("workstation ID: " + cnn.WorkstationId);
            return sb.ToString();
        }

        public static string ConnectWithErrors()
        {
            try
            {
                string cnnstring = "Server=ERROR;Connection Timeout=5;Database=ERROR;Trusted_Connection=True;";
                using (SqlConnection cnn = new SqlConnection(cnnstring))
                {
                    cnn.Open();
                    ResultText = GetConnectionInformation(cnn);
                    return ResultText.ToString();
                }
            }
            catch(Exception ex)
            {
                ResultText = ex.ToString();
                return ResultText.ToString();
            }
            
        }
    }
}
