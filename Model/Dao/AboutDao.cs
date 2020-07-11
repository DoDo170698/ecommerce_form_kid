using Common;
using Model.EF;
using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.Dao
{
    public class AboutDao
    {
        OnlineShopDbContext db = null;
        public static string USER_SESSION = "USER_SESSION";
        public AboutDao()
        {
            db = new OnlineShopDbContext();
        }

        public IEnumerable<About> ListAllPaging(string searchString, int page, int pageSize)
        {
            IQueryable<About> model = db.Abouts;
            if (!string.IsNullOrEmpty(searchString))
            {
                model = model.Where(x => x.Name.Contains(searchString) || x.Name.Contains(searchString));
            }

            return model.OrderByDescending(x => x.CreatedDate).ToPagedList(page, pageSize);
        }
        /// <summary>
        /// List all About for client
        /// </summary>
        /// <param name="page"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        public IEnumerable<About> ListAllPaging(int page, int pageSize)
        {
            IQueryable<About> model = db.Abouts;
            return model.OrderByDescending(x => x.CreatedDate).ToPagedList(page, pageSize);
        }


        public About GetByID(long id)
        {
            return db.Abouts.Find(id);
        }

        public long Create(About about)
        {
            //Xử lý alias
            //if (string.IsNullOrEmpty(about.MetaTitle))
            //{
            //    about.MetaTitle = StringHelper.ToUnsignString(about.Name);
            //}
            about.CreatedDate = DateTime.Now;
            db.Abouts.Add(about);
            db.SaveChanges();


            return about.ID;
        }
        public long Edit(About about)
        {
            //Xử lý alias
            //if (string.IsNullOrEmpty(about.MetaTitle))
            //{
            //    about.MetaTitle = StringHelper.ToUnsignString(about.Name);
            //}
            about.CreatedDate = DateTime.Now;
            db.SaveChanges();

            return about.ID;
        }
    }
}
