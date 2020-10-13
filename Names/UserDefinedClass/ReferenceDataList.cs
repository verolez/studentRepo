using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Reflection;

namespace UserDefinedClass
{
    public class ListOfDataFromDatabase
    {
        private readonly string storedProcName;
        private object modelObject;
        private readonly SqlParameter[] param;
        public List<object> GetAllDataListFromDB; //<= This property will be access outside
                                                  //Constructor

        public ListOfDataFromDatabase(string spName, object modelobj, SqlParameter[] param)
        {
            storedProcName = spName;
            modelObject = modelobj;
            this.param = param;
            LoadAllDataFromDatabase(); //Invoke invoked static method to load data to property
        }

        public ListOfDataFromDatabase(string spName, object modelobj)
        {
            storedProcName = spName;
            modelObject = modelobj;
            LoadAllDataFromDatabase(); //Invoke invoked static method to load data to property
        }


        private void LoadAllDataFromDatabase()
        {
            List<object> dataList = new List<object>();
            string connString = ConfigurationManager.ConnectionStrings["dbConString"].ConnectionString;
            using (SqlConnection conn = new SqlConnection(connString))
            {
                conn.Open();
                using (SqlCommand cmd = new SqlCommand())
                {
                    cmd.Connection = conn;
                    cmd.CommandText = storedProcName;
                    cmd.CommandType = CommandType.StoredProcedure;

                    if (this.param != null)
                    {
                        if (this.param.Length > 0)
                        {
                            cmd.Parameters.AddRange(param);
                        }
                    }

                    using (SqlDataReader rdr = cmd.ExecuteReader())
                    {
                        //int counter = -1;
                        if (rdr.FieldCount > 0)
                        {
                            while (rdr.Read())
                            {
                                object data = Activator.CreateInstance(modelObject.GetType());
                                if (modelObject != null)
                                {
                                    foreach (var Prop in modelObject.GetType().GetProperties())
                                    {
                                        if (CheckColumn.HasColumn(rdr, Prop.Name))
                                        {
                                            if (rdr[Prop.Name] != null && rdr[Prop.Name] != DBNull.Value)
                                            {
                                                if (!Prop.CanWrite)
                                                    continue;
                                                Prop.SetValue(data, Convert.ChangeType(rdr[Prop.Name] ?? Prop.GetType(), Prop.PropertyType));
                                            }
                                        }

                                        /* TODO */
                                        /* SKIP PROPERTIES NOT FOUND IN QUERY RESULT */
                                        /* CLOSE TODO */

                                    }
                                }
                                dataList.Add(data);
                            }
                        }
                    }
                }
            }
            GetAllDataListFromDB = dataList; //Load data to property
        }




        public static DataTable[] TableResult(string spName, SqlParameter[] sqlpars, string ConnectionString)
        {
            DataSet ds = new DataSet();
            SqlDataAdapter adap = new SqlDataAdapter(spName, ConnectionString);
            adap.SelectCommand.CommandType = CommandType.StoredProcedure;
            adap.SelectCommand.Parameters.AddRange(sqlpars);
            adap.Fill(ds);
            DataTable[] dtables = new DataTable[ds.Tables.Count];
            for (int i = 0; i < ds.Tables.Count; i++)
                dtables[i] = ds.Tables[i].Clone();
            ds.Dispose();
            return dtables;
        }



    }

    public class IndividualDataFromDatabase
    {
        private readonly string storedProcName;
        private object modelObject;
        private readonly SqlParameter[] param;
        public object IndividualDataRowFromDB; //<= This property will be access outside
                                               //Constructor

        public IndividualDataFromDatabase(string spName, object modelobj, SqlParameter[] param)
        {
            storedProcName = spName;
            modelObject = modelobj;
            this.param = param;
            LoadAllDataFromDatabase(); //Invoke invoked static method to load data to property
        }

        public IndividualDataFromDatabase(string spName, object modelobj)
        {
            storedProcName = spName;
            modelObject = modelobj;
            LoadAllDataFromDatabase(); //Invoke invoked static method to load data to property
        }


        private void LoadAllDataFromDatabase()
        {
            List<object> dataList = new List<object>();
            string connString = ConfigurationManager.ConnectionStrings["dbConString"].ConnectionString;
            using (SqlConnection conn = new SqlConnection(connString))
            {
                conn.Open();
                using (SqlCommand cmd = new SqlCommand())
                {
                    cmd.Connection = conn;
                    cmd.CommandText = storedProcName;
                    cmd.CommandType = CommandType.StoredProcedure;

                    if (this.param != null)
                    {
                        if (this.param.Length > 0)
                        {
                            cmd.Parameters.AddRange(param);
                        }
                    }

                    using (SqlDataReader rdr = cmd.ExecuteReader())
                    {
                        //int counter = -1;
                        if (rdr.FieldCount > 0)
                        {
                            while (rdr.Read())
                            {
                                object data = Activator.CreateInstance(modelObject.GetType());
                                if (modelObject != null)
                                {
                                    foreach (var Prop in modelObject.GetType().GetProperties())
                                    {
                                        //counter++;
                                        //var ttt = rdr.GetName(counter);
                                        //if (CheckColumn.HasColumn(rdr, Prop.Name))
                                        //{
                                        //    if (!Prop.CanWrite)
                                        //        continue;
                                        //    Prop.SetValue(data, Convert.ChangeType(rdr[Prop.Name], Prop.PropertyType));
                                        //}


                                        if (CheckColumn.HasColumn(rdr, Prop.Name))
                                        {
                                            if (rdr[Prop.Name] != null && rdr[Prop.Name] != DBNull.Value)
                                            {
                                                if (!Prop.CanWrite)
                                                    continue;
                                                //Prop.SetValue(data, Convert.ChangeType(rdr[Prop.Name], Prop.PropertyType));
                                                Prop.SetValue(data, Convert.ChangeType(rdr[Prop.Name], Prop.PropertyType));
                                            }
                                        }

                                        /* TODO */
                                        /* SKIP PROPERTIES NOT FOUND IN QUERY RESULT */ // <--- DONE
                                        /* CLOSE TODO */

                                    }
                                }
                                //dataList.Add(data);
                                IndividualDataRowFromDB = data;
                            }
                        }
                    }
                }
            }
            // IndividualDataRowFromDB = dataList; //Load data to property
        }

    }

    public static class CheckColumn
    {
        public static bool HasColumn(this IDataRecord dr, string columnName)
        {
            for (int i = 0; i < dr.FieldCount; i++)
            {
                if (dr.GetName(i).Equals(columnName, StringComparison.InvariantCultureIgnoreCase))
                    return true;
            }
            return false;
        }
    }

    #region TVP
    public static class TVPTable
    {
        public static DataTable CreateDataTableList<T>(IEnumerable<T> list)
        {
            Type type = typeof(T);
            var properties = type.GetProperties();

            DataTable dataTable = new DataTable();
            foreach (PropertyInfo info in properties)
            {
                dataTable.Columns.Add(new DataColumn(info.Name)); //, Nullable.GetUnderlyingType(info.PropertyType) ?? info.PropertyType
            }

            if (list != null)
            {

                foreach (T entity in list)
                {
                    object[] values = new object[properties.Length];
                    for (int i = 0; i < properties.Length; i++)
                    {
                        if (properties[i].PropertyType.IsGenericType && properties[i].PropertyType.GetGenericTypeDefinition() == typeof(List<>))
                            values[i] = null;
                        else
                            values[i] = properties[i].GetValue(entity);

                    }

                    dataTable.Rows.Add(values);
                }
            }
            else
            {

            }
            return dataTable;
        }
    }

    #endregion
}
