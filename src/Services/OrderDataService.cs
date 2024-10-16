using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;
using Microsoft.Extensions.Logging;
using SpeedyAir.ly.src.Models;

namespace SpeedyAir.Services
{
    public class OrderDataService
    {
        private readonly ILogger<OrderDataService> _logger;

        public OrderDataService(ILogger<OrderDataService> logger)
        {
            _logger = logger;
        }

        public List<Order> LoadOrders(string filePath)
        {
            try
            {
                Console.WriteLine("\nLoading orders from {0}", filePath);
                string json = File.ReadAllText(filePath);
                var ordersDictionary = JsonConvert.DeserializeObject<Dictionary<string, OrderData>>(json);

                List<Order> orders = new List<Order>();
                foreach (var order in ordersDictionary)
                {
                    orders.Add(new Order
                    {
                        OrderId = order.Key,
                        Destination = order.Value.Destination
                    });
                }
                Console.WriteLine("Successfully loaded {0} orders.", orders.Count);
                return orders;
            }
            catch (FileNotFoundException ex)
            {
                _logger.LogError(ex, "Order file not found: {FilePath}", filePath);
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to load orders from file: {FilePath}", filePath);
                throw;
            }
        }
    }

    public class OrderData
    {
        public string Destination { get; set; }
    }
}
