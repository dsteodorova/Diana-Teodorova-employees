using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PairOfEmployees
{
    public class EmployeeProject
    {
        public string EmpID { get; set; }
        public string ProjectID { get; set; }
        public DateTime DateFrom { get; set; }
        public DateTime DateTo { get; set; }
    }
}