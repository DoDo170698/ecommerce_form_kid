using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Model.Dao;
using Model.EF;

namespace OnlineShop.Areas.Admin.Controllers
{
    public class ContactController : BaseController
    {
        // GET: Admin/Contact
        [HasCredential(RoleID = "VIEW_CONTACT")]
        public ActionResult Index(int page = 1, int pageSize = 5)
        {
            var model = new ContactDao().GetAllPaging(page, pageSize);
            return View(model);
        }
        [HasCredential(RoleID = "DELETE_CONTACT")]
        public JsonResult Delete(int id)
        {
            if(id <= 0)
            {
                return Json(new { status = false, mess = "Không xóa được" });
            }
            new ContactDao().Delete(id);
            return Json(new { status = true, mess = "Xoá thành công" });
        }
    }
}