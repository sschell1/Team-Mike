using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ProductApproval.DAL;
using ProductApproval.Models;
using ProductApproval.Password_and_Authentication_Helpers;
using static ProductApproval.Password_and_Authentication_Helpers.HashProvider;
using Microsoft.Extensions.Configuration;

namespace ProductApproval.Controllers
{
    [Route("api/products")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private IProductDAO dao;

        public ProductsController(IProductDAO dataAccessLayer)
        {
            dao = dataAccessLayer;
        }

        [HttpGet("{isSellable}", Name = "GetProducts")]
        public IList<Product> GetProducts(int isSellable)
        {
            if (isSellable == 1)
            {
                return dao.GetAllApprovedProducts();
            }
            else
            {
                return dao.GetAllUnapprovedProducts();
            }
        }
    }

    [Route("api/item")]
    public class ItemController : ControllerBase
    {
        private IProductDAO dao;

        public ItemController(IProductDAO dataAccessLayer)
        {
            dao = dataAccessLayer;
        }

        [HttpGet("{ProductNumber}", Name = "GetProductNumber")]
        public Product GetProductNumber(string productNumber)
        {

            if (productNumber != null)
            {
                return dao.GetItemByProductNumber(productNumber);
            }
            else
            {
                return new Product();
            }
        }

        [HttpPut("{productNumber}/Sellable")]
        public ActionResult SwitchingIsSellable(string productNumber)
        {
            Product prodNum = dao.GetItemByProductNumber(productNumber);

            if (prodNum != null)
            {
                if (prodNum.IsSellable == 0)
                {
                    prodNum.IsSellable = 1;
                }
                else
                {
                    prodNum.IsSellable = 0;
                }
            }
            dao.UpdateIsSellable(productNumber, prodNum.IsSellable);

            return NoContent();
        }

        [HttpPut("{productNumber}")]
        public ActionResult EditProduct(string productNumber, [FromBody]Product product)
        {
            if (product.ProductNumber != null)
            {
                dao.EditProduct(product);
            }
            return Ok();
        }

        [HttpPost("password/{password}")]
        public ActionResult Password(string password)
        {
            HashProvider hashProvider = new HashProvider();
            HashedPassword hashedPassword = hashProvider.HashPassword(password);
            return Ok(hashedPassword);
        }
    }
}
