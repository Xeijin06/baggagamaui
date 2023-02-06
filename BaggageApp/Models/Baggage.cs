using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BaggageApp.Models
{
    public class Baggage
    {

        public int Id { get; set; }
        public string BaggageTagNumber { get; set; }
        public string Carrier { get; set; }
        public string ColourType { get; set; }
        public string BaggageType { get; set; }
        public string DescriptorFirst { get; set; }
        public string DescriptorSecond { get; set; }
        public string DescriptorThird { get; set; }
        public string BrandName { get; set; }
        public string BrandId { get; set; }
        public string DestinationBagTag { get; set; }
        public string BagLastSeen { get; set; }
        public string ContentsGender { get; set; }
        public string Category { get; set; }
        public string Description { get; set; }
        public string LocalDelivery { get; set; }
        public string FullBagTag { get; set; }
        public bool BagFound { get; set; }
        public string CaseRelatedID { get; set; }
        public int CaseRelatedBagId { get; set; }
        public List<BaggageContentItem> BaggageContents { get; set; }

        public string TotalContents { get; set; }
    }
}
