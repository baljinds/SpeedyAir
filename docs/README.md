# SpeedyAir Flight Scheduling System

## Overview

This project is a flight scheduling system for SpeedyAir that processes flight and order data. It assigns orders to flights based on destination and available flight capacity. The system is built in C# using .NET Core.

## Prerequisites

- .NET Core SDK
- Newtonsoft.Json (for JSON parsing)

## Configuration

The application uses `appsettings.json` for configuration. The file should be located in the `config/` directory and contain paths to the flight and order data files.

Example `appsettings.json`:

```json
{
  "DataPaths": {
    "FlightDataPath": "data/flights.json",
    "OrderDataPath": "data/orders.json"
  }
}
