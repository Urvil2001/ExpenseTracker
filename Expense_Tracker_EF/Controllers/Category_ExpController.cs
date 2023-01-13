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
    public class Category_ExpController : Controller
    {
        private Expense_TrackerEntities db = new Expense_TrackerEntities();

        // GET: Category_Exp
        public ActionResult Index()
        {
            return View(db.Category_Exp.ToList());
        }

        // GET: Category_Exp/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Category_Exp category_Exp = db.Category_Exp.Find(id);
            var records = from c in db.Category_Exp
                          join e in db.expenses on c.Id equals e.cid where c.Id==id 
                          select new ViewModel
                          {
                              Category_Exp = c,
                              expens = e,
                          };


            if (category_Exp == null)
            {
                return HttpNotFound();
            }
            return View(records.ToList());
        }

        // GET: Category_Exp/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Category_Exp/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Category_Name,Category_Exp_Limit")] Category_Exp category_Exp)
        {
            var getSumOfCategory = db.Category_Exp.Select(x => x.Category_Exp_Limit).Sum(); //Get Sum of category_Exp_Limit from category_Exp table
            int getTotalExpense = Convert.ToInt32(db.Total_Expense_Limit.Select(x => x.TotalExpenseLimit).Sum()); //Get TotalExpenseLimit from Total_Expense_Limit
            int TotalCategoryExp=Convert.ToInt32(getSumOfCategory)+Convert.ToInt32(category_Exp.Category_Exp_Limit); //sum of getSumOfCategory and inserted Catergory_Exp_Limit from textbox

            if (TotalCategoryExp > getTotalExpense)
            {
                ViewBag.Message = "Oops....Enter Amount is exceed Total Category Expense Limit by" + (TotalCategoryExp - getTotalExpense).ToString();

            }
            else { 
                if (ModelState.IsValid)
                {
                    db.Category_Exp.Add(category_Exp);
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
            }
            return View(category_Exp);
        }

        // GET: Category_Exp/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Category_Exp category_Exp = db.Category_Exp.Find(id);
            if (category_Exp == null)
            {
                return HttpNotFound();
            }
            return View(category_Exp);
        }

        // POST: Category_Exp/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Category_Name,Category_Exp_Limit")] Category_Exp category_Exp)
        {
            int AmountUsed = Convert.ToInt32(db.Category_Exp.Select(x => x.Category_Exp_Limit).Sum()); //get all Categroy_Expense_Limit from Category_exp table
            int getAmount = Convert.ToInt32(db.Category_Exp.Where(x => x.Id == category_Exp.Id).Select(x => x.Category_Exp_Limit).Sum()); //how many amount we used on already inserted Category_exp table
            int TotalExpAmount = Convert.ToInt32(db.Total_Expense_Limit.Select(x => x.TotalExpenseLimit).Sum()); //get TotalExpenseLimit from Total_Expense_Limit table

            int MinusAlreadyAmount = AmountUsed - getAmount; // it will minus used amount of all category from already inserted category
            int totalAmount = MinusAlreadyAmount + Convert.ToInt32(category_Exp.Category_Exp_Limit);
            if (totalAmount > TotalExpAmount)
            {
                ViewBag.Message = "Oops....Enter Amount is exceed Category Expense Limit by" + (totalAmount - TotalExpAmount).ToString();
            }
            else
            {
                if (ModelState.IsValid)
                {
                    db.Entry(category_Exp).State = EntityState.Modified;
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
            }
            return View(category_Exp);
        }

        // GET: Category_Exp/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Category_Exp category_Exp = db.Category_Exp.Find(id);
            if (category_Exp == null)
            {
                return HttpNotFound();
            }
            return View(category_Exp);
        }

        // POST: Category_Exp/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Category_Exp category_Exp = db.Category_Exp.Find(id);
            db.Category_Exp.Remove(category_Exp);
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

        //This function is used to bind category name to dropdown
        public ActionResult FilterCategory()
        {
            ViewBag.cid = new SelectList(db.Category_Exp, "Id", "Category_Name");
            return View(db.expenses.ToList());
        }

        //This function is used to display expense in view while selecting category from dropdown
        public ActionResult GetResults(int id)
        {
            // example query from table
            var expense_Data = db.expenses.Where(x => x.cid == id).Select(x => new {
                Title = x.Title,
                Amount = x.Amount,
                Description = x.Description,
            }).ToList();
            // return JSON data
            return Json(expense_Data, JsonRequestBehavior.AllowGet);
        }
    }
}
