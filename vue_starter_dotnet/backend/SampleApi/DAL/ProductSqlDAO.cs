using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using ProductApproval.DAL;
using ProductApproval.Models;
using Microsoft.Extensions.Configuration;

namespace ProductApproval.DAL
{
    public class ProductSqlDAO : IProductDAO
    {
        private readonly string connectionString;
        public ProductSqlDAO(string connectionString)
        {
            this.connectionString = connectionString;
        }

        private string getApprovedProductsSql = "SELECT * FROM ProductList WHERE isSellable = 1";
        private string getUnapprovedProductsSql = "SELECT * FROM ProductList WHERE isSellable = 0";
        private string getProductNumberSql = "SELECT * FROM ProductList WHERE ProductNumber = @ProductNumber";
        private string updateIsSellableSql = "UPDATE ProductList SET isSellable = @isSellable WHERE ProductNumber = @ProductNumber";
        private string deleteItem = "DELETE FROM ProductList WHERE ProductNumber = @ProductNumber";
        private string insertItemEdited = "INSERT INTO ProductList(ProductNumber, ProductDescription, DefaultUOM, isSellable, CrossReference, " + 
        "ItemType, isDrugControlled, ManufacturerID, InventoryStatus, AlternativeProducts, isNonReturnable, isRefrigerated, isRegulated) VALUES " +
        "(@ProductNumber, @ProductDescription, @DefaultUOM, @isSellable, @CrossReference, @ItemType, @isDrugControlled, @ManufacturerID, " +
        "@InventoryStatus, @AlternativeProducts, @isNonReturnable, @isRefrigerated, @isRegulated);";

        List<Product> ProductList { get; set; }

        public IList<Product> GetAllApprovedProducts()
        {
            List<Product> productList = new List<Product>();
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();

                SqlCommand cmd = new SqlCommand(getApprovedProductsSql, conn);
                SqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    productList.Add(MapReadToProduct(reader));
                }
            }

            return productList;
        }

        public IList<Product> GetAllUnapprovedProducts()
        {
            List<Product> productList = new List<Product>();
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();

                SqlCommand cmd = new SqlCommand(getUnapprovedProductsSql, conn);
                SqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    productList.Add(MapReadToProduct(reader));
                }
            }

            return productList;
        }

        public Product GetItemByProductNumber(string productNumber)
        {
            Product item = new Product();
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();

                SqlCommand cmd = new SqlCommand(getProductNumberSql, conn);
                cmd.Parameters.AddWithValue("@ProductNumber", productNumber);
                SqlDataReader reader = cmd.ExecuteReader();

                while(reader.Read())
                {
                    item = (MapReadToProduct(reader));
                }
                return item;
            }
        }

        public int UpdateIsSellable (string productNumber, int isSellable)
        {
            Product product = new Product();
            
            int result = 0;
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();

                    SqlCommand cmd = new SqlCommand(updateIsSellableSql, conn);
                    cmd.Parameters.AddWithValue("@ProductNumber", productNumber);
                    cmd.Parameters.AddWithValue("@isSellable", isSellable);
                    result = cmd.ExecuteNonQuery();
                }                
            }
            catch (SqlException ex)
            {
                Console.WriteLine(ex);
            }

            return result;
        }
        
        public Product EditProduct(Product product)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();

                SqlCommand cmd = new SqlCommand(deleteItem, conn);
                cmd.Parameters.AddWithValue("@ProductNumber", product.ProductNumber);

                cmd.ExecuteNonQuery();

                cmd = new SqlCommand(insertItemEdited, conn);
                cmd.Parameters.AddWithValue("@ProductNumber", product.ProductNumber);
                cmd.Parameters.AddWithValue("@ProductDescription", product.ProductDescription);
                cmd.Parameters.AddWithValue("@DefaultUOM", product.DefaultUOM);
                cmd.Parameters.AddWithValue("@isSellable", product.IsSellable);
                cmd.Parameters.AddWithValue("@CrossReference", product.CrossReference);
                cmd.Parameters.AddWithValue("@ItemType", product.ItemType);
                cmd.Parameters.AddWithValue("@isDrugControlled", product.IsDrugControlled);
                cmd.Parameters.AddWithValue("@ManufacturerId", product.ManufacturerId);
                cmd.Parameters.AddWithValue("@InventoryStatus", product.InventoryStatus);
                cmd.Parameters.AddWithValue("@AlternativeProducts", product.AlternativeProducts);
                cmd.Parameters.AddWithValue("@isNonReturnable", product.IsNonReturnable);
                cmd.Parameters.AddWithValue("@isRefrigerated", product.IsRefrigerated);
                cmd.Parameters.AddWithValue("@isRegulated", product.IsRegulated);

                SqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    product = (MapReadToProduct(reader));
                }
                return product;
            }
        }

        private Product MapReadToProduct(SqlDataReader reader)
        {
            Product product = new Product();
            product.ProductNumber = Convert.ToString(reader["ProductNumber"]);
            product.ProductDescription = Convert.ToString(reader["ProductDescription"]);
            product.DefaultUOM = Convert.ToString(reader["DefaultUOM"]);
            product.IsSellable = Convert.ToInt32(reader["isSellable"]);
            product.CrossReference = Convert.ToString(reader["CrossReference"]);
            product.ItemType = Convert.ToInt32(reader["ItemType"]);
            product.IsDrugControlled = Convert.ToInt32(reader["isDrugControlled"]);
            product.ManufacturerId = Convert.ToString(reader["ManufacturerId"]);
            product.InventoryStatus = Convert.ToString(reader["InventoryStatus"]);
            product.AlternativeProducts = Convert.ToString(reader["AlternativeProducts"]);
            product.IsNonReturnable = Convert.ToInt32(reader["isNonReturnable"]);
            product.IsRefrigerated = Convert.ToInt32(reader["isRefrigerated"]);
            product.IsRegulated = Convert.ToInt32(reader["isRegulated"]);

            return product;
        }
    }
}

