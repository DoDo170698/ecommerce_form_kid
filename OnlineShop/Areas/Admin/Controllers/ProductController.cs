using CKFinder.Settings;
using Model.Dao;
using Model.EF;
using OnlineShop.Common;
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
        [HasCredential(RoleID = "VIEW_PRODUCT")]
        public ActionResult Index(string searchString, int page = 1, int pageSize = 5)
        {
            var dao = new ProductDao();
            var model = dao.ListAllPaging(searchString, page, pageSize);

            ViewBag.SearchString = searchString;
            return View(model);
        }
        [HttpGet]
        [HasCredential(RoleID = "ADD_PRODUCT")]
        public ActionResult Create()
        {
            return View(new Product());
        }
        [HttpPost]
        [HasCredential(RoleID = "ADD_PRODUCT")]
        public ActionResult Create(Product product, HttpPostedFileBase file)
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
                var dao = new ProductDao();
                var user = (UserLogin)Session[CommonConstants.USER_SESSION];
                product.CreatedBy = user.Name;
                product.CreatedDate = DateTime.Now;
                if (product.Status == null)
                    product.Status = false;
                if (file != null)
                {
                    var extension = Path.GetExtension(file.FileName);
                    product.Image = "/assets/client/images/" + defName + "_" + Path.GetFileNameWithoutExtension(file.FileName) + extension;
                }
                long id = dao.Insert(product);
                if (id > 0)
                {
                    SetAlert("Thêm product thành công", "success");
                    return RedirectToAction("Index", "Product");
                }
                else
                {
                    ModelState.AddModelError("", "Thêm product không thành công");
                }
            }
            return View(product);
        }
        [HttpGet]
        [HasCredential(RoleID = "EDIT_PRODUCT")]
        public ActionResult Edit(int id)
        {
            var data = new ProductDao().ViewDetail((long)id);
            return View(data);
        }
        [HttpPost]
        [HasCredential(RoleID = "EDIT_PRODUCT")]
        public ActionResult Edit(Product product, HttpPostedFileBase file)
        {
            var oldProduct = new ProductDao().ViewDetail((long)product.ID);
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
                product.CreatedBy = user.Name;
                product.CreatedDate = DateTime.Now;
                if (product.Status == null)
                    product.Status = false;
                
                if (file != null)
                {
                    var extension = Path.GetExtension(file.FileName);
                    product.Image = "/assets/client/images/" + defName + "_" + Path.GetFileNameWithoutExtension(file.FileName) + extension;
                }
                else
                {
                    product.Image = oldProduct.Image;
                }
                new ProductDao().Update(product);
                return RedirectToAction("Index");
            }
            return View(product);
        }
        [HasCredential(RoleID = "DELETE_PRODUCT")]
        public JsonResult Delete(int id)
        {
            if(id <= 0)
            {
                return Json(new { status = false, mess = "không được xóa" });
            }
            var data = new ProductDao().ViewDetail((long)id);
            if (data.Status.Value)
            {
                return Json(new { status = false, mess = "không được xóa" });
            }
            var result = new ProductDao().Delete(id);
            if (result)
            {
                return Json(new { status = true, mess = "xóa thành công" });
            }
            else
            {
                return Json(new { status = false, mess = "không xóa được" });
            }
        }
        [HasCredential(RoleID = "EDIT_PRODUCT")]
        public JsonResult ChangeStatus(int id)
        {
            if (id <= 0)
            {
                return Json(new { status = false, mess = "không được xóa" });
            }
            var result = new ProductDao().ChangeStatus(id);
            return Json(new { status = result, mess = "thay đổi thành công" });
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