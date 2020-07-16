using Model.Dao;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace OnlineShop.Controllers
{
    public class AboutController : Controller
    {
        // GET: About
        public ActionResult Index()
        {
            try
            {
                var model = new AboutDao().GetAbout();
                return View(model);
            }
            catch (Exception ex)
            {

                throw;
            }
        }
    }
}