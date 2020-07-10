using Model.EF;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PagedList;
using System.Data.Entity;

namespace Model.Dao
{
    public class CategoryDao
    {
        OnlineShopDbContext db = null;
        public CategoryDao()
        {
            db = new OnlineShopDbContext();
        }
        public IEnumerable<Category> ListAllPaging(string search, int page, int pageSize)
        {
            IEnumerable<Category> categories = null;
            categories = db.Categories;
            if (!string.IsNullOrEmpty(search))
            {
                categories.Where(x => x.Name.Contains(search));
            }
            var result = categories.OrderByDescending(x => x.CreatedDate).ToPagedList(page, pageSize);
            return result;
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
        public Category ViewById(int id)
        {
            var data = db.Categories.Find(id);
            return data;
        }
        public void Update(Category category)
        {
            db.Entry(category).State = EntityState.Modified;
            db.SaveChanges();
        }
        public bool Delete(int id)
        {
            var entity = db.Categories.Find(id);
            db.Categories.Remove(entity);
            db.SaveChanges();
            return true;
        }
        public bool ChangeStatus(int id)
        {
            var data = db.Categories.Find(id);
            data.Status = !data.Status;
            db.Entry(data).State = EntityState.Modified;
            db.SaveChanges();
            return data.Status.Value;
        }
        /// <summary>
        /// id of Category
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public int ProductWithCategory(int id)
        {
            var result = db.Categories.Join(db.Contents, a => a.ID, b => b.CategoryID, (a, b) => new { a, b }).Where(x => x.a.ID == id).Select(c => new
            {
                c.b
            }).ToList();
            return result.Count;
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
