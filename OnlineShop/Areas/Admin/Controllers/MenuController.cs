﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Model.Dao;
using Model.EF;

namespace OnlineShop.Areas.Admin.Controllers
{
    public class MenuController : BaseController
    {
        // GET: Admin/Menu
        [HasCredential(RoleID = "VIEW_MENU")]
        public ActionResult Index(int page = 1, int pageSize = 5)
        {
            var model = new MenuDao().GetAllPaging(page, pageSize);
            return View(model);
        }
        [HttpGet]
        [HasCredential(RoleID = "ADD_MENU")]
        public ActionResult Create()
        {
            return View(new Menu());
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        [HasCredential(RoleID = "ADD_MENU")]
        public ActionResult Create(Menu menu)
        {
            if (ModelState.IsValid)
            {
                menu.TypeID = 1;
                menu.Target = "_self";
                new MenuDao().Create(menu);
                return RedirectToAction("Index");
            }
            return View(menu);
        }
        [HttpGet]
        [HasCredential(RoleID = "EDIT_MENU")]
        public ActionResult Edit(int id)
        {
            var data = new MenuDao().ViewDetail(id);
            return View(data);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        [HasCredential(RoleID = "EDIT_MENU")]
        public ActionResult Edit(Menu menu)
        {
            if (ModelState.IsValid)
            {
                menu.TypeID = 1;
                menu.Target = "_self";
                new MenuDao().Update(menu);
                return RedirectToAction("Index");
            }
            return View(menu);
        }
        [HasCredential(RoleID = "DELETE_MENU")]
        public JsonResult Delete(int id)
        {
            if (id <= 0)
            {
                return Json(new { status = false, mess = "False" });
            }
            var data = new MenuDao().ViewDetail(id);
            if (data.Status.Value)
            {
                return Json(new { status = false, mess = "Không xóa được" });
            }
            new MenuDao().Delete(id);
            return Json(new { status = true, mess = "Xóa thành công" });
        }
        [HasCredential(RoleID = "EDIT_MENU")]
        public JsonResult ChangeStatus(int id)
        {
            if (id <= 0)
            {
                return Json(new { status = false, mess = "False" });
            }
            var result = new MenuDao().ChangeStatus(id);
            return Json(new { status = result, mess = "Thay đổi thành công" });
        }
    }
}