using GateBoys.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace GateBoys.Models
{
    public class OrderItem
    {
        //private QDb db = new QDb();

        [Key]

        public int OrderItemId { get; set; }



        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }
        public int productId { get; set; }

        public virtual InventoryProduct Product { get; set; }
        public int OrderId { get; set; }
        public Order Order { get; set; }
        public int OrderProg { get; set; }
        public OrderItem()
        {

        }
    }
}