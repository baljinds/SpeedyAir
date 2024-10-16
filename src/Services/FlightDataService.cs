using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;
using Microsoft.Extensions.Logging;
using SpeedyAir.ly.src.Models;

namespace SpeedyAir.Services
{
    public class FlightDataService
    {
        private readonly ILogger<FlightDataService> _logger;

        public FlightDataService(ILogger<FlightDataService> logger)
        {
            _logger = logger;
        }

        public List<Flight> LoadFlights(string filePath)
        {
            try
            {
                Console.WriteLine("Loading flights from {0}", filePath);
                string json = File.ReadAllText(filePath);
                var flights = JsonConvert.DeserializeObject<List<Flight>>(json);
                Console.WriteLine("Successfully loaded {0} flights.", flights.Count);
                return flights;
            }
            catch (FileNotFoundException ex)
            {
                _logger.LogError(ex, "Flight file not found: {FilePath}", filePath);
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to load flights from file: {FilePath}", filePath);
                throw;
            }
        }
    }
}
