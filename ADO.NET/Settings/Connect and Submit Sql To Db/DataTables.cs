using ADO.NET.EntityClass;
using ADO.NET.ManagerClasses;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;

namespace ADO.NET.Settings.Connect_and_Submit_Sql_To_Db
{
    public class DataTables
    {
        private Country _SearchEntity = new Country { CountryName = "%h%" };
        public Country SearchEntity
        {
            get { return _SearchEntity; }
            set
            {
                _SearchEntity = value;
            }
        }
        private DataTable _Countries;
        private DataTable _States;
        public DataTable Countries
        {
            get { return _Countries; }
            set
            {
                _Countries = value;
            }
        }
        public DataTable States
        {
            get { return _States; }
            set
            {
                _States = value;
            }
        }




        public DataTable GetCountriesAsDataTable()
        {
            DataTable dt = null;
            using (SqlConnection cnn = new SqlConnection(Config.ConnectionString))
            {
                using (SqlCommand cmd = new SqlCommand(CountryManager.COUNTRY_SQL, cnn))
                {
                    // Create a SQL Data Adapter
                    using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                    {
                        // Create new DataTable object for filling
                        dt = new DataTable();

                        // Fill DataTable using Data Adapter
                        da.Fill(dt);

                        // Loop through all rows/columns
                        string result= ProcessRowsAndColumns(dt);
                        Console.WriteLine(result);
                    }
                }
            }
            return dt;
        }
        private string ProcessRowsAndColumns(DataTable dt)
        {
            StringBuilder sb = new StringBuilder(2048);
            int index = 1;

            // Process each row
            foreach (DataRow row in dt.Rows)
            {
                sb.AppendLine("** Row: " + index.ToString() + " **");
                // Process each column
                foreach (DataColumn col in dt.Columns)
                {
                    sb.AppendLine(col.ColumnName + ": " + row[col.ColumnName].ToString());
                }
                sb.AppendLine();

                index++;
            }

            string ResultText = sb.ToString();
            return ResultText;
        }

        public List<Country> GetCountriesAsGenericList()
        {
            List<Country> ret = new List<Country>();
            string ResultText = string.Empty;
            int RowsAffected = 0;

            // Initialize DataTable object to null in case of an error
            DataTable dt = null;

            // Create SQL statement to submit
            string sql = CountryManager.COUNTRY_SQL;
            sql += " WHERE CountryName LIKE @CountryName";

            // Create a connection
            using (SqlConnection cnn = new SqlConnection(Config.ConnectionString))
            {
                // Create command object
                using (SqlCommand cmd = new SqlCommand(sql, cnn))
                {
                    // Create parameter
                    cmd.Parameters.Add(new SqlParameter("@CountryName", SearchEntity.CountryName));

                    // Create a SQL Data Adapter
                    using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                    {
                        // Fill DataTable using Data Adapter
                        dt = new DataTable();
                        da.Fill(dt);
                        if (dt.Rows.Count > 0)
                        {
                            ret =
                              (from row in dt.AsEnumerable()  // Must convert to an enumerable object
                                select new Country
                               {
                                   // Use Field<T>() method to get data
                                   CountryId = row.Field<int>("CountryId"),
                                   IsDeleted = row.Field<bool>("IsDeleted"),
                                   CountryAbbreviation = row.Field<string>("CountryAbbreviation"),
                                   CountryName = row.Field<string>("CountryName"),
                                   CountryCallingCode = row.Field<string>("CountryCallingCode"),
                                     // The Field<T>() method works with nullable types
                               }).ToList();
                        }
                    }
                }
            }
            Console.WriteLine(JsonConvert.SerializeObject(ret));
            return ret;
        }
        public void GetMultipleResultSetsUsingDataSet()
        {
            string ResultText = string.Empty;
            int RowsAffected = 0;
            DataSet ds = new DataSet();//Dataset is a collection of Datatables

            // Create SQL statement to submit
            string sql = CountryManager.COUNTRY_SQL;
            sql += ";" + CountryManager.STATE_SQL;

            // Create a connection
            using (SqlConnection cnn = new SqlConnection(Config.ConnectionString))
            {
                // Create command object
                using (SqlCommand cmd = new SqlCommand(sql, cnn))
                {
                    // Create a SQL Data Adapter
                    using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                    {
                        // Fill DataSet using Data Adapter            
                        da.Fill(ds);

                        if (ds.Tables.Count > 0)
                        {
                            Countries = ds.Tables[0];
                            States = ds.Tables[1];
                        }
                    }
                }
            }
            int index = 0;
            foreach(DataRow row in Countries.Rows)
            {
                Console.WriteLine("** Row: " + index.ToString() + " **");
                foreach (DataColumn col in Countries.Columns)
                {
                    Console.WriteLine(col.ColumnName + ": " + row[col.ColumnName].ToString());
                    index++;
                }
                Console.WriteLine();
            }
            int index1 = 0;
            foreach (DataRow row in States.Rows)
            {
                Console.WriteLine("** Row: " + index1.ToString() + " **");
                foreach (DataColumn col in States.Columns)
                {
                    Console.WriteLine(col.ColumnName + ": " + row[col.ColumnName].ToString());
                    index1++;
                }
                Console.WriteLine();
            }
        }
    }
  }
