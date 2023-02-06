using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BaggageApp.Models.BagDetails
{
    public class IssueInformation
    {
        public string Date { get; set; }
        public string Time { get; set; }
    }

    public class DateTimeDepart
    {
        public string Date { get; set; }
        public string Time { get; set; }
    }

    public class Flight
    {
        public string AirlineCode { get; set; }
        public string FlightNumber { get; set; }
        public string DepartureAirport { get; set; }
        public string ArrivalAirport { get; set; }
        public DateTimeDepart DateTimeDepart { get; set; }
        public int SequenceNumber { get; set; }
        public string MessageType { get; set; }
    }

    public class BagTagContext
    {
        public int Id { get; set; }
        public string issuerairline { get; set; }
        public string bagtagId { get; set; }
        public string RecordLocatorID { get; set; }
        public string PassengerName { get; set; }
        public string TotalWeight { get; set; }
        public string WeightMeasure { get; set; }
        public string AGT { get; set; }
        public IssueInformation IssueInformation { get; set; }
        public string PrinterID { get; set; }
        public List<Flight> Flights { get; set; }
        public List<ErrorMessage> ErrorMessage { get; set; }
    }
    public class ErrorMessage
    {
        public string Description { get; set; }
    }
}
