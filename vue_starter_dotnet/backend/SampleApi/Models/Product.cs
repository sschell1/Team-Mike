using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProductApproval.Models
{
    public class Product
    {
        public string ProductNumber { get; set; }
        public string ProductDescription { get; set; }
        public string DefaultUOM { get; set; }
        public int IsSellable { get; set; }
        public string CrossReference { get; set; }
        public int ItemType { get; set; }
        public int IsDrugControlled { get; set; }
        public string ManufacturerId { get; set; }
        public string InventoryStatus { get; set; }
        public string AlternativeProducts { get; set; }
        public int IsNonReturnable { get; set; }
        public int IsRefrigerated { get; set; }
        public int IsRegulated { get; set; }
    }
}
