using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Model.EF;

namespace Model.Dao
{
    public class CredentialDao
    {
        OnlineShopDbContext db = null;
        public CredentialDao()
        {
            db = new OnlineShopDbContext();
        }       
        public List<string> GetRoleByGroupId(string groupId)
        {
            List<string> lst = db.Credentials.Where(x => x.UserGroupID.Equals(groupId)).Select(y => y.RoleID).ToList();
            return lst;
        }
        public List<Credential> GetModelRoleByGroupId(string groupId)
        {
            List<Credential> lst = db.Credentials.Where(x => x.UserGroupID.Equals(groupId)).ToList();
            return lst;
        }
        public bool InsertCredential(string userGroup, string roles)
        {
            db.Credentials.Add(new Credential
            {
                UserGroupID = userGroup,
                RoleID = roles
            });
            db.SaveChanges();
            return true;
        }
        public bool RemoveCredential(string userGroup, string roles)
        {
            db.Credentials.Remove(new Credential
            {
                UserGroupID = userGroup,
                RoleID = roles
            });
            db.SaveChanges();
            return true;
        }
    }
}
