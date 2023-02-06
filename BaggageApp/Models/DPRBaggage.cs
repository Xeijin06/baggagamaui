using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BaggageApp.Models
{
    class DPRBaggage
    {
        private List<BaggageDamageItem> _baggageDamages;
        public int Id { get; set; }
        public string BaggageTagNumber { get; set; }
        public string Carrier { get; set; }
        public string ColourType { get; set; }
        public string BaggageType { get; set; }
        public string DescriptorFirst { get; set; }
        public string DescriptorSecond { get; set; }
        public string DescriptorThird { get; set; }
        public string BrandName { get; set; }
        public string BaggageWeight { get; set; }
        public string BaggageDeliveryWeight { get; set; }
        public string Category { get; set; }
        public string Description { get; set; }
        public string FullBagTag { get; set; }
        public string TotalDamages { get; set; }
        public List<BaggageDamageItem> BaggageDamages
        {
            get
            {
                if (_baggageDamages == null)
                {
                    _baggageDamages = new List<BaggageDamageItem>();
                }
                return _baggageDamages;
            }
            set
            {
                _baggageDamages = value;
            }
        }
        public string TotalPhotos { get; set; }
        public string OriginalTagNumber { get; set; }
        public List<BaggagePhotoItem> BaggagePhotos { get; set; }

    }
}
