using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TestWork.Models
{
    public class LearningGroupsModel
    {
        public int IdStudyGroup { get; set; }
        public string NameStudyGroup { get; set; }
        public string FioTeacher { get; set; }
        public int CountOfStudents { get; set; }

    }
}