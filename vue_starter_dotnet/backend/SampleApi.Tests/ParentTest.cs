using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Text;
using System.Transactions;

namespace ProductApprovalTests
{
    [TestClass]
    public class ParentTest
    {
        //Change this to correct DB
        protected string connectionString = "Data Source=.\\sqlexpress;Initial Catalog=ProductApproval;Integrated Security=True";

        public TransactionScope trans;

        [TestInitialize]
        public void Setup()
        {
            trans = new TransactionScope();
        }

        [TestCleanup]
        public void Reset()
        {
            trans.Dispose();
        }

        public int GetRowCount(string table)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand($"SELECT COUNT(*) FROM {table}", conn);
                int count = Convert.ToInt32(cmd.ExecuteScalar());
                return count;
            }
        }

        protected int GetRowCountByBooleanParameter(string parameter, int value)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand($"SELECT COUNT(*) FROM ProductList WHERE {parameter} = {value}", conn);
                int count = Convert.ToInt32(cmd.ExecuteScalar());
                return count;
            }
        }

    }
}
