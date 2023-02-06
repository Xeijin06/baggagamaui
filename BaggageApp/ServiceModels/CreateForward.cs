using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BaggageApp.ServiceModels
{

    public class LogFWD
    {
        public string RequestID { get; set; }
        public int LogID { get; set; }
        public string Username { get; set; }
        public string LogDate { get; set; }
        public string Comments { get; set; }
    }
    public class CategoriaSelFWD
    {
        public string ID { get; set; }
        public string WTWeight { get; set; }
        public string DESC { get; set; }
    }
    public class ContentFWD
    {
        public int BagID { get; set; }
        public string RequestID { get; set; }
        public int ContentSequential { get; set; }
        public string Category { get; set; }
        public int CategoryWeight { get; set; }
        public string Description1 { get; set; }
        public string Description2 { get; set; }
    }
    public class ContenidoFWD
    {
        public CategoriaSelFWD CategoriaSel { get; set; }
        public string Descripcion { get; set; }
    }
    public class BagFWD
    {
        public string RequestID { get; set; }
        public int BagID { get; set; }
        public string TagNumber { get; set; }
        public string Airline { get; set; }
        public string Aerolinea { get; set; }
        public string Color { get; set; }
        public string Type { get; set; }
        public string DescriptorMaterial { get; set; }
        public string DescriptorBasic { get; set; }
        public string DescriptorExternal { get; set; }
        public string Descriptors { get; set; }
        public string BrandInformation { get; set; }
        public string BaggageWeight { get; set; }
        public string RushAirline { get; set; }
        public string RushTagNumber { get; set; }
        public int PassengerID { get; set; }
        public string RelatedFileLocatorStation { get; set; }
        public string RelatedFileLocatorAirline { get; set; }
        public string RelatedFileLocatorSeqNumber { get; set; }
        public int RelatedFileLocator { get; set; }
        public string AddressOnBag { get; set; }
        public string PhoneOnBag { get; set; }
        public string ValidacionDescriptores { get; set; }
        public string ValidacionDescriptoresMin { get; set; }
        public List<ContentFWD> Contents { get; set; }
        public string BagImageURL { get; set; }
    }

    public class PassengerFWD
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
        public string AddressOnBag { get; set; }
        public string PhoneOnBag { get; set; }
        public List<string> Addresses { get; set; }
    }



    public class FlightFWD
    {
        public string RequestID { get; set; }
        public int Sequential { get; set; }
        public int RoutingTypeId { get; set; }
        [JsonProperty(PropertyName = "origin")]
        public string Origin { get; set; }
        [JsonProperty(PropertyName = "destination")]
        public string Destination { get; set; }
        public string Airline { get; set; }
        public string Flight { get; set; }
        public string FlightDate { get; set; }
    }

    public class BagFileInputFWD
    {
        public string RequestID { get; set; }
        public int RequestType { get; set; }
        public string SupplementalInformation { get; set; }
        public string AgentName { get; set; }
        public int NumberOfBags { get; set; }
        public string Airline { get; set; }
        public string Station { get; set; }
        public int ReasonForLoss { get; set; }
        public string FaultStation { get; set; }
        public string FaultAirline { get; set; }
        public string FaultTerminal { get; set; }
        public string CommentsOnLoss { get; set; }
        public string RelatedFileLocatorStation { get; set; }
        public string RelatedFileLocatorAirline { get; set; }
        public string RelatedFileLocatorSeqNumber { get; set; }
        public string BagFileStatus { get; set; }
        public string CreationDate { get; set; }
        public List<PassengerFWD> Passengers { get; set; }
        public List<BagFWD> Bags { get; set; }
        public List<FlightFWD> Flights { get; set; }
        public List<LogFWD> Logs { get; set; }
        [JsonProperty(PropertyName = "routing")]
        public List<RushRoutingFWD> Routings { get; set; }
        [JsonProperty(PropertyName = "destinationPlatform")]
        public string DestinationPlatform { get; set; }
        public int BPMCaseID { get; set; }
    }
    public class RushRoutingFWD
    {
        [JsonProperty(PropertyName = "sequential")]
        public int Sequential { get; set; }
        [JsonProperty(PropertyName = "airline")]
        public string AirlineCode { get; set; }
        [JsonProperty(PropertyName = "station")]
        public string StationCode { get; set; }
    }
    public class CreateFWD
    {
        public BagFileInputFWD bagFileInput { get; set; }
        public string codeDestination { get; set; }
    }

}
