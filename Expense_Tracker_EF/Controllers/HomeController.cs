using Expense_Tracker_EF.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Expense_Tracker_EF.Controllers
{
    public class HomeController : Controller
    {
        private Expense_TrackerEntities db = new Expense_TrackerEntities();

        //Below function is return Category_Expende, Expense,Total_Expense_Limit table
        public ActionResult Index()
        {
            var tables = new IeViewModel
            {
                Category_Exp = db.Category_Exp.ToList(),
                expens = db.expenses.ToList(),
                Total_Expense_Limit = db.Total_Expense_Limit.ToList(),
            };

            return View(tables);
        }


       

        //Below function is used for generate pi chart
        public ActionResult GetChart() {

            
            var list = db.Category_Exp.Select(x => new {
                Title = x.Category_Name,
                Amount = x.Category_Exp_Limit,

            }).ToList();
            // return JSON data
            return Json(list, JsonRequestBehavior.AllowGet);
        }

    }
}