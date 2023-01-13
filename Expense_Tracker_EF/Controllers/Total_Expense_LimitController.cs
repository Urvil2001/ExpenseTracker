using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Expense_Tracker_EF.Models;

namespace Expense_Tracker_EF.Controllers
{
    public class Total_Expense_LimitController : Controller
    {
        private Expense_TrackerEntities db = new Expense_TrackerEntities();

        // GET: Total_Expense_Limit
        public ActionResult Index()
        {
            return View(db.Total_Expense_Limit.ToList());
        }

        // GET: Total_Expense_Limit/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Total_Expense_Limit total_Expense_Limit = db.Total_Expense_Limit.Find(id);
            if (total_Expense_Limit == null)
            {
                return HttpNotFound();
            }
            return View(total_Expense_Limit);
        }

        // GET: Total_Expense_Limit/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Total_Expense_Limit/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,TotalExpenseLimit")] Total_Expense_Limit total_Expense_Limit)
        {
            if (ModelState.IsValid)
            {
                db.Total_Expense_Limit.Add(total_Expense_Limit);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(total_Expense_Limit);
        }

        // GET: Total_Expense_Limit/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Total_Expense_Limit total_Expense_Limit = db.Total_Expense_Limit.Find(id);
            if (total_Expense_Limit == null)
            {
                return HttpNotFound();
            }
            return View(total_Expense_Limit);
        }

        // POST: Total_Expense_Limit/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,TotalExpenseLimit")] Total_Expense_Limit total_Expense_Limit)
        {
            if (ModelState.IsValid)
            {
                db.Entry(total_Expense_Limit).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(total_Expense_Limit);
        }

        // GET: Total_Expense_Limit/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Total_Expense_Limit total_Expense_Limit = db.Total_Expense_Limit.Find(id);
            if (total_Expense_Limit == null)
            {
                return HttpNotFound();
            }
            return View(total_Expense_Limit);
        }

        // POST: Total_Expense_Limit/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Total_Expense_Limit total_Expense_Limit = db.Total_Expense_Limit.Find(id);
            db.Total_Expense_Limit.Remove(total_Expense_Limit);
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
