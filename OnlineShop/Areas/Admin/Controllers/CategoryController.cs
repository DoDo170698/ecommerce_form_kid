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
    public class CategoryController : BaseController
    {
        // GET: Admin/Category
        public ActionResult Index(string search, int page = 1, int pageSize = 5)
        {
            var model = new CategoryDao().ListAllPaging(search, page, pageSize);
            ViewBag.SearchString = search;
            return View(model);
        }
        [HttpGet]
        public ActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Create(Category model)
        {
            if (ModelState.IsValid)
            {
                var currentCulture = Session[CommonConstants.CurrentCulture];
                model.Language = currentCulture.ToString();
                var id = new CategoryDao().Insert(model);
                if (id > 0)
                {
                    return RedirectToAction("Index");
                }
                else
                {
                    ModelState.AddModelError("", StaticResources.Resources.InsertCategoryFailed);
                }
            }
            return View(model);
        }
        [HttpGet]
        public ActionResult Edit(int id)
        {
            if(id <= 0)
            {
                return new HttpStatusCodeResult(System.Net.HttpStatusCode.BadRequest);
            }
            var data = new CategoryDao().ViewById(id); 
            return View(data);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Category category)
        {
            if (ModelState.IsValid)
            {
                var user = (UserLogin)Session[CommonConstants.USER_SESSION];
                category.CreatedBy = user.Name;
                category.CreatedDate = DateTime.Now;
                new CategoryDao().Update(category);
                return RedirectToAction("Index");
            }
            return View(category);
        }
        public JsonResult Delete(int id)
        {
            if(id <= 0)
            {
                return Json(new { status = false, mess = "không xóa được" });
            }
            var data = new CategoryDao().ViewById(id);
            if (data.Status.Value)
            {
                return Json(new { status = false, mess = "không xóa được" });
            }
            var count = new CategoryDao().ProductWithCategory(id);
            if(count > 0)
            {
                return Json(new { status = false, mess = "không xóa được" });
            }
            var result = new CategoryDao().Delete(id);
            if (result)
            {
                return Json(new { status = true, mess = "xóa thành công" });
            }
            else
            {
                return Json(new { status = false, mess = "không xóa được" });
            }
        }
        public JsonResult ChangeStatus(int id)
        {
            if (id <= 0)
            {
                return Json(new { status = false, mess = "không thay đổi được" });
            }
            var result = new CategoryDao().ChangeStatus(id);
            return Json(new { status = result, mess = "thay đổi thành công" });
        }
    }
}