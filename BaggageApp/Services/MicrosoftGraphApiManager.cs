using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BaggageApp.Services
{
    public class MicrosoftGraphApiManager
    {
        IMicrosoftGraphApi resetService;
        public MicrosoftGraphApiManager(IMicrosoftGraphApi service)
        {
            resetService = service;
        }

        public Task<bool> UserMemberOfGroup(string group, string auth)
        {
            return resetService.UserMemberOfGroup(group, auth);
        }
    }
}
