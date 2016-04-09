using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Nyx.Web.DAL;

namespace Nyx.Web.Controllers
{
    public class HomeController : Controller
    {
        private NyxContext Db = new NyxContext();

        public ActionResult Index()
        {
            return View(Db.Transactions.ToList());
        }
    }
}
