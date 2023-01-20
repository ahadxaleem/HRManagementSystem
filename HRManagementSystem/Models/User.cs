using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace HRManagementSystem.Models
{
    
    public class User
    {

        public string UserID { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string ConfirmPassword { get; set; }

        
    }

    public class UserBusinessService
    {
        //public string connectionString { get; set; }

        public string connectionString
        {
            get
            {
                return ConfigurationManager.ConnectionStrings["HRManagement_ConnectionString"].ConnectionString;
            }
        }
        public User User_CRUD(User user)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand("Users_CRUD", connection);
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.Add("@FirstName", SqlDbType.NVarChar).Value = user.FirstName;
                command.Parameters.Add("@LastName", SqlDbType.NVarChar).Value= user.LastName;
                command.Parameters.Add("@Email", SqlDbType.NVarChar).Value = user.Email;
                command.Parameters.Add("@Password", SqlDbType.NVarChar).Value = user.Password;

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
                       
            return user;
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

        public User User_Select(string Email, string Password)
        {
            DataSet ds = User_SelectFromDB(Email, Password);

            if ((ds != null) && (ds.Tables.Count > 0) && (ds.Tables[0].Rows.Count > 0))
                return CreateUserFromDataRow(ds.Tables[0].Rows[0]);

            return null;
        }

        public User CreateUserFromDataRow(DataRow dr)
        {
            if (dr != null)
            {
                User user = new User();

                user.UserID = GetString(dr, "UserID");
                user.FirstName = GetString(dr, "FirstName");
                user.LastName = GetString(dr, "LastName");
                user.Email = GetString(dr, "Email");
                user.Password = GetString(dr, "Password");
                
                return user;
            }
            return null;
        }
        protected static string GetString(DataRow row, string columnName)
        {
            return (row[columnName] != DBNull.Value) ? Convert.ToString(row[columnName]) : string.Empty;
        }

    }

}