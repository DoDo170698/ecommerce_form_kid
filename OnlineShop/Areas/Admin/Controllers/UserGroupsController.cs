using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Model.Dao;
using Model.EF;

namespace OnlineShop.Areas.Admin.Controllers
{
    public class UserGroupsController : BaseController
    {
        private OnlineShopDbContext db = new OnlineShopDbContext();

        // GET: Admin/UserGroups
        public ActionResult Index(int page = 1, int pageSize = 5)
        {
            var model = new UserGroupDao().GetAllPaging(1, 5);
            return View(model);
        }

        // GET: Admin/UserGroups/Details/5
        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            UserGroup userGroup = db.UserGroups.Find(id);
            if (userGroup == null)
            {
                return HttpNotFound();
            }
            return View(userGroup);
        }

        // GET: Admin/UserGroups/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Admin/UserGroups/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,Name")] UserGroup userGroup)
        {
            if (ModelState.IsValid)
            {
                userGroup.Name.ToUpper();
                db.UserGroups.Add(userGroup);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(userGroup);
        }

        // GET: Admin/UserGroups/Edit/5
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            UserGroup userGroup = db.UserGroups.Find(id);
            if (userGroup == null)
            {
                return HttpNotFound();
            }
            return View(userGroup);
        }

        // POST: Admin/UserGroups/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,Name")] UserGroup userGroup)
        {
            if (ModelState.IsValid)
            {
                db.Entry(userGroup).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(userGroup);
        }

        // GET: Admin/UserGroups/Delete/5
        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            UserGroup userGroup = db.UserGroups.Find(id);
            if (userGroup == null)
            {
                return HttpNotFound();
            }
            return View(userGroup);
        }

        // POST: Admin/UserGroups/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            UserGroup userGroup = db.UserGroups.Find(id);
            db.UserGroups.Remove(userGroup);
            db.SaveChanges();
            return RedirectToAction("Index");
        }
        [HttpGet]
        public ActionResult AuthozationGroup(string groupId)
        {
            if(groupId == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ViewBag.GroupId = groupId;
            var credentialDao = new CredentialDao();
            var roleDao = new RoleDao();
            var model = credentialDao.GetModelRoleByGroupId(groupId);
            var listRoles = roleDao.GetAllRole();
            foreach (var role in listRoles.ToList())
            {
                foreach (var item in model)
                {
                    if (item.RoleID.Equals(role.ID))
                    {
                        listRoles.Remove(role);
                    }
                }
            }
            TempData["listRoles"] = listRoles;
            return View(model);
        }
        public JsonResult AddRole(string userGroup, List<string> RoleId)
        {
            var cretialDao = new CredentialDao();
            foreach (var item in RoleId)
            {
                cretialDao.InsertCredential(userGroup, item);
            }
            // using dictionary or list, ajax return data, client append to select

            return Json(new { status = true, lst = RoleId});
        }
        public JsonResult RemoveRole(string userGroup, List<string> RoleId)
        {
            var cretialDao = new CredentialDao();
            foreach (var item in RoleId)
            {
                cretialDao.RemoveCredential(userGroup, item);
            }
            // using dictionary or list, ajax return data, client append to select

            return Json(new { status = true, lst = RoleId });
        }
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
