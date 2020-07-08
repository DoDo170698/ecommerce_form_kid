using Model.EF;
using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.Dao
{
    public class RoleDao
    {
        OnlineShopDbContext db = null;
        public RoleDao()
        {
            db = new OnlineShopDbContext();
        }

        public List<Role> GetAllRole()
        {
            try
            {
                var listRole = db.Roles.ToList();
                return listRole;
            }
            catch (Exception ex)
            {

                throw;
            }           
        }

        public IEnumerable<Role> GetAllPaging(int page, int pageSize)
        {
            try
            {
                IQueryable<Role> listRole = db.Roles;
                return listRole.OrderByDescending(x => x.ID).ToPagedList(page, pageSize);
            }
            catch (Exception ex)
            {

                throw;
            }          
        }
    }
}
