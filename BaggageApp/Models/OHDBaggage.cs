using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BaggageApp.Models
{
    public class OHDBaggage
    {
        public string BaggageNumber { get; set; }
        public string BaggageTagNumber { get; set; }
        public string Carrier { get; set; }
        public string ColourType { get; set; }
        public string BaggageType { get; set; }
        public string DescriptorFirst { get; set; }
        public string DescriptorSecond { get; set; }
        public string DescriptorThird { get; set; }
        public string BrandName { get; set; }
        public string BrandId { get; set; }
        public string BaggageWeight { get; set; }
        public string Category { get; set; }
        public string Description { get; set; }
        public string FullBagTag { get; set; }
        public List<BaggageContentItem> BaggageContents { get; set; }
        public string TotalContents { get; set; }
        public List<BaggagePhotoItem> BaggagePhotos { get; set; }
        public string TotalPhotos { get; set; }
    }
}
