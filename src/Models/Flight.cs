using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace SpeedyAir.ly.src.Models
{
    public class Flight
    {
        [JsonProperty("Flight_Number")]
        public int FlightNumber { get; set; }

        [JsonProperty("departure_city")]
        public string DepartureCity { get; set; }

        [JsonProperty("arrival_city")]
        public string ArrivalCity { get; set; }

        [JsonProperty("day")]
        public int Day { get; set; }
        public int Capacity { get; private set; } = 20; // Capacity of each flight
        public List<string> Orders { get; private set; } = new List<string>();

        // Method to add an order if there's capacity
        public bool AddOrder(string order)
        {
            if (Orders.Count < Capacity)
            {
                Orders.Add(order);
                return true;
            }
            return false;  // Flight is full
        }
    }
}
