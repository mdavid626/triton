using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Nyx.Data.DAL;
using Nyx.Web.Models;

namespace Nyx.Web.Controllers
{
    public class SettingsController : Controller
    {
        private NyxContext Db = new NyxContext();

        public ActionResult Index()
        {
            var vm = new SettingsViewModel(Db);
            vm.Load();
            return View(vm);
        }
    }
}