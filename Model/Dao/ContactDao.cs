using Model.EF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PagedList;

namespace Model.Dao
{
    public class ContactDao
    {
        OnlineShopDbContext db = null;
        public ContactDao()
        {
            db = new OnlineShopDbContext();
        }

        public Contact GetActiveContact()
        {
            return db.Contacts.Single(x => x.Status == true);
        }

        public int InsertFeedBack(Feedback fb)
        {
            db.Feedbacks.Add(fb);
            db.SaveChanges();
            return fb.ID;
        }
        public IEnumerable<Feedback> GetAllPaging(int page, int pageSize)
        {
            return db.Feedbacks.OrderByDescending(x => x.Name).ToPagedList(page, pageSize);
        }
        public bool Delete(int id)
        {
            var data = db.Feedbacks.Find(id);
            db.Feedbacks.Remove(data);
            db.SaveChanges();
            return true;
        }
    }
}
