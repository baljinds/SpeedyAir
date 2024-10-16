using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpeedyAir.ly.src.Models
{
    public class Order
    {
        public string OrderId { get; set; }  // Unique identifier for the order
        public string Destination { get; set; }  // The destination of the order (YYZ, YYC, YVR)
    }
}
