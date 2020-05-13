using ADO.NET.EntityClass;
using ADO.NET.ManagerClasses;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;

namespace ADO.NET.Settings.Connect_and_Submit_Sql_To_Db
{
    public static class DataReader
    {
        private static ObservableCollection<Country> _Countries = new ObservableCollection<Country>();
        private static ObservableCollection<State> _State = new ObservableCollection<State>();
        public static ObservableCollection<Country> Countries
        {
            get { return _Countries; }
            set
            {
                _Countries = value;
            }
        }
        public static ObservableCollection<State> States
        {
            get { return _State; }
            set
            {
                _State = value;
            }
        }
        public static void GetCountriesAsDataReader()
        {
            string ResultText = null;
            StringBuilder sb = new StringBuilder(1024);
            using (SqlConnection cnn = new SqlConnection(Config.ConnectionString))
            {
                cnn.Open();
                using (SqlCommand cmd = new SqlCommand(CountryManager.COUNTRY_SQL, cnn))
                {
                    using (SqlDataReader dr = cmd.ExecuteReader(CommandBehavior.CloseConnection))
                    {
                        while (dr.Read())
                        {
                            sb.AppendLine("CountryId: " + dr["CountryId"].ToString());
                            sb.AppendLine("IsDeleted: " + dr["IsDeleted"].ToString());
                            sb.AppendLine("CountryAbbreviation: " + dr["CountryAbbreviation"].ToString());
                            sb.AppendLine("CountryName: " + dr["CountryName"].ToString());
                            sb.AppendLine("CountryCallingCode: " + dr["CountryCallingCode"].ToString());
                            sb.AppendLine();
                        }
                    }
                }
            }

            ResultText = sb.ToString();
            Console.WriteLine(ResultText);
        }
        public static void GetCountriesAsGenericList()
        {
            Countries.Clear();
            string ResultText = null;
            using (SqlConnection cnn = new SqlConnection(Config.ConnectionString))
            {
                cnn.Open();
                using (SqlCommand cmd = new SqlCommand(CountryManager.COUNTRY_SQL, cnn))
                {
                    using (SqlDataReader dr = cmd.ExecuteReader(CommandBehavior.CloseConnection))
                    {
                        while (dr.Read())
                        {
                            Countries.Add(new Country
                            {
                                CountryId = dr.GetInt32(dr.GetOrdinal("Countryid")),
                                IsDeleted = dr.GetBoolean(dr.GetOrdinal("IsDeleted")),
                                CountryAbbreviation = dr.GetString(dr.GetOrdinal("CountryAbbreviation")),
                                CountryName = dr.GetString(dr.GetOrdinal("CountryName")),
                                CountryCallingCode = dr.IsDBNull(dr.GetOrdinal("CountryCallingCode"))
                                                        ? "shound not be null"
                                                        : dr["CountryCallingCode"].ToString()
                            });

                        }
                    }
                }
            }
            foreach(var item in Countries)
            {
                Console.WriteLine(JsonConvert.SerializeObject(item));
            }
            
        }
        public static void GetCountriesAsGenericListUsingGetFieldValue()
        {
            Countries.Clear();
            string ResultText = null;
            using (SqlConnection cnn = new SqlConnection(Config.ConnectionString))
            {
                cnn.Open();
                using (SqlCommand cmd = new SqlCommand(CountryManager.COUNTRY_SQL, cnn))
                {
                    using (SqlDataReader dr = cmd.ExecuteReader(CommandBehavior.CloseConnection))
                    {
                        while (dr.Read())
                        {
                            Countries.Add(new Country
                            {
                                CountryId = dr.GetFieldValue<int>(dr.GetOrdinal("Countryid")),
                                IsDeleted = dr.GetFieldValue<bool>(dr.GetOrdinal("IsDeleted")),
                                CountryAbbreviation = dr.GetFieldValue<string>(dr.GetOrdinal("CountryAbbreviation")),
                                CountryName = dr.GetFieldValue<string>(dr.GetOrdinal("CountryName")),
                                //GetFieldValue<T>() does not work on below nullable type
                                CountryCallingCode = dr.IsDBNull(dr.GetOrdinal("CountryCallingCode"))
                                                        ? "shound not be null"
                                                        : dr["CountryCallingCode"].ToString()
                            });

                        }
                    }
                }
            }
            foreach (var item in Countries)
            {
                Console.WriteLine(JsonConvert.SerializeObject(item));
            }

        }
        public static void GetCountriesAsGenericListUsingCustomGetFieldValue()
        {
            Countries.Clear();
            string ResultText = null;
            using (SqlConnection cnn = new SqlConnection(Config.ConnectionString))
            {
                cnn.Open();
                using (SqlCommand cmd = new SqlCommand(CountryManager.COUNTRY_SQL, cnn))
                {
                    using (SqlDataReader dr = cmd.ExecuteReader(CommandBehavior.CloseConnection))
                    {
                        while (dr.Read())
                        {
                            Countries.Add(new Country
                            {
                                CountryId = dr.CustomGetFieldValue<int>("CountryId"),
                                IsDeleted = dr.CustomGetFieldValue<bool>("IsDeleted"),
                                CountryAbbreviation = dr.CustomGetFieldValue<string>("CountryAbbreviation"),
                                CountryName = dr.CustomGetFieldValue<string>("CountryName"),
                                CountryCallingCode = dr.CustomGetFieldValue<string>("CountryCallingCode")
                            });

                        }
                    }
                }
            }
            foreach (var item in Countries)
            {
                Console.WriteLine(JsonConvert.SerializeObject(item));
            }

        }
        public static void GetMultipleResultSets()
        {
            Countries.Clear();
            string sql = CountryManager.COUNTRY_SQL;
            sql += ";" + CountryManager.STATE_SQL;
            Console.WriteLine(sql);
            using (SqlConnection cnn = new SqlConnection(Config.ConnectionString))
            {
                cnn.Open();
                using (SqlCommand cmd = new SqlCommand(sql, cnn))
                {
                    using (SqlDataReader dr = cmd.ExecuteReader(CommandBehavior.CloseConnection))
                    {
                        while (dr.Read())
                        {
                            Countries.Add(new Country
                            {
                                CountryId = dr.CustomGetFieldValue<int>("CountryId"),
                                IsDeleted = dr.CustomGetFieldValue<bool>("IsDeleted"),
                                CountryAbbreviation = dr.CustomGetFieldValue<string>("CountryAbbreviation"),
                                CountryName = dr.CustomGetFieldValue<string>("CountryName"),
                                CountryCallingCode = dr.CustomGetFieldValue<string>("CountryCallingCode")
                            });

                        }
                        dr.NextResult();//move to next result set
                        while (dr.Read())
                        {
                            States.Add(new State
                            {
                                CountryId = dr.CustomGetFieldValue<int>("CountryId"),
                                IsDeleted = dr.CustomGetFieldValue<bool>("IsDeleted"),
                                StateAbbreviation = dr.CustomGetFieldValue<string>("StateAbbreviation"),
                                StateName = dr.CustomGetFieldValue<string>("StateName")
                            });

                        }
                    }
                }
            }
            foreach (var item in Countries)
            {
                Console.WriteLine(JsonConvert.SerializeObject(item));
            }
            foreach (var item in States)
            {
                Console.WriteLine(JsonConvert.SerializeObject(item));
            }

        }
    }
}
