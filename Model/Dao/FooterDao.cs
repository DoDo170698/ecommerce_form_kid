using Model.EF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PagedList;
using System.Data.Entity;

namespace Model.Dao
{
    public class FooterDao
    {
        OnlineShopDbContext db = null;
        public FooterDao()
        {
            db = new OnlineShopDbContext();
        }
        public IEnumerable<Footer> GetAllPaging(int page, int pageSize)
        {
            var model = db.Footers.OrderByDescending(x => x.ID).ToPagedList(page, pageSize);
            return model;
        }
        public Footer ViewByID(string ID)
        {
            var model = db.Footers.Find(ID);
            return model;
        }
        public bool Create(Footer footer)
        {
            db.Footers.Add(footer);
            db.SaveChanges();
            return true;
        }
        public bool Update(Footer footer)
        {
            db.Entry(footer).State = EntityState.Modified;
            db.SaveChanges();
            return true;
        }
        public bool Delete(string id)
        {
            var data = this.ViewByID(id);
            db.Footers.Remove(data);
            db.SaveChanges();
            return true;
        }
        public bool ChangeStatus(string id)
        {
            var data = ViewByID(id);
            data.Status = !data.Status;
            db.Entry(data).State = EntityState.Modified;
            db.SaveChanges();
            return data.Status.Value;
        }
        public Footer GetFooter()
        {
            return db.Footers.FirstOrDefault(x => x.Status == true && x.ID != "footer_2");
        }
        public Footer GetFooterHome()
        {
            return db.Footers.FirstOrDefault(x => x.Status == true && x.ID == "footer_2");
        }
    }
}
