using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BaggageApp.Helpers
{
    public static class StaticListTypes
    {
        public enum SteptStatus
        {
            Off = 1,
            On = 2,
            Success = 3,
            Error = 4
        }

        public enum ViewListTypes
        {
            PassengerInfo = 1,
            ContactInfo = 2,
            BaggegeInfo = 3,
            ItineraryInfo = 4,
            ClaimInfo = 5
        }

        public enum OHDViewListTypes
        {
            GeneralInfo = 1,
            PassengerInfo = 2,
            BaggegeInfo = 3,
            ItineraryInfo = 4
        }
        public enum ForwardViewType
        {
            GeneralInfo = 1,
            PassengerInfo = 2,
            BaggageInfo = 3,
            ItineraryInfo = 4,
            RushRouting = 5,
            PassengerItinerary = 6

        }
        public enum AddressListTypes
        {
            Permanent = 1,
            Temporary = 2
        }

        public enum FlightStatusTypes
        {
            TakeOffTime = 11,
            LandingTime = 12,
            GateOutTime = 13,
            GateInTime = 14,
            OnTime = 0
        }
        public enum ReportType
        {
            AHL = 1,
            FWD = 2,
            OHD = 3,
            DPR = 4,
            RED = 5
        }

        public enum DPRViewListTypes
        {
            ClaimInfo = 1,
            PassengerInfo = 2,
            BaggegeInfo = 3,
            ItineraryInfo = 4
        }

        public enum CreationMethodType
        {
            NONE = 'N',
            DIRECT = 'D',
            FLIGHTS = 'V',
            SCANNER = 'E',
            FORWARD = 'F',
            ONHAND = 'O'
        }

        public enum CreationMechanismType
        {
            FLIGHTS,
            SCANNER,
            DIRECT,
            UPDATED,
            NONE
        }

        public static string ToCreationMethodTypeString(this CreationMethodType registrationType)
        {
            // TODO: validation
            return ((char)registrationType).ToString();
        }

        public enum AccentType
        {
            OnlyLetters,
            LettersAndNumbers,
            LettersNumbersAccentsAndSpace,
            LettersNumbersAccentsAndCommonSpecialChar,
            LettersNumbersAndDot,
            LettersNumbersAndSpace,
            LettersNumbersHyphenAndSpace,
            NumbersAndAsterisk,
            LettersSpaceAndSlash,
            LettersAndSpace,
            OnlyNumbers
        };
    }
}
