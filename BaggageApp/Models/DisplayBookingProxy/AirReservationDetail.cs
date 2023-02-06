using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BaggageApp.Models.DisplayBookingProxy
{
    public class Error
    {
        public string Type { get; set; }
        public string ShortText { get; set; }
        public string Text { get; set; }
    }

    public class Errors
    {
        public Error Error { get; set; }
    }

    public class Warning
    {
        public string Type { get; set; }
        public string ShortText { get; set; }
        public string Text { get; set; }
    }

    public class Warnings
    {
        public Warning Warning { get; set; }
    }

    public class DepartureAirport
    {
        public string LocationCode { get; set; }
    }

    public class ArrivalAirport
    {
        public string LocationCode { get; set; }
    }

    public class MarketingAirline
    {
        public string Code { get; set; }
    }

    public class FlightSegment
    {
        public DepartureAirport DepartureAirport { get; set; }
        public ArrivalAirport ArrivalAirport { get; set; }
        public MarketingAirline MarketingAirline { get; set; }
        public string MarriageGrp { get; set; }
        public string ResBookDesigCode { get; set; }
        public string NumberInParty { get; set; }
        public string Status { get; set; }
        public string E_TicketEligibility { get; set; }
        public string DepartureDay { get; set; }
        public string FlightNumber { get; set; }
        public DateTime? DepartureDateTime { get; set; }
        public DateTime? ArrivalDateTime { get; set; }
        public string RPH { get; set; }
    }

    public class OriginDestinationOption
    {
        public FlightSegment FlightSegment { get; set; }
    }

    public class OriginDestinationOptions
    {
        public List<OriginDestinationOption> OriginDestinationOption { get; set; }
    }

    public class AirItinerary
    {
        public OriginDestinationOptions OriginDestinationOptions { get; set; }
    }

    public class PassengerTypeQuantity
    {
        public string Code { get; set; }
        public string Quantity { get; set; }
        public object Age { get; set; }
    }

    public class FareBasisCode
    {
        public string FlightSegmentRPH { get; set; }
        public string Text { get; set; }
    }

    public class FareBasisCodes
    {
        public List<FareBasisCode> FareBasisCode { get; set; }
    }

    public class BaseFare
    {
        public string Amount { get; set; }
        public string CurrencyCode { get; set; }
    }

    public class Tax
    {
        public string TaxCode { get; set; }
        public string Amount { get; set; }
    }

    public class Taxes
    {
        public List<Tax> Tax { get; set; }
    }

    public class TotalFare
    {
        public string Amount { get; set; }
        public string CurrencyCode { get; set; }
    }

    public class FareBaggageAllowance
    {
        public string FlightSegmentRPH { get; set; }
        public string UnitOfMeasureQuantity { get; set; }
        public string UnitOfMeasure { get; set; }
    }

    public class PassengerFare
    {
        public BaseFare BaseFare { get; set; }
        public Taxes Taxes { get; set; }
        public TotalFare TotalFare { get; set; }
        public string UnstructuredFareCalc { get; set; }
        public List<FareBaggageAllowance> FareBaggageAllowance { get; set; }
    }

    public class TravelerRefNumber
    {
        public string RPH { get; set; }
        public string SurnameRefNumber { get; set; }
    }

    public class Endorsements
    {
        public List<string> Endorsement { get; set; }
    }

    public class Date
    {
        public string _Date { get; set; }
        public string Type { get; set; }
    }

    public class FareInfoDate
    {
        public Date Date { get; set; }
    }

    public class FareInfo
    {
        public object Date { get; set; }
        public string DepartureAirport2 { get; set; }
        public string ArrivalAirport2 { get; set; }
        public FareInfoDate FareInfoDate { get; set; }
        public string PassengerFare { get; set; }
    }

    public class PTCFareBreakdown
    {
        public PassengerTypeQuantity PassengerTypeQuantity { get; set; }
        public FareBasisCodes FareBasisCodes { get; set; }
        public PassengerFare PassengerFare { get; set; }
        public TravelerRefNumber TravelerRefNumber { get; set; }
        public Endorsements Endorsements { get; set; }
        public FareInfo FareInfo { get; set; }
        public string FareQuoteRefNumber { get; set; }
    }

    public class PTCFareBreakdowns
    {
        public List<PTCFareBreakdown> PTC_FareBreakdown { get; set; }
    }

    public class PriceRequestInformation
    {
        public string NegotiatedFareCode { get; set; }
        public string PricingSource { get; set; }
    }

    public class PriceInfo
    {
        public PTCFareBreakdowns PTC_FareBreakdowns { get; set; }
        public PriceRequestInformation PriceRequestInformation { get; set; }
    }

    public class PersonName
    {
        public string GivenName { get; set; }
        public string Surname { get; set; }
    }

    public class Telephone
    {
        public string LocationCode { get; set; }
        public string PhoneUseType { get; set; }
        public string AreaCityCode { get; set; }
        public string PhoneNumber { get; set; }
        public string Remark { get; set; }
    }

    public class PassengerTypeQuantity2
    {
        public string Code { get; set; }
        public string Quantity { get; set; }
        public string Age { get; set; }
    }

    public class TravelerRefNumber2
    {
        public string RPH { get; set; }
        public string SurnameRefNumber { get; set; }
    }

    public class AirTraveler
    {
        public PersonName PersonName { get; set; }
        public Telephone Telephone { get; set; }
        public PassengerTypeQuantity2 PassengerTypeQuantity { get; set; }
        public TravelerRefNumber2 TravelerRefNumber { get; set; }
        public string Comment { get; set; }
        public string PassengerTypeCode { get; set; }
    }

    public class DepartureAirport2
    {
        public string LocationCode { get; set; }
    }

    public class ArrivalAirport2
    {
        public string LocationCode { get; set; }
    }

    public class SeatRequest
    {
        public DepartureAirport2 DepartureAirport2 { get; set; }
        public ArrivalAirport2 ArrivalAirport2 { get; set; }
        public string TravelerRefNumberRPHList { get; set; }
        public string FlightRefNumberRPHList { get; set; }
        public DateTime DepartureDate { get; set; }
        public string SeatNumber { get; set; }
    }

    public class SeatRequests
    {
        public List<SeatRequest> SeatRequest { get; set; }
    }

    public class Airline
    {
        public string Code { get; set; }
    }

    public class SpecialServiceRequest
    {
        public Airline Airline { get; set; }
        public string Text { get; set; }
        public string SSRCode { get; set; }
        public string ServiceQuantity { get; set; }
        public string Status { get; set; }
        public string TravelerRefNumberRPHList { get; set; }
        public string FlightRefNumberRPHList { get; set; }
    }

    public class SpecialServiceRequests
    {
        public List<SpecialServiceRequest> SpecialServiceRequest { get; set; }
    }


    public class OtherServiceInformation
    {
        public Airline Airline { get; set; }
        public string Text { get; set; }
    }

    public class OtherServiceInformations
    {
        public List<OtherServiceInformation> OtherServiceInformation { get; set; }
    }

    public class Remarks
    {
        public List<string> Remark { get; set; }
    }

    public class SpecialReqDetails
    {
        public SeatRequests SeatRequests { get; set; }
        public SpecialServiceRequests SpecialServiceRequests { get; set; }
        public OtherServiceInformations OtherServiceInformations { get; set; }
        public Remarks Remarks { get; set; }
    }

    public class TravelerInfo
    {
        public List<AirTraveler> AirTraveler { get; set; }
        public SpecialReqDetails SpecialReqDetails { get; set; }
    }

    public class Ticketing
    {
        public string TicketAdvisory { get; set; }
        public string TicketingStatus { get; set; }
        public string TicketType { get; set; }
    }

    public class BookingReferenceID
    {
        public string Type { get; set; }
        public string ID { get; set; }
    }

    public class AirReservation
    {
        public AirItinerary AirItinerary { get; set; }
        public PriceInfo PriceInfo { get; set; }
        public TravelerInfo TravelerInfo { get; set; }
        public List<Ticketing> Ticketing { get; set; }
        public List<BookingReferenceID> BookingReferenceID { get; set; }
        public DateTime CreateDateTime { get; set; }
        public DateTime LastModified { get; set; }
    }

    public class AirReservationDetail
    {
        public int Id { get; set; }
        public Errors Errors { get; set; }
        public string Success { get; set; }
        public Warnings Warnings { get; set; }
        public AirReservation AirReservation { get; set; }
        public DateTime TimeStamp { get; set; }
        public string Version { get; set; }
    }

}
