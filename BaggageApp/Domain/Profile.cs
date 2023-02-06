using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BaggageApp.Domain
{
    public class Profile
    {
        public string UniqueId { get; set; }
        public string GivenName { get; set; }
        public string FamilyName { get; set; }
        public string DisplayableId { get; set; }
    }
}
