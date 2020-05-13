using System;
using System.Collections.Generic;
using System.Text;

namespace ADO.NET.EntityClass
{
    public class Country
    {
        public int CountryId { get; set; }
        public bool IsDeleted { get; set; }
        public string CountryAbbreviation { get; set; }
        public string CountryName { get; set; }
        public string CountryCallingCode { get; set; }
    }
}
