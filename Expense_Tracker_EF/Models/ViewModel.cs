using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Expense_Tracker_EF.Models
{
    public class ViewModel
    {
        public Category_Exp Category_Exp { get; set; }
        public expens expens { get; set; }
    }

    public class IeViewModel
    {
        public IEnumerable<Category_Exp> Category_Exp { get; set; }
        public IEnumerable<expens> expens { get; set; }

        public IEnumerable<Total_Expense_Limit> Total_Expense_Limit { get;set; }
    }
}