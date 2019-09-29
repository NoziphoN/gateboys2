using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace GateBoys.Models
{
    public class TrackOrder
    {
        [Key]
        public int TrackId { get; set; }

        [Display(Name = "Order Number")]
        public string OrderNumber { get; set; }

        [Display(Name = "Order Placed")]
        public bool OrderPlaced { get; set; }

        [Display(Name = "Order Placed Date")]
        public string OrderPlacedDate { get; set; }

        [Display(Name = "Preparing Parcel")]
        public bool PreparingParcel { get; set; }

        [Display(Name = "Preparing Date")]
        public string PrepParcelDate { get; set; }

        [Display(Name = "Order In-Warehouse")]
        public bool InWarehouse { get; set; }

        [Display(Name = "Order inWarehouse Date")]
        public string InWarehsDate { get; set; }

        [Display(Name = "Order is Ready")]
        public bool IsReady { get; set; }

        [Display(Name = "Order is Ready Date")]
        public string IsReadyDate { get; set; }

        [Display(Name = "Order In-Transit")]
        public bool InTransit { get; set; }

        [Display(Name = "Order inTransit Date")]
        public string InTransitDate { get; set; }

        [Display(Name = "Order Is Delivered")]
        public bool IsDeliver { get; set; }

        [Display(Name = "Order Is Deliver Date")]
        public string IsDeliverDate { get; set; }

        [Display(Name = "Order Is Delivered")]
        public int driverId { get; set; }
        [Display(Name = "Order Is Deliver Date")]
        public string driverNames { get; set; }

        [Display(Name = "ID Number")]
        public string colIdNum { get; set; }

        [MaxLength(13)]
        [MinLength(13)]
        [Display(Name = "ID Compare")]
        [Range(13, Int64.MaxValue, ErrorMessage = "ID Number should not contain charecters and must be 13 digits")]
        public string idCompareNum { get; set; }

        [Display(Name = "User Email")]
        public string UserMail { get; set; }

        [Display(Name = "Deliver/collect")]
        public string delColect { get; set; }

        [Display(Name = "Pin")]
        public int pin { get; set; }

        [Display(Name = "Pin Compare")]
        public int comparePin { get; set; }
    }
}