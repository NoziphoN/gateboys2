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
//using GateBoys.Models.NewModels;

namespace GateBoys.Controllers
{
    public class OrderTracksController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        public ActionResult Track(string searchString, string currentFilter, int? page)
        {

            if (searchString != null)
            {
                page = 1;
            }
            else
            {
                searchString = currentFilter;
            }

            ViewBag.CurrentFilter = searchString;

            List<OrderTrack> orders = db.OrderTracks.Where(x => x.OrderNumber.OrderNumber == searchString).OrderByDescending(x => x.status).ToList();

            OrderIndexViewModel oivn1 = new OrderIndexViewModel()
            {
                orders = orders
            };

            if (!String.IsNullOrEmpty(searchString))
            {
                orders = orders.Where(o => o.OrderNumber.Equals(searchString)).ToList();

            }

            return View(oivn1);
        }
        // GET: OrderTracks
        public ActionResult Index()
        {
            List<OrderTrack> Pending = db.OrderTracks.Where(x => x.status == OrderStat.pending).OrderByDescending(x => x.status).ToList();
            List<OrderTrack> OnOurWarehouse = db.OrderTracks.Where(x => x.status == OrderStat.OnOurWarehouse).OrderByDescending(x => x.status).ToList();
            List<OrderTrack> OnItsWay = db.OrderTracks.Where(x => x.status == OrderStat.OnItsWay).OrderByDescending(x => x.status).ToList();
            List<OrderTrack> Delivered = db.OrderTracks.Where(x => x.status == OrderStat.Delivered).OrderByDescending(x => x.status).ToList();
            List<OrderTrack> cancelledOrders = db.OrderTracks.Where(x => x.status == OrderStat.Cancelled).OrderByDescending(x => x.status).ToList();
            OrderIndexViewModel oivn = new OrderIndexViewModel()
            {
                Pending = Pending,
                OnOurWarehouse = OnOurWarehouse,
                OnItsWay = OnItsWay,
                Delivered = Delivered,
                cancelledOrders = cancelledOrders
            };
            return View(oivn);
        }

        // GET: OrderTracks/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            OrderTrack orderTrack = db.OrderTracks.Find(id);
            if (orderTrack == null)
            {
                return HttpNotFound();
            }
            return View(orderTrack);
        }

        // GET: OrderTracks/Create
        public ActionResult Create()
        {
            ViewBag.OrderId = new SelectList(db.Orders, "OrderId", "OrderNumber");
            return View();
        }

        // POST: OrderTracks/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "TID,OrderId,captured,status")] OrderTrack orderTrack)
        {
            if (ModelState.IsValid)
            {
                db.OrderTracks.Add(orderTrack);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.OrderId = new SelectList(db.Orders, "OrderId", "OrderNumber", orderTrack.OrderId);
            return View(orderTrack);
        }

        // GET: OrderTracks/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            OrderTrack orderTrack = db.OrderTracks.Find(id);
            if (orderTrack == null)
            {
                return HttpNotFound();
            }
            ViewBag.OrderId = new SelectList(db.Orders, "OrderId", "OrderNumber", orderTrack.OrderId);
            return View(orderTrack);
        }

        // POST: OrderTracks/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "TID,OrderId,captured,status")] OrderTrack orderTrack)
        {
            if (ModelState.IsValid)
            {
                db.Entry(orderTrack).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.OrderId = new SelectList(db.Orders, "OrderId", "OrderNumber", orderTrack.OrderId);
            return View(orderTrack);
        }

        // GET: OrderTracks/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            OrderTrack orderTrack = db.OrderTracks.Find(id);
            if (orderTrack == null)
            {
                return HttpNotFound();
            }
            return View(orderTrack);
        }

        // POST: OrderTracks/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            OrderTrack orderTrack = db.OrderTracks.Find(id);
            db.OrderTracks.Remove(orderTrack);
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
    }
}
