using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BaggageApp.ServiceModels
{
    public class BpmSessionContext
    {
        public string copyright { get; set; }
        public string user_id { get; set; }
        public string user_name { get; set; }
        public string session_id { get; set; }
        public string conf { get; set; }
        public string is_technical_user { get; set; }
        public string version { get; set; }
    }
}
