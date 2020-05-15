using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Text;

namespace ADO.NET.Settings.DataWrapper.Repository
{
    public abstract class RepositoryBase
    {
        protected virtual CustomException GetCustomExceptionOn(SqlException ex)
        {
            return new CustomException()
            {
                ErrorMessage = ex.Message,
                StatusCode = System.Net.HttpStatusCode.BadRequest,
                ExceptionTrace = ex.StackTrace
            };
        }
        protected virtual CustomException GetCustomExceptionOn(Exception ex)
        {
            return new CustomException()
            {
                ErrorMessage = ex.Message,
                StatusCode = System.Net.HttpStatusCode.BadGateway,
                ExceptionTrace = ex.StackTrace
            };
        }
    }
}
