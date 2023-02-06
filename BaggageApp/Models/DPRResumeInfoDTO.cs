using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BaggageApp.Models
{
    public class DPRResumeInfoDTO
    {
        [JsonProperty("Id")]
        public int Id { get; set; }
        public string BPMCaseId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string CreatedByStation { get; set; }
        public string CreatedByUser { get; set; }
        public string BPMCaseStatus { get; set; }
        public int DPRType { get; set; }
        public string AffectationType { get; set; }
        public string BagTagNumberList { get; set; }
        public string FlightNumberList { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}
