using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TestWork.Models
{
    public class CreateGroupModel
    {
        public int IdGroup { get; set; }
        public string NameStudyGroup { get; set; }
        public int IdTeacher { get; set; }
        public int IdCourse { get; set; }
        public virtual TeachersModel Teachers { get; set; }
    }
}