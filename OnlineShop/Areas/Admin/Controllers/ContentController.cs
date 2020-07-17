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
    public class ContentController : BaseController
    {
        // GET: Admin/Content
        [HasCredential(RoleID = "VIEW_CONTENT")]
        public ActionResult Index(string searchString, int page = 1, int pageSize = 10)
        {
            var dao = new ContentDao();
            var model = dao.ListAllPaging(searchString, page, pageSize);

            ViewBag.SearchString = searchString;
            return View(model);
        }
        [HttpGet]
        [HasCredential(RoleID = "ADD_CONTENT")]
        public ActionResult Create()
        {
            SetViewBag();
            return View(new Content());
        }
        [HttpPost]      
        [ValidateInput(false)]
        [ValidateAntiForgeryToken]
        [HasCredential(RoleID = "ADD_CONTENT")]
        public ActionResult Create(Content model)
        {
            if (ModelState.IsValid)
            {
                var session = (UserLogin)Session[CommonConstants.USER_SESSION];
                model.CreatedBy = session.UserName;
                model.CreatedDate = DateTime.Now;               
                var culture = Session[CommonConstants.CurrentCulture];
                model.Language = culture.ToString();
                new ContentDao().Create(model);
                return RedirectToAction("Index");
            }
            SetViewBag();
            return View(model);
        }
        [HttpGet]
        [HasCredential(RoleID = "EDIT_CONTENT")]
        public ActionResult Edit(long id)
        {
            if(id <= 0)
            {
                return new HttpStatusCodeResult(System.Net.HttpStatusCode.BadRequest);
            }
            var dao = new ContentDao();
            var content = dao.GetByID(id);
            SetViewBag(content.CategoryID);
            return View(content);
        }
        [HttpPost]
        [ValidateInput(false)]
        [ValidateAntiForgeryToken]
        [HasCredential(RoleID = "EDIT_CONTENT")]
        public ActionResult Edit(Content model)
        {
            if (ModelState.IsValid)
            {
                var session = (UserLogin)Session[CommonConstants.USER_SESSION];
                model.CreatedBy = session.UserName;
                model.CreatedDate = DateTime.Now;
                var culture = Session[CommonConstants.CurrentCulture];
                model.Language = culture.ToString();
                new ContentDao().Edit(model);
                return RedirectToAction("Index");
            }
            SetViewBag(model.CategoryID);
            return View(model);
        }
        [HasCredential(RoleID = "EDIT_CONTENT")]
        public JsonResult ChangeStatus(int id)
        {
            if (id <= 0)
            {
                return Json(new { status = false, mess = "không thay đổi được" });
            }
            var result = new ContentDao().ChangeStatus(id);
            return Json(new { status = result, mess = "thay đổi thành công" });
        }
        [HasCredential(RoleID = "DELETE_CONTENT")]
        public JsonResult Delete(int id)
        {
            if (id <= 0)
            {
                return Json(new { status = false, mess = "không xóa được" });
            }
            var data = new ContentDao().GetByID((long)id);
            if (data.Status)
            {
                return Json(new { status = false, mess = "không xóa được" });
            }            
            var result = new ContentDao().Delete(id);
            if (result)
            {
                return Json(new { status = true, mess = "xóa thành công" });
            }
            else
            {
                return Json(new { status = false, mess = "không xóa được" });
            }
        }
        // set dropdowlist
        public void SetViewBag(long? selectedId = null)
        {
            var dao = new CategoryDao();
            ViewBag.CategoryID = new SelectList(dao.ListAll(), "ID", "Name", selectedId);
        }
    }
}