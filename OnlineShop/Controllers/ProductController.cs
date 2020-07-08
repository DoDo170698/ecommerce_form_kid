using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Xml.Linq;
using Model.Dao;

namespace OnlineShop.Controllers
{
    public class ProductController : Controller
    {
        // GET: Product      
        public ActionResult Index(int page = 1, int pageSize =5)
        {
            int totalProduct = 0;
            var model = new ProductDao().ListAllPagingIndex(page, pageSize, ref totalProduct);
            ViewBag.Total = totalProduct;
            ViewBag.Page = page;
            int maxPage = 5;
            int totalPage = 0;
            totalPage = (int)Math.Ceiling((double)(totalProduct / pageSize));
            ViewBag.TotalPage = totalPage;
            ViewBag.MaxPage = maxPage;
            ViewBag.First = 1;
            ViewBag.Last = totalPage;
            ViewBag.Next = page + 1;
            ViewBag.Prev = page - 1;
            
            return View(model);
        }
        // Mutil Menu
        [ChildActionOnly]
        public PartialViewResult ProductCategory()
        {
            try
            {
                var model = new ProductCategoryDao().ListAll();
                return PartialView(model);
            }
            catch (Exception ex)
            {

                throw;
            }

        }
        public JsonResult ListName(string q)
        {
            try
            {
                var data = new ProductDao().ListName(q);
                return Json(new
                {
                    data = data,
                    status = true
                }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {

                throw;
            }

        }      
        public ActionResult Category(long cateId, int page = 1, int pageSize = 5)
        {
            try
            {
                // check linq
                var check = new CategoryDao().check();
                // end 
                var category = new CategoryDao().ViewDetail(cateId);  
                ViewBag.Category = category;
                int totalRecord = 0;
                var model = new ProductDao().ListByCategoryId(cateId, ref totalRecord, page, pageSize);

                ViewBag.Total = totalRecord;
                ViewBag.Page = page;

                int maxPage = 5;
                int totalPage = 0;

                totalPage = (int)Math.Ceiling((double)(totalRecord / pageSize));
                ViewBag.TotalPage = totalPage;
                ViewBag.MaxPage = maxPage;
                ViewBag.First = 1;
                ViewBag.Last = totalPage;
                ViewBag.Next = page + 1;
                ViewBag.Prev = page - 1;

                return View(model);
            }
            catch (Exception ex)
            {

                throw;
            }
        }
        public ActionResult Search(string keyword, int page = 1, int pageSize = 1)
        {
            try
            {
                int totalRecord = 0;
                var model = new ProductDao().Search(keyword, ref totalRecord, page, pageSize);

                ViewBag.Total = totalRecord;
                ViewBag.Page = page;
                ViewBag.Keyword = keyword;
                int maxPage = 5;
                int totalPage = 0;

                totalPage = (int)Math.Ceiling((double)(totalRecord / pageSize));
                ViewBag.TotalPage = totalPage;
                ViewBag.MaxPage = maxPage;
                ViewBag.First = 1;
                ViewBag.Last = totalPage;
                ViewBag.Next = page + 1;
                ViewBag.Prev = page - 1;

                return View(model);
            }
            catch (Exception ex)
            {
                throw;
            }            
        }
        [OutputCache(CacheProfile = "Cache1DayForProduct")]
        public ActionResult Detail(long id)
        {
            try
            {
                //check change git
                var product = new ProductDao().ViewDetail(id);
                var images = product.MoreImages;
                List<string> lstImages = new List<string>();
                if(images == null)
                {
                    lstImages.Add("/Data/images/not_image.png");
                }
                else
                {
                    XElement xImages = XElement.Parse(images);
                    foreach (var item in xImages.Elements())
                    {
                        lstImages.Add(item.Value);
                    }
                }
                ViewBag.listMoreImage = lstImages;
                ViewBag.Category = new ProductCategoryDao().ViewDetail(product.CategoryID.Value);
                ViewBag.RelatedProducts = new ProductDao().ListRelatedProducts(id);
                return View(product);
            }
            catch (Exception ex)
            {
                throw;
            }            
        }
    }
}