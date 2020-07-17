using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Model.Dao;
using Model.EF;

namespace OnlineShop.Areas.Admin.Controllers
{
    public class OrderController : BaseController
    {
        // GET: Admin/Order
        [HasCredential(RoleID = "VIEW_ORDER")]
        public ActionResult Index(int page = 1, int pageSize = 5)
        {
            var model = new OrderDao().GetAllPaging(page, pageSize);
            return View(model);
        }
        [HasCredential(RoleID = "EDIT_ORDER")]
        public JsonResult ChangeStatus(int id)
        {
            var data = new OrderDao().CheckExistsOrder((long)id);
            if (data)
            {
                return Json(new { status = false, check = false, mess = "đơn đã đặt ship, không chuyển đc trạng thái" });
            }
            var result = new OrderDao().ChangeStatus(id);
            return Json(new
            {
                status = result, 
                check = true
            });
        }
        [HasCredential(RoleID = "VIEW_ORDER")]
        public ActionResult ViewDetail(int id)
        {
            var model = new OrderDao().ViewDetail(id).ToList();
            return View(model);
        }
    }
}