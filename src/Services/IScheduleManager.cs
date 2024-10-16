using SpeedyAir.ly.src.Models;

namespace SpeedyAir.Services
{
    public interface IScheduleManager
    {
        void PrintAllFlights();           // Method to print all flights
        void ScheduleOrders(List<Order> orders);  // Method to schedule orders to flights
        void PrintAssignedOrders();       // Method to print assigned orders to flights
    }
}
