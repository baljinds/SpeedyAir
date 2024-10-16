using System.Collections.Generic;
using Microsoft.Extensions.Logging;
using SpeedyAir.ly.src.Models;


namespace SpeedyAir.Services
{
    public class ScheduleManager : IScheduleManager
    {
        private readonly List<Flight> flights;  // Readonly list, initialized once
        private readonly Dictionary<string, List<Flight>> destinationToFlightMap;
        private readonly ILogger<ScheduleManager> _logger;

        public ScheduleManager(FlightDataService flightDataService, ILogger<ScheduleManager> logger, string flightFilePath)
        {
            _logger = logger;
            flights = flightDataService.LoadFlights(flightFilePath);
            destinationToFlightMap = new Dictionary<string, List<Flight>>();
            InitializeFlightMapping();  // Map flights based on destination
        }

        // Initialize the mapping of flights to destinations
        private void InitializeFlightMapping()
        {
            foreach (var flight in flights)
            {
                if (!destinationToFlightMap.ContainsKey(flight.ArrivalCity))
                {
                    destinationToFlightMap[flight.ArrivalCity] = new List<Flight>();
                }
                destinationToFlightMap[flight.ArrivalCity].Add(flight);
            }
        }

        // Print list of all flights
        public void PrintAllFlights()
        {
            Console.WriteLine("\nList of all flights:");
            foreach (var flight in flights)
            {
                Console.WriteLine($"Flight: {flight.FlightNumber}, Departure: {flight.DepartureCity}, Arrival: {flight.ArrivalCity}, Day: {flight.Day}");
            }
        }

        // Assign orders to flights based on destination and capacity
        public void ScheduleOrders(List<Order> orders)
        {
            if (orders == null || !orders.Any())
            {
                _logger.LogError("No orders provided for scheduling.");
                return;
            }

            foreach (var order in orders)
            {
                if (destinationToFlightMap.TryGetValue(order.Destination, out List<Flight> flightList))
                {
                    bool orderScheduled = false;

                    foreach (var flight in flightList)
                    {
                        if (flight.AddOrder(order.OrderId))
                        {
                            orderScheduled = true;
                            break;  // Stop once the order is scheduled
                        }
                    }

                    if (!orderScheduled)
                    {
                        Console.WriteLine($"Warning: Order {order.OrderId} could not be scheduled, all flights to {order.Destination} are full.");
                    }
                }
                else
                {
                    Console.WriteLine($"Warning: Order {order.OrderId} has an invalid destination.");
                }
            }
        }

        // Print orders and their assigned flights
        public void PrintAssignedOrders()
        {
            Console.WriteLine("\nOrders assigned to flights:");

            foreach (var flight in flights)
            {
                if (flight.Orders.Any())
                {
                    Console.WriteLine($"Flight {flight.FlightNumber} (from {flight.DepartureCity} to {flight.ArrivalCity} on Day {flight.Day}) has the following orders:");

                    foreach (var order in flight.Orders)
                    {
                        Console.WriteLine($"  Order: {order}");
                    }
                }
                else
                {
                    Console.WriteLine($"Flight {flight.FlightNumber} (from {flight.DepartureCity} to {flight.ArrivalCity} on Day {flight.Day}) has no assigned orders.");
                }
            }
        }

    }
}
