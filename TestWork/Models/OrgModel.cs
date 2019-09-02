using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TestWork.Models
{
    public class OrgModel
    {
        public List<OrgModel> EmployeeList { get; set; }
        public int IdOrganization { get; set; }
        public string NameOrganization { get; set; }
        public int IdTeacher { get; set; }

    }
}