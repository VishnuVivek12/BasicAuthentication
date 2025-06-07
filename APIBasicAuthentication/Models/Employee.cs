using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace APIBasicAuth.Models
{
        public class Employee
        {
                public int Id { get; set; }
                public string Name { get; set; }
                public string Department { get; set; }
                public float Salary { get; set; }
        }
}