using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Model.Dao;
using Model.EF;

namespace OnlineShop.Areas.Admin.Controllers
{
    public class HomeController : BaseController
    {
        // GET: Admin/Home
        /// <summary>
        /// Dashboad
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            ViewBag.feedBack = new ContactDao().SumFeedBack();
            ViewBag.user = new UserDao().SumUser();
            ViewBag.order = new OrderDao().SumOrder();
            ViewBag.product = new ProductDao().SumProduct();
            return View();
        }
    }
}