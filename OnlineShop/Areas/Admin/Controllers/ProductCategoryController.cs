using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Model.Dao;
using Model.EF;
using OnlineShop.Common;
using System.Data.Entity;
using PagedList;
using System.Net;

namespace OnlineShop.Areas.Admin.Controllers
{
    public class ProductCategoryController : BaseController
    {
        OnlineShopDbContext db = null;
        public ProductCategoryController()
        {
            db = new OnlineShopDbContext();
        }
        // GET: Admin/ProductCategory
        [HasCredential(RoleID = "VIEW_PRODUCT_CATEGORY")]
        public ActionResult Index(string search, int page = 1, int pageSize = 5)
        {
            IQueryable<ProductCategory> model = db.ProductCategories;
            if (!string.IsNullOrEmpty(search))
            {
                model.Where(x => x.Name.Contains(search));
            }
            var result = model.OrderByDescending(x => x.Name).ToPagedList(page, pageSize);
            ViewBag.SearchString = search;
            return View(result);
        }
        [HttpGet]
        [HasCredential(RoleID = "ADD_PRODUCT_CATEGORY")]
        public ActionResult Create()
        {
            return View(new ProductCategory());
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        [HasCredential(RoleID = "ADD_PRODUCT_CATEGORY")]
        public ActionResult Create(ProductCategory entity)
        {
            if (ModelState.IsValid)
            {
                var user = (UserLogin)Session[CommonConstants.USER_SESSION];
                entity.CreatedBy = user.Name;
                entity.CreatedDate = DateTime.Now;
                if (entity.Status == null)
                    entity.Status = false;

                db.ProductCategories.Add(entity);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(entity);
        }
        [HttpGet]
        [HasCredential(RoleID = "EDIT_PRODUCT_CATEGORY")]
        public ActionResult Edit(int id)
        {
            if (id <= 0)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var data = db.ProductCategories.Find(id);
            return View(data);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        [HasCredential(RoleID = "EDIT_PRODUCT_CATEGORY")]
        public ActionResult Edit(ProductCategory entity)
        {
            if (ModelState.IsValid)
            {
                var user = (UserLogin)Session[CommonConstants.USER_SESSION];
                entity.CreatedBy = user.Name;
                entity.CreatedDate = DateTime.Now;
                if (entity.Status == null)
                    entity.Status = false;

                db.Entry(entity).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(entity);
        }
        [HasCredential(RoleID = "DELETE_PRODUCT_CATEGORY")]
        public JsonResult Delete(int id)
        {
            if (id <= 0)
            {
                return Json(new { status = false, mess = "Xóa ko thành công"});
            }
            var result = db.ProductCategories.Join(db.Products, a => a.ID, b => b.CategoryID, (a, b) => new { a, b }).Where(c => c.a.ID == id).Select(x => new
            {
                x.b.ID,
                x.b.Name
            }).ToList();
            if(result.Count > 0)
            {
                return Json(new { status = false, mess = "Đã dùng, không xóa được" });
            }
            var data = db.ProductCategories.Find(id);
            if (data.Status.Value)
            {
                return Json(new { status = false, mess = "Status = true, không được xóa" });
            }
            db.ProductCategories.Remove(data);
            db.SaveChanges();
            return Json(new { status = true, mess = "Xóa thành công" });
        }
        [HasCredential(RoleID = "EDIT_PRODUCT_CATEGORY")]
        public JsonResult ChangeStatus(int id)
        {
            if(id <= 0)
            {
                return Json(new { status = false, mess = "không thay đổi được" });
            }
            var data = db.ProductCategories.Find(id);
            data.Status = !data.Status;
            db.Entry(data).State = EntityState.Modified;
            db.SaveChanges();
            return Json(new { status = data.Status, mess = "thay đổi thành công" });
        }
    }
}