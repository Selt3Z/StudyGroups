using System.Collections.Generic;

namespace TestWork.Models
{
    public class EditStudyGroupModel
    {
        public List<EmployeeModel> EmployeeList { get; set; }

        public int IdStudyGroup { get; set; }
        public string NameStudyGroup { get; set; }
        public string NameTeacher { get; set; }
    }
}