using CKFinder.Settings;
using Model.Dao;
using Model.EF;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using System.Xml.Linq;

namespace OnlineShop.Areas.Admin.Controllers
{
    public class ProductController : BaseController
    {
        // GET: Admin/Product
        public ActionResult Index(string searchString, int page = 1, int pageSize = 5)
        {
            var dao = new ProductDao();
            var model = dao.ListAllPaging(searchString, page, pageSize);

            ViewBag.SearchString = searchString;
            return View(model);
        }
        [HttpGet]
        public ActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Create(Product product)
        {
            if (ModelState.IsValid)
            {
                var dao = new ProductDao();             
                long id = dao.Insert(product);
                if (id > 0)
                {
                    SetAlert("Thêm product thành công", "success");
                    return RedirectToAction("Index", "User");
                }
                else
                {
                    ModelState.AddModelError("", "Thêm product không thành công");
                }
            }
            return View("Index");
        }
        // set select list
        public SelectList SetViewBag(long ? selectedId = null)
        {
            var dao = new ProductCategoryDao();
            ViewBag.CategotyId = new SelectList(dao.ListAll(), "ID", "Name", selectedId);
            return new SelectList(dao.ListAll(), "ID", "Name", selectedId);
        }
        public JsonResult GetAllImage()
        {
            string pathImage = Server.MapPath("~/Data/images/");
            string[] imageFiles = Directory.GetFiles(pathImage);
            List<string> lstName = new List<string>();
            foreach (var item in imageFiles)
            {
                lstName.Add(Path.GetFileName(item));
            }
            return Json(new { status = true, lst = lstName}, JsonRequestBehavior.AllowGet);
        }
        public JsonResult RemoveImage(string name)
        {
            string pathImage = Server.MapPath("~/Data/images/") + name;
            if (System.IO.File.Exists(pathImage))
            {
                System.IO.File.Delete(pathImage);
            }
            return Json(new { satatus = true });
        }
        public JsonResult SaveImages(long id, string images)
        {
            JavaScriptSerializer scriptSerializer = new JavaScriptSerializer();
            var listImage = scriptSerializer.Deserialize<List<string>>(images);
            XElement xElement = new XElement("Images");
            foreach (var item in listImage)
            {
                xElement.Add(new XElement("Image", item));
            }
            var dao = new ProductDao();
            try
            {
                dao.UpdateImages(id, xElement.ToString());
                return Json(new { status = true });
            }
            catch (Exception ex)
            {
                return Json(new { status = false });
               
            }          
        }
        public JsonResult LoadImages(long id)
        {
            var dao = new ProductDao();
            var product = dao.ViewDetail(id);
            var images = product.MoreImages;
            if(images == null)
            {
                return Json(new { status = true, lst = new { } }, JsonRequestBehavior.AllowGet);
            }
            XElement xImages = XElement.Parse(images);
            List<string> lstImages = new List<string>();
            foreach (var item in xImages.Elements())
            {
                lstImages.Add(item.Value);
            }
            return Json(new { status = true, lst = lstImages }, JsonRequestBehavior.AllowGet);
        }
    }
}