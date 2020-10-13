using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL;
using BO;

namespace BL
{
    public static class StudentLogic
    {
        public static IEnumerable<NameObject> GetNames() => StudentData.GetNames();
        public static string InsertStudentName(NameObject name) => StudentData.InsertStudentName(name);
        public static string UpdateStudentName(NameObject name) => StudentData.UpdateStudentInfo(name);
        public static string RemoveStudent(int ID) => StudentData.RemoveStudent(ID);
    }
}
