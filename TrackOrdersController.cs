using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Net.Mime;
using System.Web;
using System.Web.Mvc;
using IdentitySample.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using GateBoys.Models;
using GateBoys.Helpers;

namespace GateBoys.Controllers
{
    public class TrackOrdersController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: TrackOrders
        public ActionResult Index()
        {
            return View(db.TrackOrders.ToList());
        }

        public ActionResult assignedToMe()
        {
            string email = User.Identity.GetUserName();
            var driver = db.drivers.FirstOrDefault(a => a.employeeEmail == email);
            return View(db.TrackOrders.Where(a => a.delColect == "Deliver" && a.InWarehouse == true && a.driverId == driver.id).ToList());
        }

        // GET: TrackOrders/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                //return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                return RedirectToAction("Error", "Home");
            }
            TrackOrder trackOrder = db.TrackOrders.Find(id);
            if (trackOrder == null)
            {
                //return HttpNotFound();
                return RedirectToAction("Error", "Home");

            }
            return View(trackOrder);
        }

        // GET: TrackOrders/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: TrackOrders/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "TrackId,OrderNumber,OrderPlaced,OrderPlacedDate,PreparingParcel," +
            "PrepParcelDate,InWarehouse,InWarehsDate," +
            "IsReady,IsReadyDate,InTransit,InTransitDate,IsDeliver,IsDeliverDate," +
            "colIdNum,idCompareNum,pin,comparePin,UserMail,delColect,driverId,driverNames")] TrackOrder trackOrder)
        {

            if (ModelState.IsValid)
            {
                db.TrackOrders.Add(trackOrder);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(trackOrder);
        }

        // GET: TrackOrders/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return RedirectToAction("Error", "Home");
            }
            TrackOrder trackOrder = db.TrackOrders.Find(id);
            if (trackOrder == null)
            {
                return RedirectToAction("Error", "Home");
            }
            return View(trackOrder);
        }

        // POST: TrackOrders/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "TrackId,OrderNumber,OrderPlaced,OrderPlacedDate,PreparingParcel," +
            "PrepParcelDate,InWarehouse,InWarehsDate," +
            "IsReady,IsReadyDate,InTransit,InTransitDate,IsDeliver,IsDeliverDate," +
            "colIdNum,idCompareNum,pin,comparePin,UserMail,delColect,driverId,driverNames")] TrackOrder trackOrder)
        {
            if (ModelState.IsValid)
            {
                db.Entry(trackOrder).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(trackOrder);
        }

        // GET: TrackOrders/Edit/5
        public ActionResult assign(int? id)
        {
            if (id == null)
            {
                return RedirectToAction("Error", "Home");
            }
            TrackOrder trackOrder = db.TrackOrders.Find(id);
            if (trackOrder == null)
            {
                return RedirectToAction("Error", "Home");
            }
            return View(trackOrder);
        }
        // POST: TrackOrders/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult assign([Bind(Include = "TrackId,OrderNumber,OrderPlaced,OrderPlacedDate,PreparingParcel," +
            "PrepParcelDate,InWarehouse,InWarehsDate," +
            "IsReady,IsReadyDate,InTransit,InTransitDate,IsDeliver,IsDeliverDate," +
            "colIdNum,idCompareNum,pin,comparePin,UserMail,delColect,driverId,driverNames")] TrackOrder trackOrder)
        {
            var driver = db.drivers.Where(a => a.id == trackOrder.driverId).FirstOrDefault();
            trackOrder.driverNames = (driver.employeeMidName == null) ? $"{driver.employeeName} {driver.employeeSurname}" : $"{driver.employeeName} {driver.employeeMidName} {driver.employeeSurname}";
            string message = $"Hi {trackOrder.driverNames}, You have been assigned new order ({trackOrder.OrderNumber}) ready for you to deliver" +
                $" please check all orders assigned to you and find the corresponsing details. Please ensure the user insert the correct ID Number" +
                $" and Pin when collecting the parcel as you will be liable if the parcel does not receive the parcel." +
                $" For more info contact us at parcelenquiry@gateboys.com.";

            emailhelper.sendMail(driver.employeeEmail, $"New order to deliver", message);
            var orderToUpdate = db.Orders.FirstOrDefault(a => a.OrderNumber == trackOrder.OrderNumber);

            if (ModelState.IsValid)
            {
                orderToUpdate.AssignedDriver = trackOrder.driverNames;
                db.Entry(trackOrder).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(trackOrder);
        }
        // GET: TrackOrders/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return RedirectToAction("Error", "Home");
            }
            TrackOrder trackOrder = db.TrackOrders.Find(id);
            if (trackOrder == null)
            {
                return RedirectToAction("Error", "Home");
            }
            return View(trackOrder);
        }

        // POST: TrackOrders/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            TrackOrder trackOrder = db.TrackOrders.Find(id);
            db.TrackOrders.Remove(trackOrder);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        // GET: TrackOrders/Edit/5
        public ActionResult prepDone(int? id)
        {
            if (id == null)
            {
                return RedirectToAction("Error", "Home");
            }
            TrackOrder trackOrder = db.TrackOrders.Find(id);
            if (trackOrder == null)
            {
                return RedirectToAction("Error", "Home");
            }

            Random otp = new Random();
            int pin = otp.Next(10000, 99999);

            trackOrder.PrepParcelDate = DateTime.Now.ToString();
            trackOrder.PreparingParcel = true;
            trackOrder.pin = pin;
            var user = db.addInfoes.FirstOrDefault(a => a.addInfoOf == trackOrder.UserMail);
            string names = $"{user.name} {user.surname}";
            string emailSubject = $"Order {trackOrder.OrderNumber} update";
            string message = $"Hi { names}. There has been progress on order number { trackOrder.OrderNumber},When receiving or collecting the parcel please provide the pin - {pin} " +
                   $",Please note that when collecting you will be required to put your ID number and pin({pin}) if you cannot provide the pin you will have to reset the pin on the website." +
                   $"Track your order using order number for more information Contact us on gateboys@mail.com for enquiries";
            emailhelper.sendMail(trackOrder.UserMail, emailSubject, message);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        // parcel in warehouse to be separated (collection/delivery)
        public ActionResult inWare(int? id)
        {
            if (id == null)
            {
                return RedirectToAction("Error", "Home");
            }
            TrackOrder trackOrder = db.TrackOrders.Find(id);
            if (trackOrder == null)
            {
                return RedirectToAction("Error", "Home");
            }
            trackOrder.InWarehouse = true;
            trackOrder.InWarehsDate = DateTime.Now.ToString();
            db.SaveChanges();
            var user = db.addInfoes.FirstOrDefault(a => a.addInfoOf == trackOrder.UserMail);
            string names = $"{user.name} {user.surname}";
            string emailSubject = $"Order {trackOrder.OrderNumber} update";
            string message = $"Hi { names}. There has been progress on order number { trackOrder.OrderNumber},When receiving or collecting the parcel please provide the pin - {trackOrder.pin} " +
                 $",Please note that when collecting you will be required to put your ID number and pin({trackOrder.pin}) if you cannot provide the pin you will have to reset the pin on the website." +
                 $"Track your order using order number for more information Contact us on gateboys@mail.com for enquiries";
            emailhelper.sendMail(trackOrder.UserMail, emailSubject, message);
            return RedirectToAction("Index");
        }

        // ready for collection
        public ActionResult ready(int? id)
        {
            if (id == null)
            {
                //return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                return RedirectToAction("Error", "Home");
            }
            TrackOrder trackOrder = db.TrackOrders.Find(id);

            if (trackOrder == null)
            {
                return RedirectToAction("Error", "Home");
            }

            if (trackOrder.delColect == "Deliver")
            {
                return RedirectToAction("Error", "Home");
            }

            trackOrder.IsReadyDate = DateTime.Now.ToString();
            var user = db.addInfoes.FirstOrDefault(a => a.addInfoOf == trackOrder.UserMail);
            string names = $"{user.name} {user.surname}";
            string emailSubject = $"Order {trackOrder.OrderNumber} update";
            string message = $"Hi { names}. We have some Good :) news for order number { trackOrder.OrderNumber}," +
                $"Your order is ready for collection please make sure to bring your ID and don't forget " +
                $"your pin as it is required for you to get the parcel." +
                $"Contact us on gateboys@mail.com for enquiries";
            string sent = emailhelper.sendMail(trackOrder.UserMail, emailSubject, message);
            if (sent == "sent")
            {
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            else
            {
                return RedirectToAction("Error", "Home");
            }
        }

        // GET: TrackOrders/Edit/5
        public ActionResult transit(int? id)
        {
            if (id == null)
            {
                //return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                return RedirectToAction("Error", "Home");
            }
            TrackOrder trackOrder = db.TrackOrders.Find(id);
            if (trackOrder == null)
            {
                //return HttpNotFound();
                return RedirectToAction("Error", "Home");
            }
            if (trackOrder.delColect == "Collect")
            {
                //RedirectToAction("Error");
                return RedirectToAction("Error", "Home");
            }

            trackOrder.InTransit = true;
            trackOrder.InTransitDate = DateTime.Now.ToString();
            var user = db.addInfoes.FirstOrDefault(a => a.addInfoOf == trackOrder.UserMail);
            var driver = db.drivers.FirstOrDefault(d => d.id == trackOrder.driverId);
            string names = $"{user.name} {user.surname}";
            string emailSubject = $"Order {trackOrder.OrderNumber} update";
            string message = $"Hi { names}. We have some Good :) news for order number { trackOrder.OrderNumber}," +
                 $"Your order is on it's way for delivery please make sure to bring your ID when coming to the driver and don't forget " +
                 $"your pin as it is required for you to get the parcel." +
                 $"Contact us on gateboys@mail.com for enquiries";
            string messageTodriver = $"Hi { trackOrder.driverNames}.You just updated the status for order number { trackOrder.OrderNumber}," +
                             $" We understand the order is in on it way to the customer ensure the customer provide the ID number to the app  and correct pin." +
                             $" Contact us on deliverparcels@gateboys.com for enquiries";
            string sent = emailhelper.sendMail(trackOrder.UserMail, emailSubject, message);
            string sentToDriver = emailhelper.sendMail(driver.employeeEmail, emailSubject, messageTodriver);
            if (sent == "sent" && sentToDriver == "sent")
            {
                db.SaveChanges();
                return RedirectToAction("assignedToMe");
            }
            else
            {
                return RedirectToAction("Error", "Home");
            }
        }
        // GET: TrackOrders/MarkComplete/5
        public ActionResult MarkComplete(int? id)
        {
            if (id == null)
            {
                //return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                return RedirectToAction("Error", "Home");
            }
            TrackOrder trackOrder = db.TrackOrders.Find(id);
            if (trackOrder == null)
            {
                return RedirectToAction("Error", "Home");
            }
            return View(trackOrder);
        }


        // POST: TrackOrders/MarkComplete/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult MarkComplete([Bind(Include = "TrackId,OrderNumber,OrderPlaced,OrderPlacedDate,PreparingParcel," +
            "PrepParcelDate,InWarehouse,InWarehsDate," +
            "IsReady,IsReadyDate,InTransit,InTransitDate,IsDeliver,IsDeliverDate," +
            "colIdNum,idCompareNum,pin,comparePin,UserMail,delColect,driverId,driverNames")] TrackOrder trackOrder)
        {
            string message = null;
            if (trackOrder.idCompareNum == trackOrder.colIdNum && trackOrder.pin == trackOrder.comparePin)
            {
                if (ModelState.IsValid)
                {
                    trackOrder.IsDeliverDate = DateTime.Now.ToString();
                    trackOrder.IsDeliver = true;
                    var user = db.addInfoes.FirstOrDefault(a => a.addInfoOf == trackOrder.UserMail);
                    string names = $"{user.name} {user.surname}";
                    string emailSubject = $"Order {trackOrder.OrderNumber} update";
                    if (trackOrder.delColect == "Collect")
                    {
                        message = $"Hi { names}. There has been progress on order number { trackOrder.OrderNumber}," +
                                  $"Your order hase been collected from our. Thank you for putting your faith in us :)." +
                                  $"Contact us on gateboys@mail.com for enquiries";
                    }
                    else
                    {
                        message = $"Hi { names}. There has been progress on order number { trackOrder.OrderNumber}," +
                                $"Your order hase been delivered by our drivers and collected.Thank you for putting your faith in us :)." +
                                $"Contact us on gateboys@mail.com for enquiries";
                    }
                    emailhelper.sendMail(trackOrder.UserMail, emailSubject, message);

                    db.Entry(trackOrder).State = EntityState.Modified;
                    db.SaveChanges();
                    if (trackOrder.delColect == "Deliver")
                    {

                        return RedirectToAction("assignedToMe");
                    }
                    else
                    {
                        return RedirectToAction("Index");
                    }
                }
            }
            return View(trackOrder);
        }

        public ActionResult notify(int? id)
        {
            var parcel = db.TrackOrders.FirstOrDefault(a=>a.TrackId==id);

            if (parcel != null)
            {
                emailhelper.sendMail(parcel.UserMail,$"Order {parcel.OrderNumber} Ready for collection"
                    ,$"Hi There, this is to kindly notify you that order is ready for collection, Please bring your ID and " +
                    $"use the password {parcel.pin} to verify it you");
                return RedirectToAction("Index");

            }
            else
            {
                return RedirectToAction("Error", "Home");
            }
        }
        public ActionResult GetSearchOrder()
        {
            return View("GetSearchRecord");
        }

        public ActionResult GetSearchRecord(string search)
        {
            if (search == null)
            {
                return View();
            }
            var check = db.TrackOrders.ToList().Where(a => a.OrderNumber == search).ToList();
            if (check.Count < 1)
            {
                ViewBag.Errrr = " There is no parcel for the order number . Make sure you inserted the correct order number or Contact us via email we will get back to you or if you feel like calling us do so between 08:00 am to 17:00 pm.";
            }
            return View(check);
        }
        // GET: TrackOrders/Edit/5
        public ActionResult Error()
        {
            return View();
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
