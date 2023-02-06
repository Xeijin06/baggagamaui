using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BaggageApp.Services
{
    public interface IMicrosoftGraphApi
    {
        Task<bool> UserMemberOfGroup(string group, string auth);
    }
}
