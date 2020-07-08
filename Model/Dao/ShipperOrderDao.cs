using System;
using System.Collections.Generic;
using System.Data.Entity.Core.Metadata.Edm;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Model.EF;
using Model.ViewModel;
using PagedList;

namespace Model.Dao
{
    public class ShipperOrderDao
    {
        OnlineShopDbContext db = null;
        public ShipperOrderDao()
        {
            db = new OnlineShopDbContext();
        }
        public IEnumerable<ShiperOrderViewDetail> GetAllPaging(int page, int pageSize)
        {
            try
            {
                IEnumerable<ShipperOrder> model = db.ShipperOrders.ToList();
                var result = db.ShipperOrders.Join(db.Users, a => a.UserID, b => b.ID, (a, b) => new
                {
                    a.OrderID,
                    b.UserName,
                    b.Name,
                    b.Phone,
                    b.Email,
                    a.Status
                })
                    .Join(db.Orders, c => c.OrderID, d => d.ID, (c, d) => new ShiperOrderViewDetail()
                    {
                        OrderID = c.OrderID,
                        ShipAddress = d.ShipAddress,
                        ShipEmail = d.ShipEmail,
                        ShipName = d.ShipName,
                        ShipPhone = d.ShipMobile,
                        ShipperName = c.UserName,
                        ShipperPhone = c.Phone,
                        Status = c.Status
                    }).ToList();
                return result.OrderByDescending(x => x.Status).ToPagedList(page, pageSize);
            }
            catch (Exception ex)
            {
                
                throw;
            }            
        }
        public bool InsertShipper(int UserID, int OrderId)
        {
            try
            {
                List<ShipperOrder> valid = db.ShipperOrders.Where(x => x.UserID == UserID).ToList();
                foreach (var item in valid)
                {
                    if (!item.Status)
                    {
                        return false;
                    }
                }
                
                var data = new ShipperOrder()
                {
                    UserID = UserID,
                    OrderID = OrderId,
                    Status = false
                };
                db.ShipperOrders.Add(data);
                db.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {

                throw;
            }            
        }
        public bool DeleteShipper(int OrderID)
        {
            try
            {
                var data = db.ShipperOrders.Find(OrderID);
                db.ShipperOrders.Remove(data);
                db.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {

                throw;
            }
        }
        public bool ChangeStatus(int OrderID)
        {
            try
            {
                var result = db.ShipperOrders.FirstOrDefault(x => x.OrderID == OrderID);
                result.Status = !result.Status;
                db.SaveChanges();
                return result.Status;
            }
            catch (Exception ex)
            {

                throw;
            }
        }
    }
}
