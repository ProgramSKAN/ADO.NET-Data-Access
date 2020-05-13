using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Text;

namespace ADO.NET.Settings
{
    public static class DataHelper
    {
        public static T CustomGetFieldValue<T>(this SqlDataReader dr,string name)
        {
            T ret = default;
            if (!dr[name].Equals(DBNull.Value))
            {
                ret = (T)dr[name];
            }
            return ret;
        }
    }
}
