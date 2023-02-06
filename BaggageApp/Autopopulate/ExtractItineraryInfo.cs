using BaggageApp.Helpers;
using BaggageApp.Models;
using BaggageApp.Models.BagDetails;
using BaggageApp.Models.DisplayBookingProxy;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BaggageApp.Autopopulate
{
    public static class ExtractItineraryInfo
    {

        public static List<Itinerary> GetItineraryList(BagTagContext bagTagDetail, AirReservationDetail reservationInfo)
        {
            List<Itinerary> itineraryList = new List<Itinerary>();
            if (bagTagDetail != null && bagTagDetail.Flights != null && bagTagDetail.Flights.Count > 0)
            {
                itineraryList = GetItineraryListFromBagTagDetails(bagTagDetail);
                SetFlightArrivalHour(itineraryList, reservationInfo);
            }
            return itineraryList;
        }

        public static List<Itinerary> GetItineraryListFromReservation(AirReservationDetail reservationInfo)
        {
            List<Itinerary> itineraryList = new List<Itinerary>();
            try
            {
                int index = 0;
                List<Itinerary> tempItineraryList = new List<Itinerary>();
                if (reservationInfo != null && reservationInfo.AirReservation != null &&
                    reservationInfo.AirReservation.AirItinerary != null &&
                    reservationInfo.AirReservation.AirItinerary.OriginDestinationOptions != null &&
                    reservationInfo.AirReservation.AirItinerary.OriginDestinationOptions.OriginDestinationOption != null &&
                    reservationInfo.AirReservation.AirItinerary.OriginDestinationOptions.OriginDestinationOption.Count > 0)
                {
                    foreach (OriginDestinationOption segment in reservationInfo.AirReservation.AirItinerary.OriginDestinationOptions.OriginDestinationOption)
                    {
                        if (segment.FlightSegment.FlightNumber.Contains("ARNK"))
                        {
                            continue;
                        }
                        Itinerary flightSegment = new Itinerary();
                        flightSegment.Id = index++;
                        flightSegment.Airline = segment.FlightSegment.MarketingAirline.Code;
                        flightSegment.FlightNumber = string.Format(@"{0} {1}", segment.FlightSegment.MarketingAirline.Code, segment.FlightSegment.FlightNumber);
                        flightSegment.FlightNumberOnly = segment.FlightSegment.FlightNumber;
                        flightSegment.Departure = segment.FlightSegment.DepartureAirport.LocationCode;
                        flightSegment.Arrival = segment.FlightSegment.ArrivalAirport.LocationCode;
                        flightSegment.ItineraryDate = (segment.FlightSegment.DepartureDateTime != null) ? segment.FlightSegment.DepartureDateTime.Value.ToString("MMMM dd, yyyy") : string.Empty;
                        flightSegment.ItineraryDateFull = (segment.FlightSegment.DepartureDateTime != null) ? segment.FlightSegment.DepartureDateTime.Value : DateTime.MinValue;
                        flightSegment.Route = string.Format("{0} {1}", segment.FlightSegment.DepartureAirport.LocationCode, segment.FlightSegment.ArrivalAirport.LocationCode);
                        flightSegment.FlightArrivalHour = (segment.FlightSegment.ArrivalDateTime != null) ? segment.FlightSegment.ArrivalDateTime.Value.ToString("yyyy-MM-ddTHH:mm:ss.fffZ") : string.Empty;
                        tempItineraryList.Add(flightSegment);
                    }
                    List<Itinerary> flightUsed = tempItineraryList.Where(x => x.ItineraryDateFull <= DateTime.Now).ToList();
                    if (flightUsed != null && flightUsed.Count > 0)
                    {
                        flightUsed[flightUsed.Count - 1].IsLast = true;
                        itineraryList.Clear();
                        itineraryList.AddRange(flightUsed);
                    }
                }
            }
            catch (Exception ex)
            {
                DependencyService.Get<IMessage>().LongAlert("Ocurrió un error inesperado mientras se autopopulaba el itineario.");
                var properties = new Dictionary<string, string>
                {
                    { "ExtractItineraryInfo", "GetItineraryListFromReservation" },
                };
                //Crashes.TrackError(ex, properties);
            }
            return itineraryList;
        }

        private static void SetFlightArrivalHour(List<Itinerary> itineraryList, AirReservationDetail reservationInfo)
        {
            //Buscar hora de llegada de cada vuelo en el PNR
            if (itineraryList != null && itineraryList.Count > 0 && reservationInfo != null
                && reservationInfo.AirReservation != null && reservationInfo.AirReservation.AirItinerary != null
                && reservationInfo.AirReservation.AirItinerary.OriginDestinationOptions != null
                && reservationInfo.AirReservation.AirItinerary.OriginDestinationOptions.OriginDestinationOption != null
                && reservationInfo.AirReservation.AirItinerary.OriginDestinationOptions.OriginDestinationOption.Count > 0)
            {
                List<OriginDestinationOption> flightsFromPNT = reservationInfo.AirReservation.AirItinerary.OriginDestinationOptions.OriginDestinationOption;
                foreach (var flightItem in itineraryList)
                {
                    var currentFlightItem = flightsFromPNT.Where(x => int.Parse(x.FlightSegment.FlightNumber) == int.Parse(flightItem.FlightNumberOnly)).LastOrDefault();
                    if (currentFlightItem != null)
                    {
                        var arrivalTime = (currentFlightItem.FlightSegment.ArrivalDateTime != null) ? currentFlightItem.FlightSegment.ArrivalDateTime.Value.ToString("yyyy-MM-ddTHH:mm:ss.fffZ") : string.Empty; ;
                        flightItem.FlightArrivalHour = arrivalTime;
                    }
                }
            }
        }

        private static List<Itinerary> GetItineraryListFromBagTagDetails(BagTagContext bagtagInfo)
        {
            List<Itinerary> allflightSegmentList = new List<Itinerary>();
            if (bagtagInfo != null && bagtagInfo.Flights != null && bagtagInfo.Flights.Count > 0)
            {
                int index = 1;
                List<Models.BagDetails.Flight> usedFlights = bagtagInfo.Flights.Where(x => GetDepartureDateTime(x.DateTimeDepart.Date, x.DateTimeDepart.Time) < DateTime.Now).ToList();
                int isLast = usedFlights.Count;
                foreach (var segments in usedFlights)
                {
                    Itinerary newItem = new Itinerary()
                    {
                        Airline = segments.AirlineCode,
                        Arrival = segments.ArrivalAirport,
                        Departure = segments.DepartureAirport,
                        FlightNumber = string.Format("{0} {1}", segments.AirlineCode, segments.FlightNumber),
                        FlightNumberOnly = segments.FlightNumber,
                        Id = index,
                        IsLast = (isLast == index),
                        ItineraryDate = GetDepartureDateTime(segments.DateTimeDepart.Date, segments.DateTimeDepart.Time).ToString("MMMM dd, yyyy").ToUpper(),
                        ItineraryDateFull = GetDepartureDateTime(segments.DateTimeDepart.Date, segments.DateTimeDepart.Time),
                        Route = string.Format("{0} - {1}", segments.DepartureAirport, segments.ArrivalAirport)
                    };
                    allflightSegmentList.Add(newItem);
                    index++;
                }
            }
            return allflightSegmentList;
        }

        private static DateTime GetDepartureDateTime(string date, string time)
        {
            string[] formatTime = time.Split(':');
            string newTimeString = (time != null && time.Count() > 0) ? time : "00:00:00";
            if (formatTime != null && formatTime.Count() > 0 && formatTime[0].Equals("24"))
            {
                formatTime[0] = "12";
                newTimeString = string.Join(":", formatTime);
            }
            string completeDateTime = string.Format("{0}T{1}", date, newTimeString);
            DateTime result;
            if (!DateTime.TryParse(completeDateTime, out result))
            {
                throw new FormatException("La fecha de salida del vuelo asociado a la colilla tiene un valor inválido.");
            }
            return result;
        }

        public static Itinerary GetFlightItemByText(string flightItemSelected, List<Itinerary> flightItemList)
        {
            Itinerary segmentSelected = null;
            if (!string.IsNullOrEmpty(flightItemSelected))
            {
                string[] flightParts = flightItemSelected.Split('/');
                string airline = flightParts[0].Substring(0, 2);
                string flightNumber = flightParts[0].Substring(2);
                string departureDate = flightParts[1];
                segmentSelected = flightItemList.Where(x => x.Airline == airline &&  int.Parse(x.FlightNumberOnly) == int.Parse(flightNumber)
                                        && x.ItineraryDateFull.ToString("ddMMM", CultureInfo.InvariantCulture).ToUpper() == departureDate.ToUpper()).LastOrDefault();
            }
            return segmentSelected;
        }

        public static List<string> GetFlightListText(List<Itinerary> flightItemList)
        {
            List<string> flightList = new List<string>();
            foreach (Itinerary flightItem in flightItemList)
            {
                string plainFlightText = string.Format("{0}{1}/{2}", flightItem.Airline, flightItem.FlightNumberOnly, flightItem.ItineraryDateFull.ToString("ddMMM", CultureInfo.InvariantCulture).ToUpper());
                flightList.Add(plainFlightText);
            }
            return flightList;
        }
    }

}
