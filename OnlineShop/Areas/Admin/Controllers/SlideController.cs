using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Model.Dao;
using Model.EF;
using OnlineShop.Common;
using PagedList;

namespace OnlineShop.Areas.Admin.Controllers
{
    public class SlideController : BaseController
    {
        OnlineShopDbContext db = null;
        public SlideController()
        {
            db = new OnlineShopDbContext();
        }
        // GET: Admin/Slide
        public ActionResult Index(string search, int page = 1, int pageSize = 5)
        {
            var model = new SlideDao().ListAllPaging(search , page, pageSize);
            ViewBag.SearchString = search;
            return View(model);
        }
        [HttpGet]
        public ActionResult Create()
        {
            return View(new Slide());
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Slide entity, HttpPostedFileBase file)
        {
            string path = Server.MapPath("~/assets/client/images/");
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
            var defName = DateTime.Now.ToString("MM_dd_yyyy_HH_mm_ss");
            if (file != null)
            {
                var extension = Path.GetExtension(file.FileName);
                string fileName = defName + "_" + Path.GetFileNameWithoutExtension(file.FileName) + extension;
                file.SaveAs(Path.Combine(path, fileName));
            }
            if (ModelState.IsValid)
            {
                var user = (UserLogin)Session[CommonConstants.USER_SESSION];
                entity.CreatedBy = user.Name;
                entity.CreatedDate = DateTime.Now;
                if (entity.Status == null)
                    entity.Status = false;
                if (file != null)
                {
                    var extension = Path.GetExtension(file.FileName);
                    entity.Image = "/assets/client/images/" + defName + "_" + Path.GetFileNameWithoutExtension(file.FileName) + extension;
                }
                db.Slides.Add(entity);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(entity);
        }
        [HttpGet]
        public ActionResult Edit(int id)
        {
            if(id <= 0)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var model = db.Slides.Find(id);
            return View(model);
        }
        [HttpPost]
        public ActionResult Edit(Slide entity, HttpPostedFileBase file)
        {
            string pathOldImage = Server.MapPath("~") + entity.Image;
            if (System.IO.File.Exists(pathOldImage))
            {
                System.IO.File.Delete(pathOldImage);
            }
            string path = Server.MapPath("~/assets/client/images/");
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
            var defName = DateTime.Now.ToString("MM_dd_yyyy_HH_mm_ss");
            if (file != null)
            {
                var extension = Path.GetExtension(file.FileName);
                string fileName = defName + "_" + Path.GetFileNameWithoutExtension(file.FileName) + extension;
                file.SaveAs(Path.Combine(path, fileName));
            }
            if (ModelState.IsValid)
            {
                var user = (UserLogin)Session[CommonConstants.USER_SESSION];
                entity.CreatedBy = user.Name;
                entity.CreatedDate = DateTime.Now;
                if (entity.Status == null)
                    entity.Status = false;
                if (file != null)
                {
                    var extension = Path.GetExtension(file.FileName);
                    entity.Image = "/assets/client/images/" + defName + "_" + Path.GetFileNameWithoutExtension(file.FileName) + extension;
                }
                db.Entry(entity).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(entity);
        }
        public JsonResult Delete(int id)
        {
            if(id <= 0)
            {
                return Json(new { status = false });
            }
            var data = db.Slides.Find(id);
            string pathImage = Server.MapPath("~") + data.Image;
            
            if (data.Status.Value)
            {
                return Json(new { status = false, mess = "Không xóa được" });
            }
            else
            {
                if (System.IO.File.Exists(pathImage))
                {
                    System.IO.File.Delete(pathImage);
                }
                db.Slides.Remove(data);
                db.SaveChanges();
                return Json(new { status = true, mess = "Xóa thành công" });
            }
        }
        public JsonResult ChangeStatus(int id)
        {
            if(id <= 0)
            {
                return Json(new { status = false });
            }
            var entity = db.Slides.Find(id);
            entity.Status = !entity.Status;
            db.SaveChanges();
            return Json(new { status = entity.Status.Value });
        }
    }
}