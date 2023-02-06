using BaggageApp.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BaggageApp.Models
{
    public class Flight : ObservableObject
    {
        public int Id { get; set; }
        public string Number { get; set; }
        public string Airline { get; set; }
        public DateTime Departure { get; set; }
        public string DepartureDate { get; set; }
        public string DepartureTime { get; set; }
        public string Origin { get; set; }
        public DateTime Arrival { get; set; }
        public string ArrivalDate { get; set; }
        public string ArrivalTime { get; set; }
        public string Destination { get; set; }
        public string Time { get; set; }
        public string Status { get; set; }
        public string AirlineFlight { get; set; }
        public string FlightId { get; set; }
    }
}
