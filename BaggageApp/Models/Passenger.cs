using BaggageApp.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BaggageApp.Models
{
    public class Passenger : ObservableObject
    {

        public int Id { get; set; }
        public string Title { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string FullName { get; set; }
        public string Gender { get; set; }
        public string FrequentFlyerNumber { get; set; }
        public string FrequentFlyerProgramId { get; set; }
        public string FrequentFlyerNumberText
        {
            get
            {
                return GetFrequentFlyerNumber();
            }
        }

        private string GetFrequentFlyerNumber()
        {
            string frequentFlyerText = string.Format("{0}{1}", FrequentFlyerProgramId, FrequentFlyerNumber).Replace("FF: ", "");
            return (!string.IsNullOrEmpty(frequentFlyerText)) ? frequentFlyerText.Insert(0, "FFN: ") : string.Empty;
        }

        public string LoyaltyLevel { get; set; }
        public string Class { get; set; }
        public string RecordLocator { get; set; }
        public bool HasForward { get; set; }
        public bool IsOnHand { get; set; }
        public bool HasBaggageClaim { get; set; }
        public string LoyaltyLevelSub1 { get; set; }
        public string LoyaltyLevelSub2 { get; set; }
        public bool LoyaltyLevelSub1Visible { get; set; }
        public bool LoyaltyLevelVisible { get; set; }
        public string LoyaltyStatusColor { get; set; }
        public string FlightId { get; set; }
        public string BagTagList { get; set; }
        public string PirId { get; set; }
        public string FlightWhereTraveled { get; set; }
        public string Manifest { get; set; }
        public string PNRInformation { get; set; }
    }
}
