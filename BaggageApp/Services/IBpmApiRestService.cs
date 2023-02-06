using BaggageApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BaggageApp.Services
{
    public interface IBpmApiRestService
    {
        Task<bool> LoginToApiBpm(string user, string password);
        Task LogoutToApiBpm();
        Task<string> GetProcessId(string processName);
        Task<string> CreateCase(string processId, PIR baggageCase);
        Task<bool> CheckCase(string processId, PIR baggageCase);
        Task<bool> CheckCaseOHD(string processId, OnHand baggageCase);
        Task<bool> CheckCaseFWD(string processId, ForwardItem forwardCase);
        Task<bool> CheckCaseDPR(string processId, DamageReport damageCase);
        Task<string> CreateOHDAsync(string processId, OnHand onHandCase);
        Task<string> CreateFWDAsync(string processId, ForwardItem onHandCase);
        Task<string> CreateDPRAsync(string processId, DamageReport onHandCase);
        Task<bool> GetSessionContext();
        Task<AHLDelayedBaggageDTO> GetAHLDetailsById(string processBagId);
        Task<List<DPRResumeInfoDTO>> GetDPRMatchesByCriteria(string PNR, string bagTagNumbers);
        Task<bool> RelateAHLWithFWD(PIR relateInfo);
    }
}
