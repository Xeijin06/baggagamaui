using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BaggageApp.Helpers
{
    public interface IConnectionStatus
    {
        Task<bool> ConnectionEnabled(string url, int port = 80, bool showErrorMessage = true);
        Task<bool> ConnectionServiceEnabled(string url, int port = 80);
        Task<bool> ValidateAuthentication();
    }
}
