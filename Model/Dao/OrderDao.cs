using System;
using System.Collections.Generic;
using System.Data.SqlTypes;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Model.EF;
using Model.ViewModel;
using PagedList;

namespace Model.Dao
{
    public class OrderDao
    {
        OnlineShopDbContext db = null;
        public OrderDao()
        {
            db = new OnlineShopDbContext();
        }
        public IEnumerable<Order> GetAllPaging(int page, int pageSize)
        {
            try
            {
                IEnumerable<Order> model = db.Orders.ToList();
                return model.OrderByDescending(x => x.CreatedDate).ToPagedList(page, pageSize);
            }
            catch (Exception ex)
            {

                throw;
            }
            
        }
        public Order ViewOrder(int orderID)
        {
            try
            {
                var data = db.Orders.Where(x => x.ID == orderID).Select(x => x).SingleOrDefault();
                return data;
            }
            catch (Exception ex)
            {

                throw;
            }
        }
        public long Insert(Order order)
        {
            try
            {
                db.Orders.Add(order);
                db.SaveChanges();
                return order.ID;
            }
            catch (Exception ex)
            {

                throw;
            }
           
        }
        public bool ChangeStatus(int id)
        {
            try
            {
                var order = db.Orders.Find(id);
                order.Status = !order.Status;
                db.SaveChanges();
                return (bool)order.Status;
            }
            catch (Exception ex)
            {

                throw;
            }
        }
        public List<OrderDetailViewModel> ViewDetail(int id)
        {
            try
            {
                var countProduct = db.OrderDetails.Join(db.Products, a => a.ProductID, b => b.ID, (a, b) => new { a, b }).Where(a => a.a.OrderID == id);
                var data = db.OrderDetails.Join(db.Products, a => a.ProductID, b => b.ID, (a, b) => new { a, b }).Where(a => a.a.OrderID == id).AsEnumerable().Select(x => new OrderDetailViewModel
                {
                    Product = x.b,
                    Quantity = x.a.Quantity.Value
                });
                return data.ToList();
            }
            catch (Exception ex)
            {

                throw;
            }
            
        }
        public bool CheckExistsOrder(long orderID)
        {
            try
            {
                var result = db.Orders.Join(db.ShipperOrders, a => a.ID, b => b.OrderID, (a, b) => new { 
                    a.ID,
                    b.OrderID
                }).Where(t => t.OrderID == orderID).ToList();
                if(result.Count == 0)
                {
                    return false;
                }
                return true;
            }
            catch (Exception ex)
            {

                throw;
            }
        }
        public ShipperOrder CheckExistsOrderViewDetail(long orderID)
        {
            try
            {
                var result = db.Orders.Join(db.ShipperOrders, a => a.ID, b => b.OrderID, (a, b) => new {                   
                    b.OrderID
                }).FirstOrDefault(t => t.OrderID == orderID);
                var data = db.ShipperOrders.FirstOrDefault(x => x.OrderID == orderID);               
                return data;
            }
            catch (Exception ex)
            {

                throw;
            }
        }
    }
}
