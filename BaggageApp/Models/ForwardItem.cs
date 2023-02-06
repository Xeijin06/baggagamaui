using BaggageApp.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BaggageApp.Models
{
    public class ForwardItem : ObservableObject
    {
        public int Id { get; set; }
        public string CaseId { get; set; }
        public string BpmCaseId { get; set; }
        public string AirlineCode { get; set; }
        public string StationCode { get; set; }
        public string ReasonForLostCode { get; set; }
        public string ResponsibleAirlineCode { get; set; }
        public string ResponsibleStationCode { get; set; }
        public string RelatedFileLocatorSequenceNumber { get; set; }
        public string RelatedFileLocatorAirline { get; set; }
        public string RelatedFileLocatorStation { get; set; }

        public string AgentLastName { get; set; }
        public string ReasonLostComment { get; set; }
        public string FurtherInformation { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public string StationCreatedBy { get; set; }
        public DateTime SaveDate { get; set; }
        public string Baggages { get; set; }
        public string Itinerary { get; set; }
        public string ItineraryPassenger { get; set; }
        public string Passengers { get; set; }
        public string RushRoutings { get; set; }
    }
}
