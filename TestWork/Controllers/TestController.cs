using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Diagnostics;
using System.Web.Mvc;
using System.Data.SqlClient;
using TestWork.Models;

namespace TestWork.Controllers
{
    public class TestController : Controller
    {
        static string connectionString = @"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename='|DataDirectory|\TestDB.mdf';Integrated Security=True";
        
        
        public ActionResult List()
        {
            List<LearningGroupsModel> learningGroups = GetGroupsToList();
            return View(learningGroups);
        }

        private static List<LearningGroupsModel> GetGroupsToList()
        {
            string expressSql = "ForMainPage";
            List<LearningGroupsModel> mainList = new List<LearningGroupsModel>();

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                SqlCommand command = new SqlCommand(expressSql, connection);
                command.CommandType = System.Data.CommandType.StoredProcedure;
                var groups = command.ExecuteReader();
                if (groups.HasRows)
                {
                    while (groups.Read())
                    {
                        LearningGroupsModel g = new LearningGroupsModel();
                        g.IdStudyGroup = groups.GetInt32(0);
                        g.NameStudyGroup = groups.GetString(1);
                        g.FioTeacher = groups.GetString(2);
                        g.CountOfStudents = groups.GetInt32(3);
                        mainList.Add(g);
                    }
                }
                groups.Close();
                return mainList;
            }
        }
        [HttpGet]
        public ActionResult Create()
        {
            SelectList teachers = new SelectList(GetTeachers(), "IdTeacher", "NameTeacher");
            ViewBag.TeacherDropList = teachers;
            return View();
        }
        [HttpPost]
        public ActionResult Create(CreateGroupModel group)
        {
            return RedirectToAction("Edit", new { id = AddGroup(group) });
        }
        [HttpGet]
        public ActionResult Edit(int id)
        {
            EditStudyGroupModel editStudyGroupModel = new EditStudyGroupModel
            {
                EmployeeList = GetStudentsOfGroup(id),
                IdStudyGroup = id,
                NameStudyGroup = GetGroup(id),
                NameTeacher = GetTeacher(id).NameTeacher
            };
            return View(editStudyGroupModel);
        }
        [HttpPost]
        public ActionResult Edit(CreateGroupModel group)
        {
            ChangeGroup(group);
            return RedirectToAction("Edit", new { id = group.IdGroup });
        }
 //       [HttpPost]
        public ActionResult Delete(int idStudent, int studyGroupId)
        {
            DeleteStudent(idStudent, studyGroupId);
            return RedirectToAction("Edit", new { id = studyGroupId });
        }
        [HttpGet]
        public ActionResult CreateStudent(int IdGroup)
        {
            TeachersModel teach = GetTeacher(IdGroup);
            List<OrgModel> Orgs = GetOrg(teach.IdTeacher);
            int selectedIndex = Orgs.ElementAt(0).IdOrganization;
            List<EmployeeModel> Employee = GetEmployee(selectedIndex);
            CreateStudentModel createStudentModel = new CreateStudentModel
            {
                EmployeeList = GetStudentsOfGroup(IdGroup),
                IdStudyGroup = IdGroup,
                NameStudyGroup = GetGroup(IdGroup),
                NameTeacher = teach.NameTeacher,
                Organizations = new SelectList(Orgs, "IdOrganization", "NameOrganization"),
                Employee = new SelectList(Employee, "IdEmployee", "NameEmployee")
            };
            return View(createStudentModel);
        }
        [HttpPost]
        public ActionResult CreateStudent(CreateStudentModel student)
        {
            string expressSql = "addStudent2";
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    SqlCommand command = new SqlCommand(expressSql, connection)
                    {
                        CommandType = System.Data.CommandType.StoredProcedure
                    };
                    SqlParameter namePar = new SqlParameter // parameter for enter name
                    {
                        ParameterName = "@idGroup",
                        Value = student.IdStudyGroup
                    };
                    command.Parameters.Add(namePar); // add
                    SqlParameter idStudPar = new SqlParameter // parameter for id
                    {
                        ParameterName = "@idEmpl",
                        Value = student.IdEmployee
                    };
                    command.Parameters.Add(idStudPar);
                    var result = command.ExecuteNonQuery();
                }
            return RedirectToAction("Edit", new { id = student.IdStudyGroup });
        }
        private void DeleteStudent(int id, int studyGroupId)
        {
            string expressSql = "deleteStudent";
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                SqlCommand command = new SqlCommand(expressSql, connection);
                command.CommandType = System.Data.CommandType.StoredProcedure;
                SqlParameter Param = new SqlParameter // parameter for enter id
                {
                    ParameterName = "@idStudent",
                    Value = id
                };
                command.Parameters.Add(Param); // add
                SqlParameter groupIdParam = new SqlParameter
                {
                    ParameterName = "@IdGroup",
                    Value = studyGroupId
                };
                command.Parameters.Add(groupIdParam);
                command.ExecuteNonQuery();
            }
        }
        private static void ChangeGroup(CreateGroupModel group)
        {
            string sqlExpression = "updateNameGroup2";
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                SqlCommand command = new SqlCommand(sqlExpression, connection);
                command.CommandType = System.Data.CommandType.StoredProcedure;
                SqlParameter nameParam = new SqlParameter
                {
                    ParameterName = "@NewNameGr",
                    Value = group.NameStudyGroup
                };
                command.Parameters.Add(nameParam);
                SqlParameter groupIdParam = new SqlParameter
                {
                    ParameterName = "@IdGroup",
                    Value = group.IdGroup
                };
                command.Parameters.Add(groupIdParam);
                command.ExecuteNonQuery();
            }
        }
        public List<EmployeeModel> GetStudentsOfGroup(int IdGroup)
        {
            string expressSql = "GetStudentsInGroup";
            List<EmployeeModel> students = new List<EmployeeModel>();
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                SqlCommand command = new SqlCommand(expressSql, connection);
                command.CommandType = System.Data.CommandType.StoredProcedure;
                SqlParameter idGroupPar = new SqlParameter // parameter for id
                {
                    ParameterName = "@idSG",
                    Value = IdGroup
                };
                command.Parameters.Add(idGroupPar);
                var employee = command.ExecuteReader();
                if (employee.HasRows)
                {
                    while (employee.Read())
                    {
                        EmployeeModel e = new EmployeeModel
                        {
                            IdEmployee = employee.GetInt32(0),
                            NameEmployee = employee.GetString(1),
                            NameOrganization = employee.GetString(2)
                        };
                        students.Add(e);
                    }
                }
                employee.Close();
                return students;
            }
        }
        private static List<TeachersModel> GetTeachers()
        {
            string expressSql = "getTeachers";
            List<TeachersModel> allTeachers = new List<TeachersModel>();
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                SqlCommand command = new SqlCommand(expressSql, connection);
                command.CommandType = System.Data.CommandType.StoredProcedure;
                var teachers = command.ExecuteReader();
                if (teachers.HasRows)
                {
                    while (teachers.Read())
                    {
                        TeachersModel t = new TeachersModel
                        {
                            IdTeacher = teachers.GetInt32(0),
                            NameTeacher = teachers.GetString(1)
                        };
                        allTeachers.Add(t);
                    }
                }
                teachers.Close();
                return allTeachers;
            }
        }

        private TeachersModel GetTeacher(int IdGroup)
        {
            string expressSql = "getTeacher";
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                SqlCommand command = new SqlCommand(expressSql, connection);
                command.CommandType = System.Data.CommandType.StoredProcedure;
                SqlParameter TeacherIdPar = new SqlParameter // parameter for id
                {
                    ParameterName = "@IdGroup",
                    Value = IdGroup
                };
                command.Parameters.Add(TeacherIdPar); // add
                var g = command.ExecuteReader();
                g.Read();
                TeachersModel res = new TeachersModel
                {
                    IdTeacher= g.GetInt32(0),
                    NameTeacher = g.GetString(1)
                };
                g.Close();
                return res;
            }
        }
        private int AddGroup(CreateGroupModel group)
        {
            string expressSql = "createStudyGroup";
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                SqlCommand command = new SqlCommand(expressSql, connection)
                {
                    CommandType = System.Data.CommandType.StoredProcedure
                };
                SqlParameter namePar = new SqlParameter // parameter for enter name
                {
                    ParameterName = "@nameSG",
                    Value = group.NameStudyGroup
                };
                command.Parameters.Add(namePar); // add
                SqlParameter idTeacherPar = new SqlParameter // parameter for id
                {
                    ParameterName = "@IdTeach",
                    Value = group.IdTeacher
                };
                command.Parameters.Add(idTeacherPar);
                int result = (int)command.ExecuteScalar();
                return result;
            }
        }
        private string GetGroup(int IdGroup)
        {
            string expressSql = "getGroup";
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                SqlCommand command = new SqlCommand(expressSql, connection);
                command.CommandType = System.Data.CommandType.StoredProcedure;
                SqlParameter GroupIdPar = new SqlParameter // parameter for id
                {
                    ParameterName = "@IdGroup",
                    Value = IdGroup
                };
                command.Parameters.Add(GroupIdPar); // add
                var g = command.ExecuteReader();
                g.Read();
                var res = g.GetString(0);
                g.Close();
                return res;
            }
        }
        private static List<EmployeeModel> GetEmployee(int IdOrganization)
        {
            string expressSql = "getEmployee";
            List<EmployeeModel> allEmployee = new List<EmployeeModel>();
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                SqlCommand command = new SqlCommand(expressSql, connection);
                command.CommandType = System.Data.CommandType.StoredProcedure;
                SqlParameter orgIdParam = new SqlParameter
                {
                    ParameterName = "@organizationId",
                    Value = IdOrganization
                };
                command.Parameters.Add(orgIdParam);
                var employee = command.ExecuteReader();
                if (employee.HasRows)
                {
                    while (employee.Read())
                    {
                        EmployeeModel e = new EmployeeModel
                        {
                            IdEmployee = employee.GetInt32(0),
                            NameEmployee = employee.GetString(1)
                        };
                        allEmployee.Add(e);
                    }
                }
                employee.Close();
                return allEmployee;
            }
        }

        private static List<OrgModel> GetOrg(int idTeacher)
        {
            string expressSql = "getOrganization";
            List<OrgModel> allOrganizations = new List<OrgModel>();
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                SqlCommand command = new SqlCommand(expressSql, connection);
                command.CommandType = System.Data.CommandType.StoredProcedure;
                SqlParameter teacherIdParam = new SqlParameter
                {
                    ParameterName = "@idTeacher",
                    Value = idTeacher
                };
                command.Parameters.Add(teacherIdParam);
                var organizations = command.ExecuteReader();
                if (organizations.HasRows)
                {
                    while (organizations.Read())
                    {
                        OrgModel e = new OrgModel
                        {
                            IdOrganization = organizations.GetInt32(0),
                            NameOrganization = organizations.GetString(1),
                            IdTeacher = idTeacher
                        };
                        allOrganizations.Add(e);
                    }
                }
                organizations.Close();
                return allOrganizations;
            }
        }
        public ActionResult GetItems(int id)
        {
            return PartialView(GetEmployee(id));
        }
    }
}