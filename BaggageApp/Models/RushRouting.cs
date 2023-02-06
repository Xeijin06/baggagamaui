using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BaggageApp.Models
{
    public class RushRouting
    {
        public int Id { get; set; }
        public int Sequential { get; set; }
        public string AirlineCode { get; set; }
        public string StationCode { get; set; }
        public bool IsLast { get; set; }
    }
}
