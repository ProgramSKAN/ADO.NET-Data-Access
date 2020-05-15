using System;
using System.Collections.Generic;
using System.Net;
using System.Text;

namespace ADO.NET.Settings.DataWrapper.Repository
{
    public class CustomException
    {
        public HttpStatusCode StatusCode { get; set; }
        public string ErrorMessage { get; set; }
        public string ExceptionTrace { get; set; }
    }

}
