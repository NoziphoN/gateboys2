using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace GateBoys.Models
{
    public class OrderTrack
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int TID { get; set; }


        public int OrderId { get; set; }
        public virtual Order OrderNumber { get; set; }

        [Required(ErrorMessage = "Provide Delivery Date!")]
        [Display(Name = "Ordered Date")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]

        public DateTime captured { get; set; }
        [Required(ErrorMessage = "Provide Status!")]
        [Display(Name = "Status")]
        public OrderStat status { get; set; }
        public OrderTrack()
        {
            captured = DateTime.Now;
            status = OrderStat.OnOurWarehouse;
        }
    }

    public enum OrderStat
    {
        pending,
        OnOurWarehouse,
        OnItsWay,
        Delivered,
        Cancelled
    }

}