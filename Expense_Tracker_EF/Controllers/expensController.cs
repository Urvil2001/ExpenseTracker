using Expense_Tracker_EF.Models;
using System;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Runtime.InteropServices;
using System.Web.Mvc;

namespace Expense_Tracker_EF.Controllers
{
    public class expensController : Controller
    {
        private Expense_TrackerEntities db = new Expense_TrackerEntities();

        // GET: expens
        public ActionResult Index()
        {
            var expenses = db.expenses.Include(e => e.Category_Exp);
            return View(expenses.ToList());
        }

        // GET: expens/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            expens expens = db.expenses.Find(id);
            if (expens == null)
            {
                return HttpNotFound();
            }
            return View(expens);
        }

        // GET: expens/Create
        public ActionResult Create()
        {
            ViewBag.cid = new SelectList(db.Category_Exp, "Id", "Category_Name");
            return View();
        }

        // POST: expens/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Title,Description,Amount,cid,ExpDate")] expens expens)
        {
            if (expens.cid == null)
            {
                ViewBag.cid = new SelectList(db.Category_Exp, "Id", "Category_Name", expens.cid);
                ViewBag.Message = "Please Select Category";
                return View();
            }
            else
            {
                //Comparision of category exp_Limit 
                var AmountUsed = db.expenses.Where(x => x.cid == expens.cid).Select(x => x.Amount).Sum(); //Get Sum of expenses amount based on selected category
                int categoryAmout =Convert.ToInt32(db.Category_Exp.Where(x => x.Id == expens.cid).Select(x => x.Category_Exp_Limit).Sum());  //Get Category limit of particular category
                int amountAdd = Convert.ToInt32(expens.Amount) + Convert.ToInt32(AmountUsed); //sum of already used amount and adding new amount in expense
                if (amountAdd > categoryAmout) //it will check whether used amount is greater then category_expense_limit
                {
                    ViewBag.Message = "Oops....Enter Amount is exceed Category Expense Limit by" + (amountAdd - categoryAmout).ToString();
                }
                else
                {
                    if (ModelState.IsValid)
                    {
                        try
                        {
                            db.expenses.Add(expens);
                            db.SaveChanges();
                            return RedirectToAction("Index");
                        }
                        catch(Exception e)
                        {
                            ViewBag.Message = "You Entered Wrong Date";
                        }
                       
                    }
                }
                ViewBag.cid = new SelectList(db.Category_Exp, "Id", "Category_Name", expens.cid);
                return View(expens);
            }
        }

        // GET: expens/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            expens expens = db.expenses.Find(id);
            if (expens == null)
            {
                return HttpNotFound();
            }
            ViewBag.cid = new SelectList(db.Category_Exp, "Id", "Category_Name", expens.cid);
            return View(expens);
        }

        // POST: expens/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Title,Description,Amount,cid,ExpDate")] expens expens)
        {
            int AmountUsed =Convert.ToInt32(db.expenses.Where(x => x.cid == expens.cid).Select(x => x.Amount).Sum()); //get amount already used on particular category
            int getAmount = Convert.ToInt32(db.expenses.Where(x => x.Id == expens.Id).Select(x => x.Amount).Sum()); //how many amount we used on inserted expense
            int categoryAmout = Convert.ToInt32(db.Category_Exp.Where(x => x.Id == expens.cid).Select(x => x.Category_Exp_Limit).Sum()); //get expense_limit of particular category
          
            int MinusAlreadyAmount = AmountUsed - getAmount; // it will minus used amount on particular category from inserted expense
            int totalAmount = MinusAlreadyAmount + Convert.ToInt32(expens.Amount); //sum of AlreadAmount and edited amount


            if (totalAmount > categoryAmout) //checking condition
            {
                ViewBag.Message = "Oops....Enter Amount is exceed Category Expense Limit by" + (totalAmount - categoryAmout).ToString();
            }
            else
            {
                if (ModelState.IsValid)
                {
                    db.Entry(expens).State = EntityState.Modified;
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
               
            }
            ViewBag.cid = new SelectList(db.Category_Exp, "Id", "Category_Name", expens.cid);
            return View(expens);
        }

        // GET: expens/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            expens expens = db.expenses.Find(id);
            if (expens == null)
            {
                return HttpNotFound();
            }
            return View(expens);
        }

        // POST: expens/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            expens expens = db.expenses.Find(id);
            db.expenses.Remove(expens);
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
