using BaggageApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BaggageApp.Services
{
    public class BpmApiManager
    {
        IBpmApiRestService restService;
        public BpmApiManager(IBpmApiRestService service)
        {
            restService = service;
        }

        public Task<bool> LoginToApiBpm(string user, string password)
        {
            return restService.LoginToApiBpm(user, password);
        }

        public Task LogoutToApiBpm()
        {
            return restService.LogoutToApiBpm();
        }

        public Task<string> GetProcessId(string processName)
        {
            return restService.GetProcessId(processName);
        }

        public Task<string> CreateCase(string processId, PIR baggageCase)
        {
            return restService.CreateCase(processId, baggageCase);
        }

        public Task<bool> CheckCase(string processId, PIR baggageCase)
        {
            return restService.CheckCase(processId, baggageCase);
        }

        public Task<bool> CheckCaseOHD(string processId, OnHand ohdCase)
        {
            return restService.CheckCaseOHD(processId, ohdCase);
        }
        public Task<bool> CheckCaseFWD(string processId, ForwardItem forwardCase)
        {
            return restService.CheckCaseFWD(processId, forwardCase);
        }
        public Task<bool> CheckCaseDPR(string processId, DamageReport dprCase)
        {
            return restService.CheckCaseDPR(processId, dprCase);
        }
        public Task<string> CreateOHD(string processId, OnHand onHandCase)
        {
            return restService.CreateOHDAsync(processId, onHandCase);
        }
        public Task<string> CreateDPR(string processId, DamageReport damageCase)
        {
            try
            {
                return restService.CreateDPRAsync(processId, damageCase);
            }
            catch (System.Exception ex)
            {
                //Crashes.TrackError(ex);
                throw ex;
            }

        }
        public Task<string> CreateFWD(string processId, ForwardItem forwardCase)
        {
            return restService.CreateFWDAsync(processId, forwardCase);
        }
        public Task<bool> GetSessionContext()
        {
            return restService.GetSessionContext();
        }

        public Task<AHLDelayedBaggageDTO> GetAHLDetailsById(string processName)
        {
            return restService.GetAHLDetailsById(processName);
        }

        public Task<List<DPRResumeInfoDTO>> GetDPRMatchesByCriteria(string PNR, string bagTagNumbers)
        {
            return restService.GetDPRMatchesByCriteria(PNR, bagTagNumbers);
        }

        public Task<bool> RelateAHLWithFWD(PIR relateInfo)
        {
            return restService.RelateAHLWithFWD(relateInfo);
        }
    }
}
