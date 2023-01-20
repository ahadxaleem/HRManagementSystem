using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Configuration;
using System.Data.SqlClient;
using System.Data;

namespace HRManagementSystem.Models
{
    public class Departments
    {
        public string Name { get; set; }
        public string id { get; set; }
    }

    public class DepartmentBusinessService
    {
        public string connectionString
        {
            get
            {
                return ConfigurationManager.ConnectionStrings["HRManagement_ConnectionString"].ConnectionString;
            }
        }
        public Departments AddDepartment(Departments Department)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand("Departments_CRUD", connection);
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.Add("@Name", SqlDbType.NVarChar).Value = Department.Name;
                try
                {
                    connection.Open();
                    Int32 rowsAffected = command.ExecuteNonQuery();
                    Console.WriteLine("RowsAffected: {0}", rowsAffected);
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }
            return Department;
        }

        public DataSet DepartmentsSelectDB()
        {
            DataSet ds = new DataSet();
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlDataAdapter da = new SqlDataAdapter();
                SqlCommand command = new SqlCommand("SelectDepartments", connection);
                command.CommandType = CommandType.StoredProcedure;
                da.SelectCommand = (SqlCommand)command;
                try
                {
                    connection.Open();
                    da.Fill(ds);
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }
            return ds;
        }
        public DataTable DapartmentsSelect()
        {
            DataTable departmentTable = null;
            DataSet ds = DepartmentsSelectDB();

            if ((ds != null) && (ds.Tables.Count > 0) && (ds.Tables[0].Rows.Count > 0))
                departmentTable = ds.Tables[0];
            return departmentTable;
        }

        public int EditDepartmentName(Departments dpt)
        {
            DataSet ds = new DataSet();
            Int32 rowsAffected = 0;
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand("Departments_CRUD", connection);
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.Add("@ID", SqlDbType.Int).Value = dpt.id;
                command.Parameters.Add("@Name", SqlDbType.NVarChar).Value = dpt.Name;
                try
                {
                    connection.Open();
                    rowsAffected = command.ExecuteNonQuery();
                    //ds = rowsAffected;
                    Console.WriteLine("RowsAffected: {0}", rowsAffected);
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }
            return rowsAffected;
        }
        public DataTable RemoveDepartment(Departments dpt)
        {
            DataTable removeDepartment = null;
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand("Departments_CRUD", connection);
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.Add("@Action", SqlDbType.NVarChar).Value = "DELETE";
                command.Parameters.Add("@ID", SqlDbType.Int).Value = dpt.id;
                command.Parameters.Add("@Name", SqlDbType.NVarChar).Value = "remove";
                try
                {
                    connection.Open();
                    Int32 rowsAffected = command.ExecuteNonQuery();
                    Console.WriteLine("RowsAffected: {0}", rowsAffected);
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
                //DataSet ds = RemoveDepartmentDB(dpt);
                //if ((ds != null) && (ds.Tables.Count > 0) && (ds.Tables[0].Rows.Count > 0))
                //    departmentRow = ds.Tables[0];
                return removeDepartment;
            }
        }
    }
}