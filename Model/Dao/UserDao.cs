using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Model.EF;
using PagedList;
using System.Configuration;
using Common;
using System.Data.Entity;

namespace Model.Dao
{
    // có thể khai báo interface rồi implement các interface, override các method trong interface 
    // sử dụng được store procedure trong entity framework code first
    public class UserDao
    {
        OnlineShopDbContext db = null;
        public UserDao()
        {
            db = new OnlineShopDbContext();
        }

        public long Insert(User entity)
        {
            try
            {
                db.Users.Add(entity);
                db.SaveChanges();
                return entity.ID;
            }
            catch (Exception ex)
            {
                // logging
                throw;
            }

        }
        public long InsertForFacebook(User entity)
        {
            try
            {
                var user = db.Users.SingleOrDefault(x => x.UserName == entity.UserName);
                if (user == null)
                {
                    db.Users.Add(entity);
                    db.SaveChanges();
                    return entity.ID;
                }
                else
                {
                    return user.ID;
                }
            }
            catch (Exception ex)
            {

                throw;
            }

        }
        public bool Update(User entity)
        {
            try
            {
                using (var DB = new OnlineShopDbContext())
                {
                    //var user = DB.Users.FirstOrDefault(x => x.ID == entity.ID);
                    //user.Name = entity.Name;
                    //if (!string.IsNullOrEmpty(entity.Password))
                    //{
                    //    user.Password = entity.Password;
                    //}
                    //user.GroupID = entity.GroupID;
                    //user.Address = entity.Address;
                    //user.Email = entity.Email;
                    //user.ModifiedBy = entity.ModifiedBy;
                    //user.ModifiedDate = DateTime.Now;
                    //DB.SaveChanges();
                    //return true;
                    DB.Entry(entity).State = EntityState.Modified;
                    DB.SaveChanges();
                    return true;
                }
                
            }
            catch (Exception ex)
            {
                //logging
                return false;
            }

        }
        public IEnumerable<User> ListAllPaging(string searchString, int page, int pageSize)
        {
            try
            {
                IQueryable<User> model = db.Users;
                if (!string.IsNullOrEmpty(searchString))
                {
                    model = model.Where(x => x.UserName.Contains(searchString) || x.Name.Contains(searchString));
                }
                return model.OrderByDescending(x => x.CreatedDate).ToPagedList(page, pageSize);
            }
            catch (Exception ex)
            {

                throw;
            }
        }
        public User GetById(string userName)
        {
            try
            {
                return db.Users.SingleOrDefault(x => x.UserName == userName);
            }
            catch (Exception ex)
            {

                throw;
            }

        }
        public User ViewDetail(int id)
        {
            try
            {
                return db.Users.Find(id);
            }
            catch (Exception ex)
            {

                throw;
            }

        }
        public List<string> GetListCredential(string userName)
        {
            try
            {
                List<string> result = new List<string>();
                var user = db.Users.Single(x => x.UserName == userName);
                string[] groupId = user.GroupID.Split(',');
                foreach (var item in groupId)
                {
                    var data = (from a in db.Credentials
                                join b in db.UserGroups on a.UserGroupID equals b.ID
                                join c in db.Roles on a.RoleID equals c.ID
                                where b.ID == item
                                select new
                                {
                                    RoleID = a.RoleID,
                                    UserGroupID = a.UserGroupID
                                }).AsEnumerable().Select(x => new Credential()
                                {
                                    RoleID = x.RoleID,
                                    UserGroupID = x.UserGroupID
                                });
                    var roles = data.Select(x => x.RoleID).ToList();
                    foreach (var role in roles)
                    {
                        result.Add(role);
                    }                    
                }
                return result.Distinct().ToList();
            }
            catch (Exception ex)
            {

                throw;
            }
        }
        public int Login(string userName, string passWord, bool isLoginAdmin = false)
        {
            try
            {
                var result = db.Users.SingleOrDefault(x => x.UserName == userName);               
                if (result == null)
                {
                    return 0;
                }
                else
                {
                    if (isLoginAdmin == true && result.GroupID != null)
                    {
                        var groupId = result.GroupID.Split(',');
                        foreach (var item in groupId)
                        {
                            if (item == CommonConstants.ADMIN_GROUP || item == CommonConstants.MOD_GROUP)
                            {
                                if (result.Status == false)
                                {
                                    return -1;
                                }
                                else
                                {
                                    if (result.Password == passWord)
                                        return 1;
                                    else
                                        return -2;
                                }
                            }
                            else
                            {
                                return -3;
                            }
                        }
                        return 1;
                    }
                    else
                    {
                        if (result.Status == false)
                        {
                            return -1;
                        }
                        else
                        {
                            if (result.Password == passWord)
                                return 1;
                            else
                                return -2;
                        }
                    }
                }
            }
            catch (Exception ex)
            {

                throw;
            }
        }
        public bool ChangeStatus(long id)
        {
            try
            {
                var user = db.Users.Find(id);
                user.Status = !user.Status;
                db.SaveChanges();
                return user.Status;
            }
            catch (Exception ex)
            {

                throw;
            }
        }
        public bool Delete(int id)
        {
            try
            {
                var user = db.Users.Find(id);
                db.Users.Remove(user);
                db.SaveChanges();
                return true;
            }
            catch (Exception)
            {
                return false;
            }

        }
        public bool CheckUserName(string userName)
        {
            try
            {
                return db.Users.Count(x => x.UserName == userName) > 0;
            }
            catch (Exception ex)
            {

                throw;
            }

        }
        public bool CheckEmail(string email)
        {
            try
            {
                return db.Users.Count(x => x.Email == email) > 0;
            }
            catch (Exception ex)
            {

                throw;
            }

        }
        public int SumUser()
        {
            return db.Users.ToList().Count;
        }
    }
}
