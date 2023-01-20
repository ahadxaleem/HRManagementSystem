using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace HRManagementSystem.Models
{
    public class Employee
    {
        public string EmployeeCode { get; set; }
        public string EmployeeName { get; set; }
        public string JoinDate { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string Designation { get; set; }
        public string Department { get; set; }
        public string Branch { get; set; }
        public string ConfirmPassword { get; set; }
        public byte[] EmployeeMediaContents
        { set; get; }
        public string EmployeeMediaContentType
        { set; get; }
        public string DisplayImage { get; set; }

    }

    public class EmployeeBusinessService
    {
        //public string connectionString { get; set; }

        public string connectionString
        {
            get
            {
                return ConfigurationManager.ConnectionStrings["HRManagement_ConnectionString"].ConnectionString;
            }
        }

        public Employee ParseFromJSON(string items)
        {
            Employee e = JsonConvert.DeserializeObject<Employee>(items);
            return e;
        }
        public bool Employee_CRUD(Employee emp, string operationMode = "INSERT")
        {
            Int32 rowsAffected = 0;
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand("Employee_CRUD", connection);
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.Add("@Action", SqlDbType.NVarChar).Value = operationMode;
                command.Parameters.Add("@EmployeeCode", SqlDbType.NVarChar).Value = emp.EmployeeCode;
                command.Parameters.Add("@EmployeeName", SqlDbType.NVarChar).Value = emp.EmployeeName;
                command.Parameters.Add("@JoinDate", SqlDbType.NVarChar).Value = emp.JoinDate;
                command.Parameters.Add("@Department", SqlDbType.NVarChar).Value = emp.Department;
                command.Parameters.Add("@Branch", SqlDbType.NVarChar).Value = emp.Branch;
                command.Parameters.Add("@Email", SqlDbType.NVarChar).Value = emp.Email;
                command.Parameters.Add("@EmployeeMediaContents", SqlDbType.Image).Value = emp.EmployeeMediaContents;
                command.Parameters.Add("@EmployeeMediaContentType", SqlDbType.NVarChar).Value = emp.EmployeeMediaContentType;

                try
                {
                    connection.Open();
                    rowsAffected = command.ExecuteNonQuery();
                    Console.WriteLine("RowsAffected: {0}", rowsAffected);
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }

            return rowsAffected > 0 ? true : false;
        }

        public DataSet User_SelectFromDB(string Email, string Password)
        {
            DataSet ds = new DataSet();

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlDataAdapter da = new SqlDataAdapter();
                SqlCommand command = new SqlCommand("Users_Select", connection);
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.Add("@Email", SqlDbType.NVarChar).Value = Email;
                command.Parameters.Add("@Password", SqlDbType.NVarChar).Value = Password;
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
        public DataSet Employee_SelectFromDB(string empId)
        {
            DataSet ds = new DataSet();

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlDataAdapter da = new SqlDataAdapter();
                SqlCommand command = new SqlCommand("Employee_Select", connection);
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.Add("@EmployeeCode", SqlDbType.NVarChar).Value = empId;
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

        public DataSet Employee_SelectAll_DB()
        {
            DataSet ds = new DataSet();
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlDataAdapter da = new SqlDataAdapter();
                SqlCommand command = new SqlCommand("Employee_SelectAll", connection);
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
        public Employee CreateEmployeeFromDataRow(DataRow dr)
        {
            if (dr != null)
            {
                Employee emp = new Employee();

                emp.EmployeeCode = GetString(dr, "EmployId");
                emp.EmployeeName = GetString(dr, "EmployName");
                emp.JoinDate = GetString(dr, "JoinDate");
                emp.Department = GetString(dr, "Department");
                emp.Designation = GetString(dr, "EmployDesignation");
                emp.Branch = GetString(dr, "EmployBranch");
                emp.EmployeeMediaContents = GetImage(dr, "EmployeeMedia");

                return emp;
            }
            return null;
        }


        public Employee Employee_Select(string empID)
        {
            DataSet ds = Employee_SelectFromDB(empID);

            if ((ds != null) && (ds.Tables.Count > 0) && (ds.Tables[0].Rows.Count > 0))
                return CreateEmployeeFromDataRow(ds.Tables[0].Rows[0]);

            return null;
        }
        public DataTable Employee_SelectAll()
        {
            DataTable employeeTable = null;
            DataSet ds = Employee_SelectAll_DB();

            if ((ds != null) && (ds.Tables.Count > 0) && (ds.Tables[0].Rows.Count > 0))
                employeeTable = ds.Tables[0];
            return employeeTable;
        }

        protected static string GetString(DataRow row, string columnName)
        {
            return (row[columnName] != DBNull.Value) ? Convert.ToString(row[columnName]) : string.Empty;
        }

        protected static Byte[] GetImage(DataRow row, string columnName)
        {
            return (row[columnName] != DBNull.Value) ? (Byte[])row[columnName] : new Byte[] { };
        }
    }

}