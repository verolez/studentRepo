using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using BO;
using UserDefinedClass;

namespace DAL
{
    public static class StudentData
    {
        public static IEnumerable<NameObject> GetNames()
        {
            IList<object> objectList = new List<object>();
            NameObject nameObject = new NameObject();
            ICollection<NameObject> nameObjectList = new List<NameObject>();

            ListOfDataFromDatabase listOfDataFromDatabase = new ListOfDataFromDatabase("spGetStudentName", nameObject);

            objectList = listOfDataFromDatabase.GetAllDataListFromDB;

            for (int i = 0; i < objectList.Count; i++)
            {
                nameObject = (NameObject)objectList[i];
                nameObjectList.Add(nameObject);
            }

            return nameObjectList;
        }


        public static string InsertStudentName(NameObject name)
        {
            SqlParameter[] sqlParameters = new SqlParameter[3];
            sqlParameters[0] = new SqlParameter()
            {
                ParameterName = "@FirstName",
                SqlDbType = SqlDbType.VarChar,
                Value = name.FirstName,
                Size = 50
            };

            sqlParameters[1] = new SqlParameter()
            {
                ParameterName = "@LastName",
                SqlDbType = SqlDbType.VarChar,
                Value = name.LastName,
                Size = 50
            };
            sqlParameters[2] = new SqlParameter()
            {
                ParameterName = "@ReturnValue",
                SqlDbType = SqlDbType.VarChar,
                Size = 140,
                Direction = ParameterDirection.Output
            };


            return DbInsert.Save("spInsertStudentName", sqlParameters);
        }


        public static string UpdateStudentInfo(NameObject name)
        {
            SqlParameter[] sqlParameters = new SqlParameter[4];


            sqlParameters[0] = new SqlParameter()
            {
                ParameterName = "@ID",
                SqlDbType = SqlDbType.VarChar,
                Value = name.ID,
                Size = 50
            };
            sqlParameters[1] = new SqlParameter()
            {
                ParameterName = "@FirstName",
                SqlDbType = SqlDbType.VarChar,
                Value = name.FirstName,
                Size = 50
            };

            sqlParameters[2] = new SqlParameter()
            {
                ParameterName = "@LastName",
                SqlDbType = SqlDbType.VarChar,
                Value = name.LastName,
                Size = 50
            };
            sqlParameters[3] = new SqlParameter()
            {
                ParameterName = "@ReturnValue",
                SqlDbType = SqlDbType.VarChar,
                Size = 140,
                Direction = ParameterDirection.Output
            };


            return DbInsert.Save("spUpdateStudentName", sqlParameters);
        }

        public static string RemoveStudent(int ID)
        {
            SqlParameter[] sqlParameters = new SqlParameter[2];


            sqlParameters[0] = new SqlParameter()
            {
                ParameterName = "@ID",
                SqlDbType = SqlDbType.VarChar,
                Value = ID,
                Size = 50
            };
            sqlParameters[1] = new SqlParameter()
            {
                ParameterName = "@ReturnValue",
                SqlDbType = SqlDbType.VarChar,
                Size = 140,
                Direction = ParameterDirection.Output
            };

            return DbInsert.Save("spRemoveStudent", sqlParameters);
        }


    }
}
