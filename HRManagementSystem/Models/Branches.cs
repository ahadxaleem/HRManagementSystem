using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.SqlClient;
using System.Configuration;
using System.Data;

namespace HRManagementSystem.Models
{
    public class Branches
    {
        public string Name { get; set; }
        public string id { get; set; }
    }
    public class BranchesBusinessService
    {
        public string connectionString
        {
            get 
            {
                return ConfigurationManager.ConnectionStrings["HRManagement_ConnectionString"].ConnectionString;
            }
        }
        public DataSet BranchesSelectDB()
        {
            DataSet ds = new DataSet();
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlDataAdapter da = new SqlDataAdapter();
                SqlCommand command = new SqlCommand("SelectBranches", connection);
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
        public DataTable BranchesSelect()
        {
            DataTable branchTable = null;
            DataSet ds = BranchesSelectDB();

            if ((ds != null) && (ds.Tables.Count > 0) && (ds.Tables[0].Rows.Count > 0))
                branchTable = ds.Tables[0];
            return branchTable;
        }
        public Branches AddBranch(Branches branch)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand("Branches_CRUD", connection);
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.Add("@Name", SqlDbType.NVarChar).Value = branch.Name;
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
            return branch;
        }
      
        public bool EditBranchName(Branches branch)
        {
            Int32 rowsAffected = 0;
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand("Branches_CRUD", connection);
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.Add("@ID", SqlDbType.Int).Value = branch.id;
                command.Parameters.Add("@Name", SqlDbType.NVarChar).Value = branch.Name;
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
        public DataTable RemoveBranch(Branches branch)
        {
            DataTable removeBranch = null;
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand("Branches_CRUD", connection);
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.Add("@Action", SqlDbType.NVarChar).Value = "DELETE";
                command.Parameters.Add("@ID", SqlDbType.Int).Value = branch.id;
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
            return removeBranch;
        }

    }
}