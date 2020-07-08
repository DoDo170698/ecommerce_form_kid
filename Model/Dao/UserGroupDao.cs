using Model.EF;
using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace Model.Dao
{
    public class UserGroupDao
    {
        OnlineShopDbContext db = null;
        public UserGroupDao()
        {
            db = new OnlineShopDbContext();
        }
        public IEnumerable<UserGroup> GetAllPaging(int page, int pageSize)
        {
            try
            {
                IEnumerable<UserGroup> data = db.UserGroups.ToList();
                return data.OrderByDescending(x => x.ID).ToPagedList(page, pageSize);
            }
            catch (Exception ex)
            {

                throw;
            }           
        }
        public List<UserGroup> GetUserGroup()
        {
            try
            {
                return db.UserGroups.ToList();
            }
            catch (Exception ex)
            {

                throw;
            }
           
        }
        public List<string> GetRoleByGroupId(string groupId)
        {
            try
            {
                List<string> lst = db.UserGroups.Where(x => x.Name.Equals(groupId)).Select(y => y.ID).ToList();
                return lst;
            }
            catch (Exception ex)
            {

                throw;
            }        
        }
        public List<UserGroup> GetModelRoleByGroupId(string groupId)
        {
            try
            {
                List<UserGroup> lst = db.UserGroups.Where(x => x.Name.Equals(groupId)).ToList();
                return lst;
            }
            catch (Exception ex)
            {

                throw;
            }
            
        }
    }
}
