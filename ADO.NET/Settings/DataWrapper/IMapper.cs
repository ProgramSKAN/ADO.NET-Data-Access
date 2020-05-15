using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Text;

namespace ADO.NET.Settings.DataWrapper
{
    public interface IMapper
    {
        /// Maps raw SQL Row to CLR Object.
        T MapDataToObject<T>(SqlDataReader reader) where T : class, new();
    }
}
