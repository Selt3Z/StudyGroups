using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace TestWork.Models
{
    public class CreateStudentModel
    {
        public List<EmployeeModel> EmployeeList { get; set; }
        public SelectList Organizations { get; set; }
        public SelectList Employee { get; set; }
        public int IdStudyGroup { get; set; }
        public string NameStudyGroup { get; set; }
        public int IdTeacher { get; set; }
        public string NameTeacher { get; set; }
        public int IdOrganization { get; set; }
        public string NameOrganization { get; set; }
        public int IdEmployee { get; set; }
        public string NameEmployee { get; set; }

    }
}