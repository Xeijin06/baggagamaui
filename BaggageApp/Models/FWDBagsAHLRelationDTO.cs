using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BaggageApp.Models
{
    public class FWDBagsAHLRelationDTO
    {
        public int id { get; set; }
        public string FWDRequestNumber { get; set; }
        public string AHLRequestNumber { get; set; }
        public List<MFBagsDTO> FWDBags { get; set; }
        public string RelatedFileLocatorStation { get; set; }
        public string RelatedFileLocatorAirline { get; set; }
        public string RelatedFileLocatorSeqNumber { get; set; }
        public bool RelatedFileLocator { get; set; }
    }
}
