using ProductApproval.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProductApproval.DAL
{
    public interface IProductDAO
    {
        IList<Product> GetAllApprovedProducts();

        IList<Product> GetAllUnapprovedProducts();

        Product GetItemByProductNumber(string var);

        int UpdateIsSellable(string productNumber, int isSellable);

        Product EditProduct(Product product);
    }
}
