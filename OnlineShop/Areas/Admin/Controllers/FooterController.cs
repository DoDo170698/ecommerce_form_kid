using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Model.Dao;
using Model.EF;

namespace OnlineShop.Areas.Admin.Controllers
{
    public class FooterController : BaseController
    {
        // GET: Admin/Footer
        [HasCredential(RoleID = "VIEW_FOOTER")]
        public ActionResult Index(int page = 1, int pageSize = 5)
        {
            var model = new FooterDao().GetAllPaging(page, pageSize);
            return View(model);
        }
        [HttpGet]
        [HasCredential(RoleID = "ADD_FOOTER")]
        public ActionResult Create()
        {
            return View(new Footer());
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        [ValidateInput(false)]
        [HasCredential(RoleID = "ADD_FOOTER")]
        public ActionResult Create(Footer footer)
        {
            if (ModelState.IsValid)
            {
                new FooterDao().Create(footer);
                return RedirectToAction("Index");
            }
            return View(footer);
        }
        [HttpGet]
        [HasCredential(RoleID = "EDIT_FOOTER")]
        public ActionResult Edit(string id)
        {
            var data = new FooterDao().ViewByID(id);
            return View(data);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        [ValidateInput(false)]
        [HasCredential(RoleID = "EDIT_FOOTER")]
        public ActionResult Edit(Footer footer)
        {
            if (ModelState.IsValid)
            {
                new FooterDao().Update(footer);
                return RedirectToAction("Index");
            }
            return View(footer);
        }
        [HasCredential(RoleID = "EDIT_FOOTER")]
        public JsonResult ChangeStatus(string id)
        {
            if (String.IsNullOrEmpty(id))
            {
                return Json(new { status = false, mess = "False" });
            }
            var result = new FooterDao().ChangeStatus(id);
            return Json(new { status = result, mess = "Thay đổi thành công" });
        }
        [HasCredential(RoleID = "DELETE_FOOTER")]
        public JsonResult Delete(string id)
        {
            if (String.IsNullOrEmpty(id))
            {
                return Json(new { status = false, mess = "False" });
            }
            var data = new FooterDao().ViewByID(id);
            if (data.Status.Value)
            {
                return Json(new { status = false, mess = "Không xóa được" });
            }
            new FooterDao().Delete(id);
            return Json(new { status = true, mess = "Xóa thành công" });
        }
    }
}