using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace HRManagementSystem.Models
{
    public class Designation
    {
        public string Name { get; set; }
        public string id { get; set; }
    }

    public class DesignationBusinessService
    {
        public string connectionString
        {
            get
            {
                return ConfigurationManager.ConnectionStrings["HRManagement_ConnectionString"].ConnectionString;
            }
        }
        public DataSet DesignationSelectDB()
        {
            DataSet ds = new DataSet();
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlDataAdapter da = new SqlDataAdapter();
                SqlCommand command = new SqlCommand("SelectALLDesignation", connection);
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
        public DataTable DesignationSelect()
        {
            DataTable designationTable = null;
            DataSet ds = DesignationSelectDB();

            if ((ds != null) && (ds.Tables.Count > 0) && (ds.Tables[0].Rows.Count > 0))
                designationTable = ds.Tables[0];
            return designationTable;
        }
        public Designation AddDesignation(Designation designation)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand("Designation_CRUD", connection);
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.Add("@Name", SqlDbType.NVarChar).Value = designation.Name;
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
            return designation;
        }

        public bool EditDesignationName(Designation designation)
        {
            Int32 rowsAffected = 0;
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand("Designation_CRUD", connection);
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.Add("@ID", SqlDbType.Int).Value = designation.id;
                command.Parameters.Add("@Name", SqlDbType.NVarChar).Value = designation.Name;
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

            return rowsAffected > 0;
        }
        public DataTable RemoveDesignation(Designation designation)
        {
            DataTable removeDesignation = null;
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand("Designation_CRUD", connection);
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.Add("@Action", SqlDbType.NVarChar).Value = "DELETE";
                command.Parameters.Add("@ID", SqlDbType.Int).Value = designation.id;
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
            }
            return removeDesignation;
        }

    }
}