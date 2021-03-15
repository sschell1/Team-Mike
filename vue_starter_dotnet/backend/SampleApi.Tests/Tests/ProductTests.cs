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
    public class ProductTests : ParentTest
    {
        private string addApprovedProductSql = "INSERT INTO ProductList (ProductNumber, ProductDescription, DefaultUOM, isSellable, CrossReference, ItemType, isDrugControlled, ManufacturerID, InventoryStatus, AlternativeProducts, isNonReturnable, isRefrigerated, isRegulated) VALUES ('123','generic product description','OH',1,'123',1,0,'INTERNATIONAL INC.','Buy','',0,0,0);";
        private string addUnapprovedProductSql = "INSERT INTO ProductList (ProductNumber, ProductDescription, DefaultUOM, isSellable, CrossReference, ItemType, isDrugControlled, ManufacturerID, InventoryStatus, AlternativeProducts, isNonReturnable, isRefrigerated, isRegulated) VALUES ('123','generic product description','OH',0,'123',1,0,'INTERNATIONAL INC.','Buy','',0,0,0);";

        [DataTestMethod]
        [DataRow("isSellable", 1)]
        public void GetAllApprovedProductsTest(string parameter, int value)
        {
            ProductSqlDAO dao = new ProductSqlDAO(connectionString);
            IList<Product> product = dao.GetAllApprovedProducts();
            int testResult = product.Count;
            int expectedResult = GetRowCountByBooleanParameter(parameter, value);
            Assert.AreEqual(expectedResult, testResult);
        }

        [DataTestMethod]
        [DataRow("isSellable", 0)]
        public void GetAllUnapprovedProductsTest(string parameter, int value)
        {
            ProductSqlDAO dao = new ProductSqlDAO(connectionString);
            IList<Product> product = dao.GetAllUnapprovedProducts();
            int testResult = product.Count;
            int expectedResult = GetRowCountByBooleanParameter(parameter, value);
            Assert.AreEqual(expectedResult, testResult);
        }

        [DataTestMethod]
        [DataRow("0221785", "0221785")]
        public void GetItemByProductNumberTest(string productNumber, string expectedValue)
        {
            ProductSqlDAO dao = new ProductSqlDAO(connectionString);
            Product item = dao.GetItemByProductNumber(productNumber);
            expectedValue = item.ProductNumber;
            Assert.AreEqual(productNumber, expectedValue);
        }

        [DataTestMethod]
        [DataRow("0221785", 0)]
        public void UpdateIsSellableTest(string productNumber, int expectedValue)
        {
            ProductSqlDAO dao = new ProductSqlDAO(connectionString);
            dao.UpdateIsSellable(productNumber, 0);
            Product testProduct = dao.GetItemByProductNumber(productNumber);            
            int result = testProduct.IsSellable;
            Assert.AreEqual(expectedValue, result);
        }
    }
}
