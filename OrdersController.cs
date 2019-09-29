using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using IdentitySample.Models;
using GateBoys.Models;
using Microsoft.AspNet.Identity;
//using GateBoys.Models.NewModels;

namespace GateBoys.Controllers
{

    public class OrdersController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Orders
        //[Authorize(Roles = "Admin")]
        public ActionResult Index()
        {
            return View(db.Orders.ToList());
        }
        public ActionResult userOrders()
        {
            return View(db.Orders.ToList());
        }

        public ActionResult getMonthSales(string yearMonth)
        {
            if (yearMonth == null)
            {
                return View(db.Orders.ToList());
            }
            int year = Convert.ToInt32(yearMonth.Substring(0, 4));
            int month = Convert.ToInt32(yearMonth.Substring(5, 2));
            var orders = db.Orders.Where(a => a.OrderDate.Year == year && a.OrderDate.Month == month).ToList();
            if (orders.Count < 1)
            {
                ViewBag.noOrders = "There are no orders for the selected year and month";
                orders = db.Orders.ToList();
            }
            return View(orders);
        }
        public ActionResult printOrders(string status)
        {
            ViewBag.ststus = null;
            if (status == "Paid")
            {
                ViewBag.ststus = "All orders paid for";
                var fileP = db.Orders.Where(a => a.Status == "Paid").ToList();
                return new Rotativa.ViewAsPdf(fileP) { FileName = $"gateboys_orders_paid.pdf" };
            }
            else
            {
                ViewBag.ststus = "All orders waiting for payment";
                var fileP = db.Orders.Where(a => a.Status != "Paid").ToList();
                return new Rotativa.ViewAsPdf(fileP) { FileName = $"gateboys_orders_notpaid.pdf" };
            }
        }
        public ActionResult printOrder(int id)
        {
            ViewBag.ststus = "Order";
            Order fileP = db.Orders.FirstOrDefault(a => a.OrderId == id);
            return new Rotativa.ViewAsPdf(fileP) { FileName = $"gateboys_order_{fileP.OrderNumber}.pdf" };
        }

        // GET: Orders/Details/5
        [Authorize(Roles = "Admin")]
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Order order = db.Orders.Find(id);
            if (order == null)
            {
                return HttpNotFound();
            }
            return View(order);
        }

        // GET: Orders/Create
        [Authorize(Roles = "Admin")]
        public ActionResult Create()
        {
            return View();
        }

        // POST: Orders/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "OrderId,OrderNumber,OrderProgress,OrderQuantity,orderedQty,orderedItems,TotalOrderCost,Status,OrderDate,DeliveryAddress,Id,username,Cell,AssignedDriver")] Order order)
        {
            if (ModelState.IsValid)
            {
                db.Orders.Add(order);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(order);
        }

        // GET: Orders/Edit/5
        [Authorize(Roles = "Admin")]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Order order = db.Orders.Find(id);
            if (order == null)
            {
                return HttpNotFound();
            }
            return View(order);
        }

        // POST: Orders/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "OrderId,OrderNumber,OrderProgress,OrderQuantity,orderedQty,orderedItems,TotalOrderCost,Status,OrderDate,DeliveryAddress,Id,username,Cell,AssignedDriver")] Order order)
        {
            if (ModelState.IsValid)
            {
                db.Entry(order).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(order);
        }

        // GET: Orders/Delete/5
        [Authorize(Roles = "Admin")]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Order order = db.Orders.Find(id);
            if (order == null)
            {
                return HttpNotFound();
            }
            return View(order);
        }

        // POST: Orders/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Order order = db.Orders.Find(id);
            db.Orders.Remove(order);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        public ActionResult Inspect(int OrderId)
        {
            return View(db.OrderItems.Where(x => x.OrderId == OrderId).ToList());

        }

        public ActionResult GetDetails(int OrderId)
        {
            return View(db.OrderItems.Where(x => x.OrderId == OrderId).ToList());
        }


        [HttpGet]
        public ActionResult MyOrders()
        {
            List<OrdersVM> vmList = new List<OrdersVM>();

            var orderList = (from x in db.OrderItems
                             join y in db.Orders on x.OrderId equals y.OrderId

                             select new { x.productId, y.OrderId, y.username, x.Product.productName, x.Product.Image, x.Product.unitPrice, y.TotalOrderCost, y.OrderDate, y.DeliveryAddress, y.orderedQty, y.orderedItems }).ToList();

            foreach (var item in orderList)
            {
                OrdersVM ov = new OrdersVM()
                {
                    orderId = item.OrderId,
                    productId = item.productId,
                    user = item.username,
                    productName = item.productName,
                    Image = item.Image,
                    Price = item.unitPrice,
                    TotalOrderCost = item.TotalOrderCost,
                    orderDate = item.OrderDate.ToString(),
                    DeliveryAddress = item.DeliveryAddress,
                    Quantity = item.orderedQty
                };

                vmList.Add(ov);
                db.OrdersVMs.Add(ov);
                db.SaveChanges();

            }
            return View(vmList);
        }
    }
}
