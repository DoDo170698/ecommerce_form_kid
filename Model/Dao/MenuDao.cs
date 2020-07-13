using Model.EF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PagedList;

namespace Model.Dao
{
    public class MenuDao
    {
        OnlineShopDbContext db = null;
        public MenuDao()
        {
            db = new OnlineShopDbContext();
        }
        /// <summary>
        /// TypeMenu == 1
        /// </summary>
        /// <param name="page"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        public IEnumerable<Menu> GetAllPaging(int page, int pageSize)
        {
            var model = db.Menus.Where(x => x.TypeID == 1).OrderByDescending(x => x.Status).ToPagedList(page, pageSize);
            return model;
        }   
        public Menu ViewDetail(int id)
        {
            return db.Menus.Find(id);
        }
        public int Create(Menu menu)
        {
            db.Menus.Add(menu);
            db.SaveChanges();
            return menu.ID;
        }
        public bool Update(Menu menu)
        {
            db.Entry(menu).State = System.Data.Entity.EntityState.Modified;
            db.SaveChanges();
            return true;
        }
        public bool Delete(int id)
        {
            var data = ViewDetail(id);
            db.Menus.Remove(data);
            db.SaveChanges();
            return true;
        }
        public bool ChangeStatus(int id)
        {
            var data = ViewDetail(id);
            data.Status = !data.Status;
            db.Entry(data).State = System.Data.Entity.EntityState.Modified;
            db.SaveChanges();
            return data.Status.Value;
        }
        public List<Menu> ListByGroupId(int groupId)
        {
            return db.Menus.Where(x => x.TypeID == groupId && x.Status==true).OrderBy(x=>x.DisplayOrder).ToList();
        }
    }
}
