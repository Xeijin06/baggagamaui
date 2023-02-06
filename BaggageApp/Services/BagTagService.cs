using BaggageApp.Helpers;
using BaggageApp.Models.BagDetails;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BaggageApp.Services
{
    public class BagTagService : IBagTagService
    {
        #region Private members variables

        private IGenericAPIClient _apiClient;

        #endregion

        #region Properties
        public IGenericAPIClient ApiClient
        {
            get
            {
                if (_apiClient == null)
                {
                    _apiClient = new GenericAPIClient();
                }
                return _apiClient;
            }
            set
            {
                _apiClient = value;
            }
        }
        #endregion

        #region Methods

        public async Task<BagTagContext> GetBagTagDetail(string airlineCode, string bagTagNumber)
        {
            BagTagContext bagTagContext = null;

            string getBagTagDetailPath = string.Format(@"GetBagTagDetail(AirlineCode='{0}',BagTagNumber='{1}')", airlineCode, bagTagNumber);
            try
            {
                APIResponse response = await ApiClient.ExecuteAPICallSimple(Settings.BCAApiApp, getBagTagDetailPath, new Dictionary<string, string>());
                if (response != null && response.IsSuccessful)
                {
                    bagTagContext = JsonConvert.DeserializeObject<BagTagContext>(response.Data);
                }
            }
            catch (Exception)
            {
                throw;
            }
            return bagTagContext;
        }

        Task<BagTagContext> IBagTagService.GetBagTagDetail(string airlineCode, string bagTagNumber)
        {
            throw new NotImplementedException();
        }
        #endregion
    }
}
