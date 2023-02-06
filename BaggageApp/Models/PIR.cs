using BaggageApp.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BaggageApp.Models
{
    public class PIR : ObservableObject
    {
        public int Id { get; set; }
        public string CaseId { get; set; }
        public string RecordLocator { get; set; }
        public string Name { get; set; }
        public string LastName { get; set; }
        public string FullName { get; set; }
        public string Initials { get; set; }
        public string PassengerTitle { get; set; }
        public string CabinClass { get; set; }
        public string PassengerStatus { get; set; }
        public string FrequentFlyer { get; set; }
        public string Carrier { get; set; }
        public string PermanentAddress { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string PostalCode { get; set; }
        public string Country { get; set; }
        public string CountryCode { get; set; }
        public string PhoneType1 { get; set; }
        public string PhoneType2 { get; set; }
        public string PermanentAreaCode1 { get; set; }
        public string PermanentAreaCode2 { get; set; }
        public string PermanentPhone1 { get; set; }
        public string PermanentPhone2 { get; set; }
        public string EmailAddress { get; set; }
        public string Language { get; set; }
        public string TemporaryAddress { get; set; }
        public string TemporaryCity { get; set; }
        public string TemporaryState { get; set; }
        public string TemporaryPostalCode { get; set; }
        public string TemporaryCountry { get; set; }
        public string TemporaryCountryCode { get; set; }
        public string TemporaryPhoneType { get; set; }
        public string TemporaryAreaCode { get; set; }
        public string TemporaryPhone { get; set; }
        public DateTime ValidityDateAddress { get; set; }
        public string ValidityTimeAddress { get; set; }
        public string FurtherInformation { get; set; }
        public string LocalDelivery { get; set; }
        public string Baggages { get; set; }
        public string Itinerary { get; set; }
        public string PassengerId { get; set; }
        public string BpmCaseId { get; set; }
        public string FlightId { get; set; }
        public string ReasonForLossCode { get; set; }
        public string ResponsibleStation { get; set; }
        public string AdditionalInformation { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime SavedDate { get; set; }
        public string CreatedBy { get; set; }
        public string FWDReferenceId { get; set; }
        public string OHDReferenceId { get; set; }
        public string SearchDecisionStatus { get; set; }
        public string StationCreatedBy { get; set; }
        public string PNRInformation { get; set; }
        public string Manifest { get; set; }
        public string CreationTypeOrigin { get; set; }
        public bool IsCreatedFromFWD { get; set; }
        public string FileCreationMechanism { get; set; }
    }
}
