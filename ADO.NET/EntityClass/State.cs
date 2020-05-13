using System;
using System.Collections.Generic;
using System.Text;

namespace ADO.NET.EntityClass
{
     public class State
    {
        public int CountryId { get; set; }
        public bool IsDeleted { get; set; }
        public string StateAbbreviation { get; set; }
        public string StateName { get; set; }
    }
}
