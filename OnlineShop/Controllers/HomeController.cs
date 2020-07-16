using Model.Dao;
using OnlineShop.Common;
using OnlineShop.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.UI;

namespace OnlineShop.Controllers
{
    public class HomeController : Controller
    {
        // GET: Home
        public ActionResult Index()
        {
            try
            {
                ViewBag.Slides = new SlideDao().ListAll();
                var productDao = new ProductDao();
                ViewBag.NewProducts = productDao.ListNewProduct(3);
                ViewBag.ListFeatureProducts = productDao.ListFeatureProduct(3);
                ViewBag.FooterHome = new FooterDao().GetFooterHome();
                return View();
            }
            catch (Exception ex)
            {

                throw;
            }
        }
        [ChildActionOnly]        
        public PartialViewResult MainMenu()
        {
            try
            {
                var model = new MenuDao().ListByGroupId(1);
                return PartialView(model);
            }
            catch (Exception ex)
            {

                throw;
            }
        }
        [ChildActionOnly]
        [OutputCache(Duration = 3600 * 24)]
        public ActionResult TopMenu()
        {
            try
            {
                var model = new MenuDao().ListByGroupId(2);
                return PartialView(model);
            }
            catch (Exception ex)
            {

                throw;
            }
        }
        [ChildActionOnly]
        public PartialViewResult HeaderCart()
        {
            try
            {
                var cart = Session[CommonConstants.CartSession];
                var list = new List<CartItem>();
                if (cart != null)
                {
                    list = (List<CartItem>)cart;
                }

                return PartialView(list);
            }
            catch (Exception ex)
            {

                throw;
            }
        }
        public ActionResult Footer()
        {
            try
            {
                var model = new FooterDao().GetFooter();
                return PartialView(model);
            }
            catch (Exception ex)
            {

                throw;
            }

        }
    }
}