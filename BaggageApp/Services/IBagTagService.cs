using BaggageApp.Models.BagDetails;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BaggageApp.Services
{
    public interface IBagTagService
    {
        Task<BagTagContext> GetBagTagDetail(string airlineCode, string bagTagNumber);
    }
}
