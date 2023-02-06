using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BaggageApp.Models
{
    public class Itinerary
    {
        public int Id { get; set; }
        public string Airline { get; set; }
        public string FlightNumber { get; set; }
        public string FlightNumberOnly { get; set; }
        public string ItineraryDate { get; set; }
        public DateTime ItineraryDateFull { get; set; }
        public string Departure { get; set; }
        public string Arrival { get; set; }
        public string Route { get; set; }
        public bool IsLast { get; set; } = false;
        public string FlightArrivalHour { get; set; }
    }
}
