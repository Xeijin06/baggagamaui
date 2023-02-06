using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BaggageApp.Services
{
    public interface IGenericAPIClient
    {
        Task<APIResponse> ExecuteAPICallSimple(string endpoint, string pathService, Dictionary<string, string> headers);

        Task<APIResponse> ExecutePOSTAPICallSimple(string endpoint, string pathService, string requestContent, Dictionary<string, string> headers);
    }
}
