using Model.EF;
using Model.ViewModel;
using PagedList;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.Dao
{
    public class ProductDao
    {
        OnlineShopDbContext db = null;
        public ProductDao()
        {
            db = new OnlineShopDbContext();
        }
        public long Insert(Product entity)
        {
            try
            {
                db.Products.Add(entity);
                db.SaveChanges();
                return entity.ID;
            }
            catch (Exception ex)
            {
                // logging
                throw;
            }

        }
        public void Update(Product product)
        {
            db.Entry(product).State = EntityState.Modified;
            db.SaveChanges();
        }
        public bool Delete(int id)
        {
            var entity = db.Products.Find(id);
            db.Products.Remove(entity);
            db.SaveChanges();
            return true;
        }
        public bool ChangeStatus(int id)
        {
            var data = db.Products.Find(id);
            data.Status = !data.Status;
            db.Entry(data).State = EntityState.Modified;
            db.SaveChanges();
            return data.Status.Value;
        }
        public List<Product> ListNewProduct(int top)
        {
            return db.Products.OrderByDescending(x => x.CreatedDate).Take(top).ToList();
        }
        public List<string> ListName(string keyword)
        {
            return db.Products.Where(x => x.Name.Contains(keyword)).Select(x => x.Name).ToList();
        }
        // using ToPageList
        public IEnumerable<Product> ListAllPaging(string searchString, int page, int pageSize)
        {
            IQueryable<Product> model = db.Products;
            if (!string.IsNullOrEmpty(searchString))
            {
                model = model.Where(x => x.Name.Contains(searchString) || x.Name.Contains(searchString));
            }

            return model.OrderByDescending(x => x.CreatedDate).ToPagedList(page, pageSize);
        }
        public IQueryable<ProductViewModel> ListAllPagingIndex(int page, int pageSize, ref int totalProduct)
        {
            totalProduct = db.Products.ToList().Count;
            var model = db.Products.Select(x => new ProductViewModel()
            {
                CateMetaTitle = x.MetaTitle,
                CateName = x.Name,
                CreatedDate = x.CreatedDate,
                ID = x.ID,
                Images = x.Image,
                Name = x.Name,
                MetaTitle = x.MetaTitle,
                Price = x.Price
            });
            return model.OrderByDescending(x => x.CreatedDate).Skip((pageSize - 1) * page).Take(pageSize);         
        }
        /// <summary>
        /// Get list product by category
        /// </summary>
        /// <param name="categoryID"></param>
        /// <returns></returns>
        public IEnumerable<ProductViewModel> ListByCategoryId(long categoryID, ref int totalRecord, int pageIndex = 1, int pageSize = 5)
        {
            // type action == type collection return, type model in view
            // cast, convert IEnumrable<T>, List<T>, IQueryable<T>
            totalRecord = db.Products.Where(x => x.CategoryID == categoryID).Count();

            // linq to sql, using viewmodel
            //IEnumerable<ProductViewModel> model = (from a in db.Products
            //             join b in db.ProductCategories
            //             on a.CategoryID equals b.ID
            //             where a.CategoryID == categoryID
            //             select new ProductViewModel()
            //             {
            //                 CateMetaTitle = b.MetaTitle,
            //                 CateName = b.Name,
            //                 CreatedDate = a.CreatedDate,
            //                 ID = a.ID,
            //                 Images = a.Image,
            //                 Name = a.Name,
            //                 MetaTitle = a.MetaTitle,
            //                 Price = a.Price
            //             });

            var model_ver = db.Products.Join(db.ProductCategories, a => a.CategoryID, b => b.ID, (a, b) => new { a, b }).Where(c => c.a.CategoryID == categoryID).AsEnumerable().Select(x => new ProductViewModel()
            {
                CateMetaTitle = x.a.MetaTitle,
                CateName = x.b.Name,
                CreatedDate = x.a.CreatedDate,
                ID = x.a.ID,
                Images = x.a.Image,
                Name = x.a.Name,
                MetaTitle = x.a.MetaTitle,
                Price = x.a.Price
            });
            
            var model = db.Products.Join(db.ProductCategories, a => a.CategoryID, b => b.ID, (a, b) => new { a, b }).Where(c => c.a.CategoryID == categoryID).Select(x => new ProductViewModel()
            {
                CateMetaTitle = x.a.MetaTitle,
                CateName = x.b.Name,
                CreatedDate = x.a.CreatedDate,
                ID = x.a.ID,
                Images = x.a.Image,
                Name = x.a.Name,
                MetaTitle = x.a.MetaTitle,
                Price = x.a.Price
            });

            //var model = db.Products.Where(x => x.CategoryID == categoryID);
            //var result = model.OrderByDescending(x => x.CreatedDate).Skip((pageIndex - 1) * pageSize).Take(pageSize).ToList();
            model = model.OrderByDescending(x => x.CreatedDate).Skip((pageIndex - 1) * pageSize).Take(pageSize);
            return model;
            //return result.ToList();
        }
        public List<ProductViewModel> Search(string keyword, ref int totalRecord, int pageIndex = 1, int pageSize = 1)
        {
            totalRecord = db.Products.Where(x => x.Name == keyword).Count();
            var model = (from a in db.Products
                         join b in db.ProductCategories
                         on a.CategoryID equals b.ID
                         where a.Name.Contains(keyword)
                         select new
                         {
                             CateMetaTitle = b.MetaTitle,
                             CateName = b.Name,
                             CreatedDate = a.CreatedDate,
                             ID = a.ID,
                             Images = a.Image,
                             Name = a.Name,
                             MetaTitle = a.MetaTitle,
                             Price = a.Price
                         }).AsEnumerable().Select(x => new ProductViewModel()
                         {
                             CateMetaTitle = x.MetaTitle,
                             CateName = x.Name,
                             CreatedDate = x.CreatedDate,
                             ID = x.ID,
                             Images = x.Images,
                             Name = x.Name,
                             MetaTitle = x.MetaTitle,
                             Price = x.Price
                         });
            var result = model.OrderByDescending(x => x.CreatedDate).Skip((pageIndex - 1) * pageSize).Take(pageSize).ToList();
            //var result = model.ToList();
            return result;
        }
        /// <summary>
        /// List feature product
        /// </summary>
        /// <param name="top"></param>
        /// <returns></returns>
        public List<Product> ListFeatureProduct(int top)
        {
            return db.Products.Where(x => x.TopHot != null && x.TopHot > DateTime.Now).OrderByDescending(x => x.CreatedDate).Take(top).ToList();
        }
        public List<Product> ListRelatedProducts(long productId)
        {
            var product = db.Products.Find(productId);
            return db.Products.Where(x => x.ID != productId && x.CategoryID == product.CategoryID).ToList();
        }
        public void UpdateImages(long productId, string images)
        {
            var product = db.Products.Find(productId);
            product.MoreImages = images;
            db.SaveChanges();
        }
        public Product ViewDetail(long id)
        {
            return db.Products.Find(id);
        }

    }
}
