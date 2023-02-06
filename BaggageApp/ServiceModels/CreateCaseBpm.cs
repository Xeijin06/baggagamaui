using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BaggageApp.ServiceModels
{
    public class BoBagFileInput
    {
        public string ProcessBagID { get; set; }
        public string PNR { get; set; }
        public string PNames { get; set; }
        public string GivenName { get; set; }
        public string Initials { get; set; }
        public string Title { get; set; }
        public string ClassServiceStatus { get; set; }
        public string Status { get; set; }
        public string FrequentFlyerNumber { get; set; }
        public string LocationWhereBagLastSeen { get; set; }
        public int ContentsGender { get; set; }
        public string DestinationBaggageClaimCheck { get; set; }
        public string manifest { get; set; }
        public string pnrInformation { get; set; }
    }

    public class BoPassengerPermanentAddressInput
    {
        public string ProcessBagID { get; set; }
        public string PermanentAddress { get; set; }
        public string City { get; set; }
        public string PState { get; set; }
        public string ZipCode { get; set; }
        public string CountryCode { get; set; }
        public int AreaCode { get; set; }
        public string HomeBusinessPhone1 { get; set; }
        public string HomeBusinessPhone2 { get; set; }
        public string CellMobilePhone1 { get; set; }
        public int CellMobilePhone1AreaCode { get; set; }
        public string CellMobilePhone2 { get; set; }
        public int CellMobilePhone2AreaCode { get; set; }
        public string EmailAddress1 { get; set; }
        public int Fax1 { get; set; }
        public string PLanguage { get; set; }
    }

    public class BoPassengerTemporaryAddressInput
    {
        public string ProcessBagID { get; set; }
        public int Id { get; set; }
        public string TemporaryAddress { get; set; }
        public string City { get; set; }
        public string TState { get; set; }
        public string ZipCode { get; set; }
        public string CountryCode { get; set; }
        public int AreaCode { get; set; }
        public string ValidityDate { get; set; }
        public int Time24HR { get; set; }
        public string Phone1 { get; set; }
        public string Phone2 { get; set; }
        public string EntregaLocal { get; set; }
    }

    public class BoBagsInput
    {
        public string ProcessBagID { get; set; }
        public int BagSequential { get; set; }
        public string TagNumber { get; set; }
        public string Airline { get; set; }
        public string Color { get; set; }
        public string Type { get; set; }
        public string Descriptors { get; set; }
        public string BrandInformation { get; set; }
        public string DeliveryDate { get; set; }
        public int Time24HR { get; set; }
        public string LocalDeliveryInformation { get; set; }
        public string LocationWhereBagLastSeen { get; set; }
        public int ContentsGender { get; set; }
        public string DestinationBaggageClaimCheck { get; set; }
        public bool bagFound { get; set; }
        public string caseRelatedID { get; set; }
        public int caseRelatedBagId { get; set; }
        public string matchSystemSource { get; set; }
        public string matchFileType { get; set; }
    }

    public class BoBagContentsInput
    {
        public string ProcessBagID { get; set; }
        public string BagSequential { get; set; }
        public int Sequential { get; set; }
        public string Category { get; set; }
        public string Description1 { get; set; }
        public string Description2 { get; set; }
        public int CategoryWeight { get; set; }
    }

    public class BoFlightsInput
    {
        public string ProcessBagID { get; set; }
        public int Sequential { get; set; }
        public int RoutingTypeId { get; set; }
        public string Origin { get; set; }
        public string Destination { get; set; }
        public string Airline { get; set; }
        public string Flight { get; set; }
        public string FDate { get; set; }
        public string flightArrivalHour { get; set; }
    }

    public class BoClaimInput
    {
        public string ProcessBagID { get; set; }
        public int Reasonforloss { get; set; }
        public string FaultStation { get; set; }
        public string SupplementalInformation { get; set; }
    }
    public class BoProcessBagFilesInput
    {
        public string ProcessBagID { get; set; }
        public string InitiatorUserName { get; set; }
        public string ClaimStation { get; set; }
        public string FWDRelated { get; set; }
        public string OHDRelated { get; set; }
        public string initSearchDecision { get; set; }
        public object claimInitialTime { get; set; }
        public object claimCreationTime { get; set; }
        public string creationType { get; set; }
    }
    public class CreateCaseBpm
    {
        public BoBagFileInput boBagFileInput { get; set; }
        public BoPassengerPermanentAddressInput boPassengerPermanentAddressInput { get; set; }
        public BoPassengerTemporaryAddressInput boPassengerTemporaryAddressInput { get; set; }
        public List<BoBagsInput> boBagsInput { get; set; }
        public List<BoBagContentsInput> boBagContentsInput { get; set; }
        public List<BoFlightsInput> boFlightsInput { get; set; }
        public BoClaimInput boClaimInput { get; set; }
        public BoProcessBagFilesInput boProcessBagFilesInput { get; set; }
    }

    public class CaseCreatedBpm
    {
        public int caseId { get; set; }
    }
}
