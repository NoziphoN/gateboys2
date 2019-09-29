using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
//using GateBoys.Models.NewModels.InventoryProducts;
using GateBoys.Models;

namespace GateBoys.Models
{
    public class AdminOrder
    {
        [Key]
        public int id { get; set; }

        [ Display(Name = "Supplier")]
        public string ToSupplier { get; set; }
        [Display(Name = "Admin email"), EmailAddress]
        public string FromAdmin { get; set; }
        [Required]
        [DataType(DataType.MultilineText)]
        public string Message { get; set; }
        public string dateOrdered { get { return DateTime.Now.ToString(); } }
        

        //Foreign Key Category
        public int SupplierId { get; set; }
        public virtual Supplier Suppliers { get; set; }
    }
}