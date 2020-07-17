using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Model.Dao;
using Model.EF;
using Model.ViewModel;

namespace OnlineShop.Areas.Admin.Controllers
{
    public class ShipperOrderController : BaseController
    {
        // GET: Admin/ShipperOrder
        [HasCredential(RoleID = "VIEW_SHIPPER")]
        public ActionResult Index(int page = 1, int pageSize = 5)
        {
            var data = new ShipperOrderDao().GetAllPaging(page, pageSize);
            return View(data);
        }
        [HasCredential(RoleID = "ADD_SHIPPER")]
        public JsonResult CreateShipper(int userID, int orderID)
        {
            if(userID <= 0 || orderID <= 0)
            {
                return Json(new { status = false });
            }
            var order = new OrderDao().ViewOrder(orderID);
            if (!order.Status.Value)
            {
                return Json(new { status = false, mess = "Đơn chưa được duyệt" });
            }
            var orderShip = new OrderDao().CheckExistsOrderViewDetail(orderID);
            if(orderShip != null)
            {
                if (!orderShip.Status)
                {
                    return Json(new { status = false, mess = "Chưa giao xong đơn hàng khác" });
                }
            }
            var result = new ShipperOrderDao().InsertShipper(userID, orderID);
            if (result)
            {
                return Json(new { status = true, mess = "Đặt ship thành công"});
            }
            else
            {
                return Json(new { status = false, mess = "Chưa giao xong đơn hàng khác" });
            }
        }
        [HasCredential(RoleID = "EDIT_SHIPPER")]
        public JsonResult ChangeStatus(int orderID)
        {
            if(orderID <= 0)
            {
                return Json(new { status = false });
            }
            var result = new ShipperOrderDao().ChangeStatus(orderID);
            if (result)
            {
                return Json(new { status = true });
            }
            else
            {
                return Json(new { status = false });
            }
        }
        [HasCredential(RoleID = "DELETE_SHIPPER")]
        public JsonResult DeleteShipper(int orderID)
        {
            if(orderID <= 0)
            {
                return Json(new { status = false });
            }
            var data = new OrderDao().CheckExistsOrderViewDetail((long)orderID);
            if(data != null)
            {
                if (data.Status)
                {
                    return Json(new { status = false, check = false, mess = "đơn đã giao thành công, không xóa được" });
                }
            }         
            var result = new ShipperOrderDao().DeleteShipper(orderID);
            if (result)
            {
                return Json(new { status = true, check = true });
            }
            else
            {
                return Json(new { status = false, check = true });
            }
        }
    }
}