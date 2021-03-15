using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using ProductApproval.Models;
using ProductApproval.Password_and_Authentication_Helpers;
using static ProductApproval.Password_and_Authentication_Helpers.HashProvider;

namespace ProductApproval.DAL
{
    public class UsersSqlDAO : IUsersDAO
    {
        private readonly string connectionString;
        public UsersSqlDAO(string connectionString)
        {
            this.connectionString = connectionString;
        }

        private string GetAllUsersSql = "SELECT * FROM users";
        private string getUserSql = "Select * FROM users WHERE userName = @userName";
        private string AddUserSql = "INSERT INTO users (role, userName, password, salt, lastname, firstname) " +
            "VALUES(@role, @userName, @password, @salt, @lastname, @firstname);";
        private string UpdateUserSql = "UPDATE users SET role = @role, lastname = @lastname, firstname = @firstname " +
            "WHERE userName = @username;";
        private string DeleteUserSql = "DELETE FROM users WHERE userName = @userName";

        public IList<User> GetAllUsers()
        {
            List<User> allUsers = new List<User>();

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand(GetAllUsersSql, conn);
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    allUsers.Add(MapReadToUser(reader));
                }
            }
            return allUsers;
        }

        public User GetUser(string username)
        {
            User user = new User();
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand(getUserSql, conn);
                cmd.Parameters.AddWithValue("@username", username);
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    user = MapReadToUser(reader);
                }
            }
            return user;
        }

        public int AddUser(User user)
        {
            int result = 0;

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand(AddUserSql, conn);
                cmd.Parameters.AddWithValue("@role", user.Role);
                cmd.Parameters.AddWithValue("@userName", user.Username);
                cmd.Parameters.AddWithValue("@password", user.Password);
                cmd.Parameters.AddWithValue("@salt", user.Salt);
                cmd.Parameters.AddWithValue("@lastname", user.LastName);
                cmd.Parameters.AddWithValue("@firstname", user.FirstName);
                result = cmd.ExecuteNonQuery();
            }

            return result;
        }

        public User UpdateUser(User user)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand(UpdateUserSql, conn);
                cmd.Parameters.AddWithValue("@username", user.Username);
                cmd.Parameters.AddWithValue("@role", user.Role);
                cmd.Parameters.AddWithValue("@firstName", user.FirstName);
                cmd.Parameters.AddWithValue("@lastName", user.LastName);

                cmd.ExecuteNonQuery();
            }

            return user;
        }

        public int DeleteUser(string userName)
        {
            int result = 0;
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand(DeleteUserSql, conn);
                cmd.Parameters.AddWithValue("@userName", userName);

                result = cmd.ExecuteNonQuery();

            }
            return result;
        }

        private User MapReadToUser(SqlDataReader reader)
        {
            User user = new User();

            user.UserId = Convert.ToInt32(reader["userID"]);
            user.Username = Convert.ToString(reader["userName"]);
            user.Role = Convert.ToString(reader["role"]);
            user.LastName = Convert.ToString(reader["lastname"]);
            user.FirstName = Convert.ToString(reader["firstname"]);
            user.Password = Convert.ToString(reader["password"]);
            user.Salt = Convert.ToString(reader["salt"]);
            return user;
        }
    }
}
