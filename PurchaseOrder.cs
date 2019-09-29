using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace GateBoys.Models
{
    public class PurchaseOrder
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int OrderId { get; set; }

        public string PurchaseOrderNumber { get; set; }
        public DateTime DateRecieved { get; set; }

        public int SupplierId { get; set; }
        //public virtual Supplier Suppliers { get; set; }

        public int TotalQuantity { get; set; }
        public decimal TotalCost { get; set; }

        public virtual ICollection<OrderRowDetails> OrderRowDetails { get; set; }
    }
}