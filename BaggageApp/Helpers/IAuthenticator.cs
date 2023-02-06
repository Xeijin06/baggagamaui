using BaggageApp.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BaggageApp.Helpers
{
    public interface IAuthenticator
    {
        Task<AuthenticateResponse> Authenticate(string authority, string resource, string clientId, string graphUri, string returnUri);
        Task ClearToken(string authority);
    }
}
