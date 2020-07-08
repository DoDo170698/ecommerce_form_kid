using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.ViewModel
{
    public class ShiperOrderViewDetail
    {
        public int OrderID { get; set; }

        public string ShipName { get; set; }

        public string ShipPhone { get; set; }

        public string ShipAddress { get; set; }

        public string ShipEmail { get; set; }

        public string ShipperName { get; set; }

        public string ShipperPhone { get; set; }

        public bool Status { get; set; }
    }
}
