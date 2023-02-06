using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using BaggageApp.Helpers;
using BaggageApp.Services.Exceptions;

namespace BaggageApp.Services
{
    public class GenericAPIClient : IGenericAPIClient
    {
        #region Methods

        #endregion
        public async Task<APIResponse> ExecuteAPICallSimple(string endpoint, string pathService, Dictionary<string, string> headers)
        {

            using (HttpClient client = new HttpClient())
            {
                client.BaseAddress = new Uri(endpoint);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", Settings.ApiAuth);
                client.DefaultRequestHeaders.ExpectContinue = false;
                foreach (KeyValuePair<string, string> entry in headers)
                {
                    client.DefaultRequestHeaders.Add(entry.Key, entry.Value);
                }
                APIResponse response = new APIResponse();
                // New code:
                HttpResponseMessage responseMessage = await client.GetAsync(pathService);
                if (responseMessage.IsSuccessStatusCode)
                {
                    var responseData = await responseMessage.Content.ReadAsStringAsync();
                    if (!string.IsNullOrWhiteSpace(responseData))
                    {
                        response.IsSuccessful = true;
                        response.Data = responseData;
                    }
                    //var Items = JsonConvert.DeserializeObject<T>(responseData);
                    return response;
                }
                else if (responseMessage.StatusCode == System.Net.HttpStatusCode.Unauthorized)
                {
                    throw new UnauthorizedAPIException(String.Format("No tiene permisos para ejecutar el API {0}", pathService));
                }
                else
                {
                    throw new Exception("Error al Invocar un llamado al API, Info: " + responseMessage.ToString());
                }

            }
        }


        public async Task<APIResponse> ExecutePOSTAPICallSimple(string endpoint, string pathService, string requestContent, Dictionary<string, string> headers)
        {

            using (HttpClient client = new HttpClient())
            {
                client.BaseAddress = new Uri(endpoint);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", Settings.ApiAuth);
                client.DefaultRequestHeaders.ExpectContinue = false;
                foreach (KeyValuePair<string, string> entry in headers)
                {
                    client.DefaultRequestHeaders.Add(entry.Key, entry.Value);
                }

                APIResponse response = new APIResponse();

                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;

                StringContent content = new StringContent(requestContent, Encoding.UTF8, "application/json");

                using (HttpResponseMessage responseMessage = await client.PostAsync(pathService, content))
                {
                    if (responseMessage.IsSuccessStatusCode)
                    {
                        var responseData = await responseMessage.Content.ReadAsStringAsync();
                        if (!string.IsNullOrWhiteSpace(responseData))
                        {
                            response.IsSuccessful = true;
                            response.Data = responseData;
                        }
                        return response;
                    }
                    else if (responseMessage.StatusCode == System.Net.HttpStatusCode.Unauthorized)
                    {
                        throw new UnauthorizedAPIException(String.Format("No tiene permisos para ejecutar el API {0}", pathService));
                    }
                    else
                    {
                        throw new Exception("Error al Invocar un llamado al API, Info: " + responseMessage.ToString());
                    }
                }


            }
        }

    }
}
