using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BaggageApp.Models
{
    public class BagFile
    {
        public int persistenceId { get; set; }
        public string persistenceId_string { get; set; }
        public int persistenceVersion { get; set; }
        public string persistenceVersion_string { get; set; }
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
        public string Manifest { get; set; }
        public string PNRInformation { get; set; }
    }

    public class PassengerPermanentAddress
    {
        public int persistenceId { get; set; }
        public string persistenceId_string { get; set; }
        public int persistenceVersion { get; set; }
        public string persistenceVersion_string { get; set; }
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

    public class PassengerTemporaryAddress
    {
        public int persistenceId { get; set; }
        public string persistenceId_string { get; set; }
        public int persistenceVersion { get; set; }
        public string persistenceVersion_string { get; set; }
        public string ProcessBagID { get; set; }
        public int Id { get; set; }
        public string TemporaryAddress { get; set; }
        public string City { get; set; }
        public string TState { get; set; }
        public string ZipCode { get; set; }
        public string CountryCode { get; set; }
        public int AreaCode { get; set; }
        public object ValidityDate { get; set; }
        public int Time24HR { get; set; }
        public string Phone1 { get; set; }
        public string Phone2 { get; set; }
        public string EntregaLocal { get; set; }
    }

    public class Claim
    {
        public int persistenceId { get; set; }
        public string persistenceId_string { get; set; }
        public int persistenceVersion { get; set; }
        public string persistenceVersion_string { get; set; }
        public string ProcessBagID { get; set; }
        public int Reasonforloss { get; set; }
        public string FaultStation { get; set; }
        public string SupplementalInformation { get; set; }
    }

    public class ProcessBagFiles
    {
        public int persistenceId { get; set; }
        public string persistenceId_string { get; set; }
        public int persistenceVersion { get; set; }
        public string persistenceVersion_string { get; set; }
        public string ProcessBagID { get; set; }
        public string StartDate { get; set; }
        public string InitiatorUserName { get; set; }
        public string InitiatorFullName { get; set; }
        public string InitiatorEmail { get; set; }
        public string Status { get; set; }
        public string SubStatus { get; set; }
        public string EndDate { get; set; }
        public bool CompensateDelayActivated { get; set; }
        public bool InitialSearchTimeDueFlag { get; set; }
        public bool AdvancedSearchTimeDueFlag { get; set; }
        public string ClaimStation { get; set; }
        public string FWDRelated { get; set; }
        public string OHDRelated { get; set; }
        public string DeliveryDate { get; set; }
        public string ClaimInitialTime { get; set; }
        public int BPMCaseID { get; set; }
        public string WTCaseID { get; set; }
        public string ClaimCreationTime { get; set; }
        public string WTClaimStatus { get; set; }
        public string CreationType { get; set; }
    }

    public class BagDTO
    {
        public int persistenceId { get; set; }
        public string persistenceId_string { get; set; }
        public int persistenceVersion { get; set; }
        public string persistenceVersion_string { get; set; }
        public string ProcessBagID { get; set; }
        public int BagSequential { get; set; }
        public int? BagPersistanceId { get; set; }
        public string BagPersistanceId_string { get; set; }
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
        public int? ContentsGender { get; set; }
        public string DestinationBaggageClaimCheck { get; set; }
        public bool? BagFound { get; set; }
        public string CaseRelatedID { get; set; }
        public bool? BagProcessed { get; set; }
        public bool? BagDelivered { get; set; }
        public int? CaseRelatedBagId { get; set; }
        public string BagStatus { get; set; }
        public string MatchSystemSource { get; set; }
        public string MatchFileType { get; set; }
        public object WTROHResponse { get; set; }
        public string ReceivedDate { get; set; }
        public string ConfirmedDate { get; set; }
        public string WTBagStatus { get; set; }
    }

    public class BagContentItem
    {
        public int persistenceId { get; set; }
        public string persistenceId_string { get; set; }
        public int persistenceVersion { get; set; }
        public string persistenceVersion_string { get; set; }
        public string ProcessBagID { get; set; }
        public int BagSequential { get; set; }
        public int Sequential { get; set; }
        public string Category { get; set; }
        public int CategoryWeight { get; set; }
        public string Description1 { get; set; }
        public string Description2 { get; set; }
        public int ContentPersistanceId { get; set; }
        public string ContentPersistanceId_string { get; set; }
    }

    public class FlightItem
    {
        public int persistenceId { get; set; }
        public string persistenceId_string { get; set; }
        public int persistenceVersion { get; set; }
        public string persistenceVersion_string { get; set; }
        public string ProcessBagID { get; set; }
        public int Sequential { get; set; }
        public int RoutingTypeId { get; set; }
        public string Origin { get; set; }
        public string Destination { get; set; }
        public string Airline { get; set; }
        public string Flight { get; set; }
        public string FDate { get; set; }
        public int FPersistanceId { get; set; }
        public string FPersistanceId_string { get; set; }
    }

    public class ProcessHistory
    {
        public int persistenceId { get; set; }
        public string persistenceId_string { get; set; }
        public int persistenceVersion { get; set; }
        public string persistenceVersion_string { get; set; }
        public string ProcessBagID { get; set; }
        public string ActivityName { get; set; }
        public int ActivityId { get; set; }
        public int Sequential { get; set; }
        public string LogDate { get; set; }
        public string UserName { get; set; }
        public string Comment { get; set; }
    }

    public class PXFItem
    {
        public int persistenceId { get; set; }
        public string persistenceId_string { get; set; }
        public int persistenceVersion { get; set; }
        public string persistenceVersion_string { get; set; }
        public string ProcessBagID { get; set; }
        public string Agent { get; set; }
        public string CreationDate { get; set; }
        public string RelatedCaseID { get; set; }
        public string MessageSent { get; set; }
        public string OriginTransaction { get; set; }
    }

    public class BagCost
    {
        public string ProcessBagID { get; set; }
        public int Type { get; set; }
        public string Currency { get; set; }
        public int Amount { get; set; }
        public string Remarks { get; set; }
        public string ActivityName { get; set; }
        public string UserName { get; set; }
        public string CostDate { get; set; }
    }

    public class DeliveryInfo
    {
        public int persistenceId { get; set; }
        public string persistenceId_string { get; set; }
        public string bdoLanguage { get; set; }
        public string confirmationDate { get; set; }
        public string confirmationUser { get; set; }
        public string coordinationUser { get; set; }
        public string creationDate { get; set; }
        public string deliveredBy { get; set; }
        public int deliveryBPMCaseId { get; set; }
        public string deliveryDate { get; set; }
        public int? numberOfBags { get; set; }
        public string otherPhones { get; set; }
        public string processBagId { get; set; }
        public string receivedATODate { get; set; }
        public string tagNumbers { get; set; }
        public int? weight { get; set; }
        public int? persistenceVersion { get; set; }
        public string bdodate { get; set; }
        public string bdogeneratedflag { get; set; }
        public string deliveryaddress { get; set; }
        public string otherdeliverycompany { get; set; }
    }

    public class ProcessBagTask
    {
        public int persistenceId { get; set; }
        public string persistenceId_string { get; set; }
        public int activityId { get; set; }
        public string decision { get; set; }
        public string endTime { get; set; }
        public string processBagId { get; set; }
        public string startTime { get; set; }
        public string status { get; set; }
        public int taskId { get; set; }
        public string userName { get; set; }
        public decimal? persistenceVersion { get; set; }
    }

    public class AHLDelayedBaggageDTO
    {
        public BagFile BagFile { get; set; }
        public PassengerPermanentAddress PassengerPermanentAddress { get; set; }
        public PassengerTemporaryAddress PassengerTemporaryAddress { get; set; }
        public Claim Claim { get; set; }
        public ProcessBagFiles ProcessBagFiles { get; set; }
        public List<BagDTO> Bags { get; set; }
        public List<BagContentItem> BagContents { get; set; }
        public List<FlightItem> Flights { get; set; }
        public List<ProcessHistory> ProcessHistory { get; set; }
        public List<PXFItem> PXF { get; set; }
        public List<BagCost> BagCosts { get; set; }
        public List<DeliveryInfo> DeliveryInfos { get; set; }
        public List<ProcessBagTask> ProcessBagTasks { get; set; }
    }
}
