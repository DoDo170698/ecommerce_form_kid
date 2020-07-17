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
        [HasCredential(RoleID = "VIEW_ABOUT")]
        public ActionResult Index(string searchString, int page = 1, int pageSize = 10)
        {
            var dao = new AboutDao();
            var model = dao.ListAllPaging(searchString, page, pageSize);
            return View(model);
        }
        [HttpGet]
        [HasCredential(RoleID = "ADD_ABOUT")]
        public ActionResult Create ()
        {
            return View(new About());
        }
        [HasCredential(RoleID = "EDIT_ABOUT")]
        [HttpGet]
        public ActionResult Edit(long id)
        {
            if(id <= 0)
            {
                return new HttpStatusCodeResult(System.Net.HttpStatusCode.BadRequest);
            }
            var data = new AboutDao().GetByID(id);
            return View(data);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        [ValidateInput(false)]
        [HasCredential(RoleID = "ADD_ABOUT")]
        public ActionResult Save(About about)
        {
            if (ModelState.IsValid)
            {
                var user = (UserLogin)Session[CommonConstants.USER_SESSION];
                if(about.ID <= 0)
                {
                    about.CreatedBy = user.UserName;
                    about.CreatedDate = DateTime.Now;
                    about.Status = true;
                    new AboutDao().Create(about);
                    return RedirectToAction("Index");
                }
                else
                {
                    about.CreatedBy = user.UserName;                 
                    about.Status = true;
                    new AboutDao().Edit(about);
                    return RedirectToAction("Index");
                }
            }
            return View(about);
        }
        [HasCredential(RoleID = "DELETE_ABOUT")]
        public JsonResult Delete(int id)
        {
            if (id <= 0)
            {
                return Json(new { status = false, mess = "False" });
            }
            var data = new AboutDao().GetByID((long)id);
            if (data.Status.Value)
            {
                return Json(new { status = false, mess = "Không xóa được" });
            }
            new AboutDao().Delete(id);
            return Json(new { status = true, mess = "Xóa thành công" });
        }
        [HasCredential(RoleID = "EDIT_ABOUT")]
        public JsonResult ChangeStatus(int id)
        {
            if (id <= 0)
            {
                return Json(new { status = false, mess = "False" });
            }
            var result = new AboutDao().ChangeStatus(id);
            return Json(new { status = result, mess = "Thay đổi thành công" });
        }
    }
}