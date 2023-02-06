using BaggageApp.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BaggageApp.Models
{
    public class OnHand : ObservableObject
    {
        public int Id { get; set; }
        public string CaseId { get; set; }
        public string BpmCaseId { get; set; }
        public string Station { get; set; }
        public string Airline { get; set; }
        public string ComplementaryInfo { get; set; }
        public string AgentInitials { get; set; }
        public string BaggageCount { get; set; }
        public string BaggageWight { get; set; }
        public string Name { get; set; }
        public string LastName { get; set; }
        public string Initials { get; set; }
        public string PassengerTitle { get; set; }
        public string FrequentFlyer { get; set; }
        public string FFCarrier { get; set; }
        public string RecordLocator { get; set; }
        public string EmailAddress { get; set; }
        public string BagAddress { get; set; }
        public string BagPhone { get; set; }
        public string Baggages { get; set; }
        public string Itinerary { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime SavedDate { get; set; }
        public string CreatedBy { get; set; }
    }
}
