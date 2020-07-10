using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Model.EF;
using System.Threading.Tasks;
using PagedList;

namespace Model.Dao
{
   public class SlideDao
    {
        OnlineShopDbContext db = null;
        public SlideDao()
        {
            db = new OnlineShopDbContext();
        }
        public List<Slide> ListAll()
        {
            try
            {
                var model = db.Slides.Where(x => x.Status == true).OrderBy(y => y.DisplayOrder);
                return model.ToList();
            }
            catch (Exception ex)
            {

                throw;
            }
        }
        public IEnumerable<Slide> ListAllPaging(string search, int page, int pageSize)
        {
            try
            {
                IQueryable<Slide> model = db.Slides;
                if (!String.IsNullOrEmpty(search))
                {
                    model.Where(x => x.Description.Contains(search));
                }
                var result = model.OrderByDescending(x => x.Description).ToPagedList(page, pageSize);
                return result;
            }
            catch (Exception ex)
            {

                throw;
            }           
        }
        public Slide ViewDetail(int id)
        {
            return db.Slides.Find(id);
        }
    }
}
