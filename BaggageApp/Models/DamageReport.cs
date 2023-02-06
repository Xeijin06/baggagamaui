using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BaggageApp.Models
{
    public class DamageReport
    {
        public int Id { get; set; }
        public string CaseId { get; set; }
        public string BpmCaseId { get; set; }
        public string Station { get; set; }
        public string Airline { get; set; }
        public string ClaimType { get; set; }
        public string AffectationType { get; set; }
        public string FaultStation { get; set; }
        public string FaultTerminal { get; set; }
        public string ReasonForLoss { get; set; }
        public string ClaimAmount { get; set; }
        public string LiabilityTag { get; set; }
        public string DamagedContents { get; set; }
        public string PilferedContents { get; set; }
        public string ComplementaryInfo { get; set; }
        public string AgentInitials { get; set; }
        public string BaggageCount { get; set; }
        public string BaggageWeight { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Initials { get; set; }
        public string PassengerTitle { get; set; }
        public string FrequentFlyer { get; set; }
        public string RecordLocator { get; set; }
        public string EmailAddress { get; set; }
        public string LoyaltyLevel { get; set; }
        public string PermanentAddress { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string PostalCode { get; set; }
        public string Country { get; set; }
        public string PhoneType1 { get; set; }
        public string PhoneType2 { get; set; }
        public string PermanentAreaCode1 { get; set; }
        public string PermanentAreaCode2 { get; set; }
        public string PermanentPhone1 { get; set; }
        public string PermanentPhone2 { get; set; }
        public string TemporaryAddress { get; set; }
        public string TemporaryCity { get; set; }
        public string TemporaryState { get; set; }
        public string TemporaryPostalCode { get; set; }
        public string TemporaryCountry { get; set; }
        public string TemporaryPhoneType { get; set; }
        public string TemporaryAreaCode { get; set; }
        public string TemporaryPhone { get; set; }
        public DateTime ValidityDateAddress { get; set; }
        public TimeSpan ValidityTimeAddress { get; set; }
        public string FurtherInformation { get; set; }

        public string BagAddress { get; set; }
        public string BagPhone { get; set; }
        public string Baggages { get; set; }
        public string Itinerary { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime FinishedDate { get; set; }
        public string FullName
        {
            get
            {
                return GetPassengerFullName();
            }
        }

        private string GetPassengerFullName()
        {
            return string.Format("{0} {1}", LastName, FirstName);
        }
        public string CreatedBy { get; set; }
        public string Manifest { get; set; }
        public string PNRInformation { get; set; }
        public string CreationType { get; set; }

    }
}
