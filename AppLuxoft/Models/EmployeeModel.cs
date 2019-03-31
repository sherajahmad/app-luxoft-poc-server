using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AppLuxoft.Models
{
    public class EmployeeModel
    {
        public List<employee> Employees { get; set; }
        public int PageIndex { get; set; }
        public int PageSize { get; set; }
        public int RecordCount { get; set; }
    }
}