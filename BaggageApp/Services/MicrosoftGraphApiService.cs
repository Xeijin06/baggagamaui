using BaggageApp.Helpers;
using BaggageApp.ServiceModels;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace BaggageApp.Services
{
    public class MicrosoftGraphApiService : IMicrosoftGraphApi
    {
        public string ApiGraphUrl { get; set; }
        public List<AzureADGroup> Items { get; private set; }

        public MicrosoftGraphApiService()
        {
            ApiGraphUrl = Settings.GraphResourceUri;
        }

        public async Task<bool> UserMemberOfGroup(string group, string auth)
        {
            string _group = group;
            string _auth = auth;
            string _url = ApiGraphUrl;
            bool _userMemberOf = false;
            string graphQuery = "v1.0/me/memberOf";

            using (HttpClient client = new HttpClient())
            {
                var uri = new Uri(_url);
                client.BaseAddress = uri;
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(_auth.Split(' ')[0], _auth.Split(' ')[1]);

                using (HttpResponseMessage response = await client.GetAsync(graphQuery))
                {
                    if (response.IsSuccessStatusCode)
                    {
                        var content = await response.Content.ReadAsStringAsync();
                        var result = JsonConvert.DeserializeObject<AzureADGroupsOdata>(content);

                        Items = result.value;

                        if (Items != null)
                        {
                            AzureADGroup adGroup = Items.Where(g => g.displayName.Equals(_group)).FirstOrDefault();
                            _userMemberOf = (adGroup != null);
                        }

                    }
                }
            }
            return await Task.FromResult(_userMemberOf);
        }
    }
}
