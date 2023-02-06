using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BaggageApp.ServiceModels
{
    public class DprAddress
    {
        public string RequestID { get; set; }
        public int PassengerID { get; set; }
        public int AddressType { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
        public string AddressState { get; set; }
        public string ZipCode { get; set; }
        public string CountryCode { get; set; }
        public int AreaCode { get; set; }
        public int TypePhone1 { get; set; }
        public string Phone1 { get; set; }
        public int TypePhone2 { get; set; }
        public string Phone2 { get; set; }
        public string ValidityDate { get; set; }
        public string time24hrs { get; set; }
    }

    public class DprPassenger
    {
        public string RequestID { get; set; }
        public int PassengerID { get; set; }
        public string PNR { get; set; }
        public string PNames { get; set; }
        public string GivenName { get; set; }
        public string Initials { get; set; }
        public string Title { get; set; }
        public string FrequentFlyerNumber { get; set; }
        public string EmailAddress { get; set; }
        public string Status { get; set; }
        public List<DprAddress> dprAddresses { get; set; }
    }

    public class DprBag
    {
        public string RequestID { get; set; }
        public int BagID { get; set; }
        public string TagNumber { get; set; }
        public string Airline { get; set; }
        public string Color { get; set; }
        public string Type { get; set; }
        public string Descriptors { get; set; }
        public string BrandInformation { get; set; }
        public string BaggageWeight { get; set; }
        public string BagStatus { get; set; }
        public string DamageLocation { get; set; }
        public int PassengerID { get; set; }
        public string DamageSeverity { get; set; }
        public string DamageCode { get; set; }
        public string WeightInDelivery { get; set; }
        public string BagImageURL { get; set; }
        public int ahlBagSequential { get; set; }
    }

    public class DprFlight
    {
        public string RequestID { get; set; }
        public int Sequential { get; set; }
        public int RoutingTypeId { get; set; }
        public string Origin { get; set; }
        public string Destination { get; set; }
        public string Airline { get; set; }
        public string Flight { get; set; }
        public string FlightDate { get; set; }
        public string flightArrivalHour { get; set; }
    }

    public class DprLog
    {
        public string RequestID { get; set; }
        public int ActivityID { get; set; }
        public string ActivityName { get; set; }
        public int LogID { get; set; }
        public string Username { get; set; }
        public object LogDate { get; set; }
        public string Comments { get; set; }
    }

    public class DprBagFileInput
    {
        public string RequestID { get; set; }
        public int RequestType { get; set; }
        public string SupplementalInformation { get; set; }
        public string Airline { get; set; }
        public string Station { get; set; }
        public int ReasonForLoss { get; set; }
        public string FaultStation { get; set; }
        public string FaultTerminal { get; set; }
        public string CommentsOnLoss { get; set; }
        public string BagFileStatus { get; set; }
        public string CreationDate { get; set; }
        public DateTime ClaimInitialTime { get; set; }
        public DateTime ClaimCreationTime { get; set; }
        public object EndDate { get; set; }
        public int BPMCaseID { get; set; }
        public string WTID { get; set; }
        public string InitiatorUserName { get; set; }
        public string InitiatorFullName { get; set; }
        public string InitiatorEmail { get; set; }
        public string Affectation { get; set; }
        public int ClaimAmount { get; set; }
        public string LiabilityTag { get; set; }
        public int CostPayment { get; set; }
        public string PilferedContents { get; set; }
        public string DamagedContents { get; set; }
        public List<DprPassenger> dprPassengers { get; set; }
        public List<DprBag> dprBags { get; set; }
        public List<DprFlight> dprFlights { get; set; }
        public List<DprLog> dprLogs { get; set; }
        public string manifest { get; set; }
        public string pnrInformation { get; set; }
        public string ahlid { get; set; }
        public string creationType { get; set; }
    }

    public class CreateDPR
    {
        public DprBagFileInput boBagFileInput { get; set; }
    }
}
