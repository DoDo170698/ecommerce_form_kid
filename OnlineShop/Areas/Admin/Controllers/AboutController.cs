using Model.Dao;
using Model.EF;
using OnlineShop.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace OnlineShop.Areas.Admin.Controllers
{
    public class AboutController : BaseController
    {
        // GET: Admin/About
        public ActionResult Index(string searchString, int page = 1, int pageSize = 10)
        {
            var dao = new AboutDao();
            var model = dao.ListAllPaging(searchString, page, pageSize).FirstOrDefault() ?? new About();
            return View(model);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Save(About about)
        {
            if (ModelState.IsValid)
            {
                var user = (UserLogin)Session[CommonConstants.USER_SESSION];
                if(about.ID <= 0)
                {
                    about.CreatedBy = user.UserName;
                    about.CreatedDate = DateTime.Now;
                    new AboutDao().Create(about);
                    return RedirectToAction("Index");
                }
                else
                {
                    new AboutDao().Edit(about);
                    return RedirectToAction("Index");
                }
            }
            return View(about);
        }
    }
}