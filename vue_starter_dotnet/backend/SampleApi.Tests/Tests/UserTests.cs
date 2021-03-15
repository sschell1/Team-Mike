using Microsoft.VisualStudio.TestTools.UnitTesting;
using ProductApproval.DAL;
using ProductApproval.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Text;

namespace ProductApprovalTests.Tests
{
    [TestClass]
    public class UserTests : ParentTest
    {
        private string getUserSql = "Select * FROM users WHERE userName = @userName";

        [TestMethod]
        public void GetAllUsersTest()
        {
            UsersSqlDAO dao = new UsersSqlDAO(connectionString);
            IList<User> allUsers = dao.GetAllUsers();
            int testResult = allUsers.Count;
            int expectedResult = GetRowCount("users");
            Assert.AreEqual(expectedResult, testResult);
        }

        [TestMethod]
        public void AddUserTest()
        {
            int originalRowCount = GetRowCount("users");

            UsersSqlDAO dao = new UsersSqlDAO(connectionString);

            User testUser = new User();
            testUser.FirstName = "John";
            testUser.LastName = "Smith";
            testUser.Username = "johnsmith";
            testUser.Role = "User";
            testUser.Password = "password";
            testUser.Salt = "salt";

            dao.AddUser(testUser);

            int expectedRowCount = GetRowCount("users");

            Assert.AreEqual(originalRowCount + 1, expectedRowCount);
        }

        [TestMethod]
        public void UpdateUserTest()
        {
            UsersSqlDAO dao = new UsersSqlDAO(connectionString);
            string updatedFirstName = "Jane";
            string updatedLastName = "Doe";
            string updatedRole = "Admin";

            User testUser = new User();
            testUser.FirstName = "John";
            testUser.LastName = "Smith";
            testUser.Username = "johnsmith";
            testUser.Role = "User";
            testUser.Password = "password";
            testUser.Salt = "salt";

            dao.AddUser(testUser);

            User updatedTestUser = new User();
            updatedTestUser.FirstName = "Jane";
            updatedTestUser.LastName = "Doe";
            updatedTestUser.Username = "johnsmith";
            updatedTestUser.Role = "Admin";
            updatedTestUser.Password = "password";
            updatedTestUser.Salt = "salt";

            dao.UpdateUser(updatedTestUser);

            User finalUser = new User();
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand(getUserSql, conn);
                cmd.Parameters.AddWithValue("@userName", testUser.Username);

                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    User tempUser = new User();
                    tempUser.Role = Convert.ToString(reader["role"]);
                    tempUser.FirstName = Convert.ToString(reader["firstname"]);
                    tempUser.LastName = Convert.ToString(reader["lastname"]);

                    finalUser = tempUser;
                }
            }

            Assert.AreEqual(updatedRole, finalUser.Role);
            Assert.AreEqual(updatedFirstName, finalUser.FirstName);
            Assert.AreEqual(updatedLastName, finalUser.LastName);
        }

        [TestMethod]
        public void DeleteUserTest()
        {
             UsersSqlDAO dao = new UsersSqlDAO(connectionString);

            User testUser = new User();
            testUser.FirstName = "John";
            testUser.LastName = "Smith";
            testUser.Username = "johnsmith";
            testUser.Role = "User";
            testUser.Password = "password";
            testUser.Salt = "salt";

            dao.AddUser(testUser);

            int originalRowCount = GetRowCount("users");

            dao.DeleteUser(testUser);

            int expectedRowCount = GetRowCount("users");

            Assert.AreEqual(originalRowCount - 1, expectedRowCount);
        }

    }
}
