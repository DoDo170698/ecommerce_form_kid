using Model.EF;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.Dao
{
    public class CategoryDao
    {
        OnlineShopDbContext db = null;
        public CategoryDao()
        {
            db = new OnlineShopDbContext();
        }
        public IEnumerable<Category> check()
        {
            IQueryable<Category> a = db.Categories;
            IEnumerable<Category> b = a.Where(x => x.Name.StartsWith("T")).ToList<Category>();
            return b;
        }
        public long Insert(Category category)
        {
            db.Categories.Add(category);
            db.SaveChanges();
            return category.ID;
        }
        public List<Category> ListAll()
        {
            return db.Categories.Where(x => x.Status == true).ToList();
        }
        public ProductCategory ViewDetail(long id)
        {
            return db.ProductCategories.Find(id);
        }
        // ussing store
        public int Create(string name, string alias, int parentID, int order, bool status)
        {
            object[] parameters =
            {
                new SqlParameter("@Name", name),
                new SqlParameter("@Alias", alias),
                new SqlParameter("@ParentID", parentID),
                new SqlParameter("@Order", order),
                new SqlParameter("@Status", status)
            };
            int result = db.Database.ExecuteSqlCommand("Sp_Category_Insert", parameters);
            return result;
        }
    }
}
