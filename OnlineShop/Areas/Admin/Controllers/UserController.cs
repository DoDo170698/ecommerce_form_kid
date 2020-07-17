using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Model.EF;
using Model.Dao;
using OnlineShop.Common;
using PagedList;
using System.Net;
using System.Data.SqlClient;

namespace OnlineShop.Areas.Admin.Controllers
{
    public class UserController : BaseController
    {
        // GET: Admin/User
        [HasCredential(RoleID = "VIEW_USER")]
        public ActionResult Index(string searchString, int page = 1, int pageSize = 10)
        {
            var dao = new UserDao();
            var model = dao.ListAllPaging(searchString, page, pageSize);
            ViewBag.SearchString = searchString;
            return View(model);
        }
        [HttpGet]
        [HasCredential(RoleID = "ADD_USER")]
        public ActionResult Create()
        {
            return View();
        }
        [HasCredential(RoleID = "EDIT_USER")]
        public ActionResult Edit(int id)
        {
            var user = new UserDao().ViewDetail(id);
            return View(user);
        }
        [HttpPost]
        [HasCredential(RoleID = "ADD_USER")]
        public ActionResult Create(User user)
        {
            if (ModelState.IsValid)
            {
                var dao = new UserDao();

                var encryptedMd5Pas = Encryptor.MD5Hash(user.Password);
                user.Password = encryptedMd5Pas;

                long id = dao.Insert(user);
                if (id > 0)
                {
                    SetAlert("Thêm user thành công", "success");
                    return RedirectToAction("Index", "User");
                }
                else
                {
                    ModelState.AddModelError("", "Thêm user không thành công");
                }
            }
            return View("Index");
        }
        [HttpPost]
        [HasCredential(RoleID = "EDIT_USER")]
        public ActionResult Edit(User user)
        {
            if (ModelState.IsValid)
            {
                var dao = new UserDao();
                if (!string.IsNullOrEmpty(user.Password))
                {
                    var encryptedMd5Pas = Encryptor.MD5Hash(user.Password);
                    user.Password = encryptedMd5Pas;
                }
                var result = dao.Update(user);
                if (result)
                {
                    SetAlert("Sửa user thành công", "success");
                    return RedirectToAction("Index", "User");
                }
                else
                {
                    ModelState.AddModelError("", "Cập nhật user không thành công");
                }
            }
            return View("Index");
        }
        [HttpDelete]
        [HasCredential(RoleID = "DELETE_USER")]
        public JsonResult Delete(int id)
        {
            using (var context = new OnlineShopDbContext())
            {
                // using store in EF code first
                //SqlParameter p = new SqlParameter();
                //context.Database.ExecuteSqlCommand("", p);
                var obj = context.Users.Find(id);
                if (obj.Status == true)
                {
                    return Json(new { status = false, mess = "Active không được xóa" }, JsonRequestBehavior.AllowGet) ;
                }
                else
                {
                    new UserDao().Delete(id);
                    return Json(new { status = true, mess = "Xóa thành công" }, JsonRequestBehavior.AllowGet);
                }
            }
        }
        [HttpPost]
        [HasCredential(RoleID = "EDIT_USER")]
        public JsonResult ChangeStatus(long id)
        {
            var result = new UserDao().ChangeStatus(id);
            return Json(new
            {
                status = result
            });
        }
        // phân nhóm
        [HasCredential(RoleID = "EDIT_USER")]
        public ActionResult ChangeGroup(int id = 0)
        {
            var daoUser = new UserDao();
            var daoUserGroup = new UserGroupDao();
            var user = daoUser.ViewDetail(id);
            List<string> result = new List<string>();
            if(user.GroupID != null)
            {
                result = user.GroupID.Split(',').ToList();
            }               
            var lstUserGroupFirst = daoUserGroup.GetUserGroup();
            Dictionary<string, string> lstGroupId = new Dictionary<string, string>();
            foreach (var item in result)
            {
                lstGroupId.Add(item, item);
            }
            TempData["lstGroupId"] = result;
            foreach (var role in result)
            {
                foreach (var item in lstUserGroupFirst.ToList())
                {
                    if (role.Equals(item.ID))
                    {
                        lstUserGroupFirst.Remove(item);
                    }
                }
            }
            // show data có thể dùng ajax
            TempData["lstUserGroupFirst"] = lstUserGroupFirst;
            return View(user);
        }
        // add group
        [HasCredential(RoleID = "EDIT_USER")]
        public JsonResult AddGroup(int userId, string groupId)
        {
            if(userId == 0 || String.IsNullOrEmpty(groupId))
            {
                return Json(new { status = false });
            }
            var daoUser = new UserDao();
            var user = daoUser.ViewDetail(userId);
            var oldGroupId = user.GroupID.Split(',').ToList();
            user.GroupID = groupId;
            var updateStatus = daoUser.Update(user);
            var result = user.GroupID.Split(',').ToList();
            Dictionary<string, string> lstGroupId = new Dictionary<string, string>();
            foreach (var item in result)
            {
                foreach (var role in oldGroupId)
                {
                    if(role != item)
                    {
                        lstGroupId.Add(item, item);
                    }
                }
            }
            return Json(new { status = true, lst = lstGroupId });
        }
        // remove group
        [HasCredential(RoleID = "EDIT_USER")]
        public JsonResult RemoveGroup(int userId, string groupId)
        {
            var daoUser = new UserDao();
            var user = daoUser.ViewDetail(userId);
            var oldGroupId = user.GroupID.Split(',').ToList();
            var removeGroupId = groupId.Split(',').ToList();
            foreach (var item in removeGroupId)
            {
                foreach (var oldItem in oldGroupId.ToList())
                {
                    if (oldItem.Equals(item))
                    {
                        oldGroupId.Remove(oldItem);
                    }
                }
            }
            //user.GroupID = oldGroupId.ToString();
            user.GroupID = string.Join(",", oldGroupId);
            daoUser.Update(user);
            
            Dictionary<string, string> lstGroupId = new Dictionary<string, string>();
            foreach (var item in removeGroupId)
            {
                lstGroupId.Add(item, item);
            }
            return Json(new { status = true, lst = lstGroupId });
        }
    }
}