using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using BaggageApp.Models;
using BaggageApp.DataStores;
using BaggageApp.Helpers;
using BaggageApp.ServiceModels;

namespace BaggageApp.Services
{
    public class BpmApiService : IBpmApiRestService
    {
        #region Private members variables
        private IGenericAPIClient _apiClient;
        IDataStore<PIR> pirDataStore;
        IDataStore<OnHand> ohdDataStore;
        IDataStore<ForwardItem> fwdDataStore;
        IDataStore<DamageReport> dprDataStore;
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
        public CookieCollection Collection { get; set; }
        public CookieContainer CookiesContainer { get; set; }
        public string StrCookietoPass { get; set; }
        public string SessionID { get; set; }
        public string Token { get; set; }
        public string BpmUrl { get; set; }


        private IConnectionStatus connectionStatus;

        public BpmApiService()
        {
            pirDataStore = new PIRDataStore();
            ohdDataStore = new OnHandDataStore();
            fwdDataStore = new ForwardDataStore();
            dprDataStore = new DPRDataStore();
            BpmUrl = Settings.BonitaBPM;
            connectionStatus = new ConnectionStatus();
        }

        public async Task<bool> CheckCase(string processId, PIR baggageCase)
        {
            PIR _bpmFile = new PIR();

            if (CookiesContainer == null)
            {
                Settings.BPMToken = "";
                return await Task.FromResult(false);
            }

            string url = BpmUrl;

            var handler = new HttpClientHandler
            {
                CookieContainer = CookiesContainer
            };

            string getBPMId = string.Format(@"API/bdm/businessData/com.company.model.ProcessBagFiles?q=findByProcessBagID&p=0&c=1&f=ProcessBagID={0}", baggageCase.CaseId);
            int _bpmCaseID = 0;

            using (var client = new HttpClient(handler))
            {
                var uri = new Uri(url);
                client.BaseAddress = uri;
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                //client.DefaultRequestHeaders.Add("X-Bonita-API-Token", Token);

                using (HttpResponseMessage response = await client.GetAsync(getBPMId))
                {

                    if (response.IsSuccessStatusCode)
                    {
                        var responseBodyAsText = await response.Content.ReadAsStringAsync();

                        if (!string.IsNullOrEmpty(responseBodyAsText))
                        {
                            List<BPMFIleObject> currentFile = (List<BPMFIleObject>)JsonConvert.DeserializeObject(responseBodyAsText, typeof(List<BPMFIleObject>));
                            if (currentFile.Count > 0)
                            {
                                _bpmCaseID = currentFile.FirstOrDefault().BPMCaseID;

                                if (_bpmCaseID != 0)
                                {

                                    if (string.IsNullOrEmpty(baggageCase.BpmCaseId))
                                    {
                                        baggageCase.BpmCaseId = _bpmCaseID.ToString();
                                        await pirDataStore.UpdateItemAsync(baggageCase);

                                        return true;
                                    }
                                    else
                                    {
                                        return false;
                                    }

                                }
                                else
                                {
                                    return false;
                                }
                            }
                            else
                            {
                                return false;
                            }
                        }
                        else
                        {
                            return false;
                        }
                    }
                    else
                    {
                        return false;
                    }
                }

            }
        }

        public async Task<string> CreateCase(string processId, PIR baggageCase)
        {
            string caseId = string.Empty;
            string PIR_JSONMessage = string.Empty;
            string url = BpmUrl;

            var handler = new HttpClientHandler
            {
                CookieContainer = CookiesContainer
            };

            string processInstantiation = string.Format(@"API/bpm/process/{0}/instantiation", processId);


            using (var client = new HttpClient(handler))
            {
                var uri = new Uri(url);
                client.BaseAddress = uri;
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                client.DefaultRequestHeaders.Add("X-Bonita-API-Token", Token);

                PIR_JSONMessage = await SerializePIRtoJSON(baggageCase);

                StringContent httpContent = new StringContent(PIR_JSONMessage, Encoding.UTF8, "application/json");

                using (HttpResponseMessage response = await client.PostAsync(processInstantiation, httpContent))
                {

                    if (response.IsSuccessStatusCode)
                    {
                        var responseBodyAsText = await response.Content.ReadAsStringAsync();

                        if (!string.IsNullOrEmpty(responseBodyAsText))
                        {
                            CaseCreatedBpm createdProcess = (CaseCreatedBpm)JsonConvert.DeserializeObject(responseBodyAsText, typeof(CaseCreatedBpm));

                            caseId = createdProcess.caseId.ToString();
                            baggageCase.BpmCaseId = caseId;

                            await pirDataStore.UpdateItemAsync(baggageCase);

                        }

                    }
                    else
                    {
                        var responseBodyAsText = await response.Content.ReadAsStringAsync();
                        var ex = new ApplicationException(string.Format("Error durante sincronización de AHL{0} a BONITA. Request: {1}. Response: {2}",
                                                                baggageCase.CaseId, PIR_JSONMessage, responseBodyAsText));
                        //Crashes.TrackError(ex);
                    }

                    return await Task.FromResult(response.StatusCode.ToString());

                }
            }
        }

        private async Task<string> SerializePIRtoJSON(PIR baggageCase)
        {
            string newJsonBcaCase = string.Empty;
            try
            {
                newJsonBcaCase = await JsonGeneratorToCreate(baggageCase);
            }
            catch (Exception ex)
            {
                Dictionary<string, string> properties = new Dictionary<string, string>();
                properties.Add("MethodName", "JsonGeneratorToCreate");
                properties.Add("PIR_JSON", "Se debe descargar el reporte para ver el mensaje JSON completo.");
                ApplicationException errorContainer = new ApplicationException(string.Format("Error message: {0}, PIR_JSON: {1}", ex.Message, JsonConvert.SerializeObject(baggageCase)), ex);
                //Crashes.TrackError(errorContainer, properties);
            }
            return newJsonBcaCase;
        }

        public async Task<string> GetProcessId(string processName)
        {
            if (CookiesContainer == null)
            {
                Settings.BPMToken = "";
                return await Task.FromResult("");
            }

            string url = BpmUrl;

            var handler = new HttpClientHandler
            {
                CookieContainer = CookiesContainer
            };

            string getProcessId = string.Format(@"API/bpm/process?f=displayName={0}&p=0&c=10&o=version&f=activationState=ENABLED", processName);
            string processId = string.Empty;

            using (var client = new HttpClient(handler))
            {
                var uri = new Uri(url);
                client.BaseAddress = uri;
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                using (HttpResponseMessage response = await client.GetAsync(getProcessId))
                {

                    if (response.IsSuccessStatusCode)
                    {
                        var responseBodyAsText = await response.Content.ReadAsStringAsync();

                        if (!string.IsNullOrEmpty(responseBodyAsText))
                        {
                            List<ProcessBpm> currentProcess = (List<ProcessBpm>)JsonConvert.DeserializeObject(responseBodyAsText, typeof(List<ProcessBpm>));

                            processId = currentProcess.FirstOrDefault().id;
                        }

                    }

                    return await Task.FromResult(processId);

                }
            }
        }

        public async Task<bool> LoginToApiBpm(string user, string password)
        {
            string url = BpmUrl;
            bool login = false;

            CookiesContainer = new CookieContainer();
            var handler = new HttpClientHandler
            {
                CookieContainer = CookiesContainer
            };

            using (var client = new HttpClient(handler))
            {
                var uri = new Uri(url);
                client.BaseAddress = uri;

                var content = new FormUrlEncodedContent(new[]
                {
                    new KeyValuePair<string, string>("username", user),//"BonitaAdmin"
                    new KeyValuePair<string, string>("password", password),//"Pa$$word"
                    new KeyValuePair<string, string>("redirect", "false"),
                    new KeyValuePair<string, string>("redirectUrl", ""),
                });

                using (HttpResponseMessage response = await client.PostAsync("loginservice", content))
                {

                    if (response.IsSuccessStatusCode)
                    {
                        //var responseBodyAsText = await response.Content.ReadAsStringAsync();

                        //if (!string.IsNullOrEmpty(responseBodyAsText))
                        //{
                        //    //Console.WriteLine("Unsuccessful Login.Bonita bundle may not have been started, or the URL is invalid.");
                        //    return;
                        //}

                        Collection = CookiesContainer.GetCookies(uri);
                        StrCookietoPass = response.Headers.GetValues("Set-Cookie").FirstOrDefault();

                        SessionID = Collection["SESSIONID"].ToString();
                        Token = Collection["X-Bonita-API-Token"].ToString().Split('=')[1];

                        Settings.BPMToken = Token;
                        Settings.CurrentUser = user;
                        Settings.BPMSessionID = SessionID;
                        Settings.IsAuthenticated = true;
                        Settings.ShowUnAuthorizedMessage = false;
                        login = true;
                    }
                    else
                    {
                        //Console.WriteLine("Login Error" + (int)response.StatusCode + "," + response.ReasonPhrase);
                    }

                }
            }
            return await Task.FromResult(login);
        }

        public async Task LogoutToApiBpm()
        {
            string url = BpmUrl;
            int port = 80;
            string urlTestConnection = Settings.TestConnectionDefaultURL;
            bool isConnected = await connectionStatus.ConnectionEnabled(urlTestConnection, port, false);

            if (isConnected)
            {
                CookiesContainer = new CookieContainer();
                var handler = new HttpClientHandler
                {
                    CookieContainer = CookiesContainer
                };
                using (var client = new HttpClient(handler))
                {
                    var uri = new Uri(url);
                    client.BaseAddress = uri;

                    var content = new FormUrlEncodedContent(new[]
                    {
                        new KeyValuePair<string, string>("redirect", "false")
                    });

                    using (HttpResponseMessage response = await client.PostAsync("logoutservice", content))
                    {

                        if (response.IsSuccessStatusCode)
                        {

                            Settings.BPMToken = "";
                            Settings.CurrentUser = "";
                            Settings.IsAuthenticated = false;
                            Settings.ShowUnAuthorizedMessage = false;

                        }

                    }
                }
            }

        }

        public async Task<bool> CheckCaseOHD(string processId, OnHand onHandCase)
        {
            if (CookiesContainer == null)
            {
                Settings.BPMToken = "";
                return await Task.FromResult(false);
            }

            string url = BpmUrl;

            var handler = new HttpClientHandler
            {
                CookieContainer = CookiesContainer
            };

            string getBPMId = string.Format(@"API/bdm/businessData/com.company.model.MFBagFiles?q=findByRequestID&p=0&c=50&f=RequestID={0}", onHandCase.CaseId);
            int _bpmCaseID = 0;

            using (var client = new HttpClient(handler))
            {
                var uri = new Uri(url);
                client.BaseAddress = uri;
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                //client.DefaultRequestHeaders.Add("X-Bonita-API-Token", Token);

                using (HttpResponseMessage response = await client.GetAsync(getBPMId))
                {

                    if (response.IsSuccessStatusCode)
                    {
                        var responseBodyAsText = await response.Content.ReadAsStringAsync();

                        if (!string.IsNullOrEmpty(responseBodyAsText))
                        {
                            List<BPMFIleObject> currentFile = (List<BPMFIleObject>)JsonConvert.DeserializeObject(responseBodyAsText, typeof(List<BPMFIleObject>));
                            if (currentFile.Count > 0)
                            {
                                _bpmCaseID = currentFile.FirstOrDefault().BPMCaseID;

                                if (_bpmCaseID != 0)
                                {

                                    if (string.IsNullOrEmpty(onHandCase.BpmCaseId))
                                    {
                                        onHandCase.BpmCaseId = _bpmCaseID.ToString();
                                        await ohdDataStore.UpdateItemAsync(onHandCase);

                                        return true;
                                    }
                                    else
                                    {
                                        return false;
                                    }

                                }
                                else
                                {
                                    return false;
                                }
                            }
                            else
                            {
                                return false;
                            }
                        }
                        else
                        {
                            return false;
                        }
                    }
                    else
                    {
                        return false;
                    }
                }

            }
        }

        public async Task<bool> CheckCaseFWD(string processId, ForwardItem forwardCase)
        {
            if (CookiesContainer == null)
            {
                Settings.BPMToken = "";
                return await Task.FromResult(false);
            }

            string url = BpmUrl;

            var handler = new HttpClientHandler
            {
                CookieContainer = CookiesContainer
            };

            string getBPMId = string.Format(@"API/bdm/businessData/com.company.model.MFBagFiles?q=findByRequestID&p=0&c=50&f=RequestID={0}", forwardCase.CaseId);
            int _bpmCaseID = 0;

            using (var client = new HttpClient(handler))
            {
                var uri = new Uri(url);
                client.BaseAddress = uri;
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                //client.DefaultRequestHeaders.Add("X-Bonita-API-Token", Token);

                using (HttpResponseMessage response = await client.GetAsync(getBPMId))
                {

                    if (response.IsSuccessStatusCode)
                    {
                        var responseBodyAsText = await response.Content.ReadAsStringAsync();

                        if (!string.IsNullOrEmpty(responseBodyAsText))
                        {
                            List<BPMFIleObject> currentFile = (List<BPMFIleObject>)JsonConvert.DeserializeObject(responseBodyAsText, typeof(List<BPMFIleObject>));
                            if (currentFile.Count > 0)
                            {
                                _bpmCaseID = currentFile.FirstOrDefault().BPMCaseID;

                                if (_bpmCaseID != 0)
                                {

                                    if (string.IsNullOrEmpty(forwardCase.BpmCaseId))
                                    {
                                        forwardCase.BpmCaseId = _bpmCaseID.ToString();
                                        await fwdDataStore.UpdateItemAsync(forwardCase);

                                        return true;
                                    }
                                    else
                                    {
                                        return false;
                                    }

                                }
                                else
                                {
                                    return false;
                                }
                            }
                            else
                            {
                                return false;
                            }
                        }
                        else
                        {
                            return false;
                        }
                    }
                    else
                    {
                        return false;
                    }
                }

            }
        }

        public async Task<bool> CheckCaseDPR(string processId, DamageReport damageCase)
        {
            if (CookiesContainer == null)
            {
                Settings.BPMToken = "";
                return await Task.FromResult(false);
            }

            string url = BpmUrl;

            var handler = new HttpClientHandler
            {
                CookieContainer = CookiesContainer
            };

            string getBPMId = string.Format(@"API/bdm/businessData/com.company.model.DPRBagFiles?q=findByRequestID&p=0&c=50&f=RequestID={0}", damageCase.CaseId);
            int _bpmCaseID = 0;

            using (var client = new HttpClient(handler))
            {
                var uri = new Uri(url);
                client.BaseAddress = uri;
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                //client.DefaultRequestHeaders.Add("X-Bonita-API-Token", Token);

                using (HttpResponseMessage response = await client.GetAsync(getBPMId))
                {

                    if (response.IsSuccessStatusCode)
                    {
                        var responseBodyAsText = await response.Content.ReadAsStringAsync();

                        if (!string.IsNullOrEmpty(responseBodyAsText))
                        {
                            List<BPMFIleObject> currentFile = (List<BPMFIleObject>)JsonConvert.DeserializeObject(responseBodyAsText, typeof(List<BPMFIleObject>));
                            if (currentFile.Count > 0)
                            {
                                _bpmCaseID = currentFile.FirstOrDefault().BPMCaseID;

                                if (_bpmCaseID != 0)
                                {

                                    if (string.IsNullOrEmpty(damageCase.BpmCaseId))
                                    {
                                        damageCase.BpmCaseId = _bpmCaseID.ToString();
                                        await dprDataStore.UpdateItemAsync(damageCase);

                                        return true;
                                    }
                                    else
                                    {
                                        return false;
                                    }

                                }
                                else
                                {
                                    return false;
                                }
                            }
                            else
                            {
                                return false;
                            }
                        }
                        else
                        {
                            return false;
                        }
                    }
                    else
                    {
                        return false;
                    }
                }

            }
        }

        public async Task<string> CreateOHDAsync(string processId, OnHand onHandCase)
        {
            string caseId = string.Empty;

            string url = BpmUrl;

            var handler = new HttpClientHandler
            {
                CookieContainer = CookiesContainer
            };

            string processInstantiation = string.Format(@"API/bpm/process/{0}/instantiation", processId);

            using (var client = new HttpClient(handler))
            {
                var uri = new Uri(url);
                client.BaseAddress = uri;
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                client.DefaultRequestHeaders.Add("X-Bonita-API-Token", Token);

                string newJsonBcaCase = await JsonGeneratorToCreateOHD(onHandCase);

                StringContent httpContent = new StringContent(newJsonBcaCase, Encoding.UTF8, "application/json");

                using (HttpResponseMessage response = await client.PostAsync(processInstantiation, httpContent))
                {

                    if (response.IsSuccessStatusCode)
                    {
                        var responseBodyAsText = await response.Content.ReadAsStringAsync();

                        if (!string.IsNullOrEmpty(responseBodyAsText))
                        {
                            CaseCreatedBpm createdProcess = (CaseCreatedBpm)JsonConvert.DeserializeObject(responseBodyAsText, typeof(CaseCreatedBpm));

                            caseId = createdProcess.caseId.ToString();
                            onHandCase.BpmCaseId = caseId;

                            await ohdDataStore.UpdateItemAsync(onHandCase);
                        }

                    }
                    else
                    {
                        var responseBodyAsText = await response.Content.ReadAsStringAsync();
                        var ex = new ApplicationException(string.Format("Error durante sincronización de OHD {0} a BONITA. Request: {1}. Response: {2}",
                                                                onHandCase.CaseId, newJsonBcaCase, responseBodyAsText));

                        //Crashes.TrackError(ex);
                    }
                    return await Task.FromResult(response.StatusCode.ToString());

                }
            }
        }

        public async Task<string> CreateFWDAsync(string processId, ForwardItem forwardCase)
        {
            string caseId = string.Empty;
            string newJsonBcaCase = string.Empty;
            string url = BpmUrl;

            var handler = new HttpClientHandler
            {
                CookieContainer = CookiesContainer
            };

            string processInstantiation = string.Format(@"API/bpm/process/{0}/instantiation", processId);

            using (var client = new HttpClient(handler))
            {
                var uri = new Uri(url);
                client.BaseAddress = uri;
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                client.DefaultRequestHeaders.Add("X-Bonita-API-Token", Token);

                newJsonBcaCase = await JsonGeneratorToCreateFWD(forwardCase);
                StringContent httpContent = new StringContent(newJsonBcaCase, Encoding.UTF8, "application/json");

                using (HttpResponseMessage response = await client.PostAsync(processInstantiation, httpContent))
                {

                    if (response.IsSuccessStatusCode)
                    {
                        var responseBodyAsText = await response.Content.ReadAsStringAsync();

                        if (!string.IsNullOrEmpty(responseBodyAsText))
                        {
                            CaseCreatedBpm createdProcess = (CaseCreatedBpm)JsonConvert.DeserializeObject(responseBodyAsText, typeof(CaseCreatedBpm));

                            caseId = createdProcess.caseId.ToString();
                            forwardCase.BpmCaseId = caseId;

                            await fwdDataStore.UpdateItemAsync(forwardCase);
                        }

                    }
                    else
                    {
                        var responseBodyAsText = await response.Content.ReadAsStringAsync();
                        var ex = new ApplicationException(string.Format("Error durante sincronización de FWD {0} a BONITA. Request: {1}. Response: {2}",
                                                                forwardCase.CaseId, newJsonBcaCase, responseBodyAsText));

                        //Crashes.TrackError(ex);
                    }

                    return await Task.FromResult(response.StatusCode.ToString());

                }
            }
        }
        public async Task<string> CreateDPRAsync(string processId, DamageReport damageCase)
        {
            string caseId = string.Empty;

            string url = BpmUrl;

            var handler = new HttpClientHandler
            {
                CookieContainer = CookiesContainer
            };

            string processInstantiation = string.Format(@"API/bpm/process/{0}/instantiation", processId);

            using (var client = new HttpClient(handler))
            {
                var uri = new Uri(url);
                client.BaseAddress = uri;
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                client.DefaultRequestHeaders.Add("X-Bonita-API-Token", Token);

                string newJsonBcaCase = await JsonGeneratorToCreateDPR(damageCase);
                if (string.IsNullOrEmpty(newJsonBcaCase))
                {
                    throw new ApplicationException(string.Format("Error durante construcción del mensaje JSON del DPR con Id {0}", damageCase.CaseId));
                }
                StringContent httpContent = new StringContent(newJsonBcaCase, Encoding.UTF8, "application/json");

                using (HttpResponseMessage response = await client.PostAsync(processInstantiation, httpContent))
                {

                    if (response.IsSuccessStatusCode)
                    {
                        var responseBodyAsText = await response.Content.ReadAsStringAsync();

                        if (!string.IsNullOrEmpty(responseBodyAsText))
                        {
                            CaseCreatedBpm createdProcess = (CaseCreatedBpm)JsonConvert.DeserializeObject(responseBodyAsText, typeof(CaseCreatedBpm));

                            caseId = createdProcess.caseId.ToString();
                            damageCase.BpmCaseId = caseId;

                            await dprDataStore.UpdateItemAsync(damageCase);
                        }

                    }
                    else
                    {
                        var responseBodyAsText = await response.Content.ReadAsStringAsync();
                        var ex = new ApplicationException(string.Format("Error durante sincronización de DPR {0} a BONITA. Request: {1}. Response: {2}",
                                                                damageCase.CaseId, newJsonBcaCase, responseBodyAsText));
                        //Crashes.TrackError(ex);
                    }

                    return await Task.FromResult(response.StatusCode.ToString());

                }
            }
        }
        private async Task<string> JsonGeneratorToCreateOHD(OnHand onHandCase)
        {
            List<string> addresses = new List<string>();
            string requestId = onHandCase.CaseId;
            string baggages = onHandCase.Baggages;
            string itineraries = onHandCase.Itinerary;
            string userName = Settings.CurrentUser;

            List<PassengerOHD> Passengers = new List<PassengerOHD>
            {
                new PassengerOHD()
                {
                    RequestID = requestId,
                    PassengerID = 1,
                    PNR = NotNull(onHandCase.RecordLocator),
                    PNames = NotNull(onHandCase.LastName),
                    GivenName = NotNull(onHandCase.Name),
                    Initials = NotNull(onHandCase.Initials),
                    Title = NotNull(onHandCase.PassengerTitle),
                    FrequentFlyerNumber = NotNull(string.Format("{0}{1}", onHandCase.FFCarrier, onHandCase.FrequentFlyer)),
                    EmailAddress = NotNull(onHandCase.EmailAddress),
                    AddressOnBag = NotNull(onHandCase.BagAddress),
                    PhoneOnBag = NotNull(onHandCase.BagPhone),
                    Addresses = addresses
                }
            };

            List<Bag> Bags = new List<Bag>();

            List<OHDBaggage> baggageInfo = string.IsNullOrEmpty(baggages) ? null : JsonConvert.DeserializeObject<List<OHDBaggage>>(baggages);


            int bagSequential = 0;
            int contentSequential = 0;

            foreach (OHDBaggage bag in baggageInfo)
            {
                List<Content> Contents = new List<Content>();
                List<Contenido> Contenido = new List<Contenido>();
                List<string> bagPhotos = new List<string>();

                contentSequential = 0;

                bagSequential = bagSequential + 1;

                if (bag.BaggageContents != null)
                {
                    foreach (BaggageContentItem content in bag.BaggageContents)
                    {
                        contentSequential = contentSequential + 1;

                        Contents.Add
                            (
                                new Content()
                                {
                                    RequestID = requestId,
                                    BagID = bagSequential,
                                    ContentSequential = contentSequential,
                                    Category = NotNull(content.CategoryId),
                                    CategoryWeight = content.CategoryWeight,
                                    Description1 = NotNull(content.ContentDescription),
                                    Description2 = NotNull(content.ContentDescription)
                                }
                            );

                        CategoriaSel categorialSel = new CategoriaSel()
                        {
                            ID = NotNull(content.CategoryId),
                            WTWeight = content.CategoryWeight.ToString(),
                            DESC = NotNull(content.CategoryName)
                        };
                        Contenido.Add(
                            new Contenido()
                            {
                                CategoriaSel = categorialSel,
                                Descripcion = NotNull(content.ContentDescription)
                            }
                            );
                    }
                }

                if (bag.BaggagePhotos != null)
                {
                    string apiApp = Settings.BCAApiApp;
                    string auth = Settings.ApiAuth;

                    foreach (BaggagePhotoItem photos in bag.BaggagePhotos)
                    {
                        bagPhotos.Add(
                            string.Format("{0}/Home/Image?name={1}&auth={2}", apiApp, photos.FileName, auth)
                            );
                    }
                }




                Bags.Add
                    (
                        new Bag()
                        {
                            RequestID = requestId,
                            BagID = bagSequential,
                            TagNumber = NotNull(bag.BaggageTagNumber),
                            Airline = NotNull(bag.Carrier),
                            Aerolinea = onHandCase.Airline,
                            Color = NotNull(bag.ColourType),
                            Type = NotNull(bag.BaggageType),
                            DescriptorMaterial = bag.DescriptorFirst,
                            DescriptorBasic = bag.DescriptorSecond,
                            DescriptorExternal = bag.DescriptorThird,
                            Descriptors = string.Format(@"{0}{1}{2}", bag.DescriptorFirst, bag.DescriptorSecond, bag.DescriptorThird),
                            BrandInformation = NotNull(bag.BrandName),
                            BaggageWeight = bag.BaggageWeight,
                            RushAirline = "",
                            RushTagNumber = "",
                            PassengerID = 1,//Revisar
                            RelatedFileLocatorStation = "",
                            RelatedFileLocatorAirline = "",
                            RelatedFileLocatorSeqNumber = "",
                            RelatedFileLocator = 0,
                            AddressOnBag = "",
                            PhoneOnBag = "",
                            ValidacionDescriptores = "0",
                            ValidacionDescriptoresMin = "0",
                            Contents = Contents,
                            Contenido = Contenido,
                            BagImageURL = string.Join(",", bagPhotos)
                        }
                    );


            }

            List<Itinerary> itineraryInfo = string.IsNullOrEmpty(itineraries) ? null : JsonConvert.DeserializeObject<List<Itinerary>>(itineraries);

            int itinerarySequential = 0;
            List<FlightOHD> Flights = new List<FlightOHD>();

            foreach (Itinerary itinerary in itineraryInfo)
            {
                itinerarySequential++;

                Flights.Add
                    (
                        new FlightOHD
                        {
                            RequestID = requestId,
                            Sequential = itinerarySequential,//?
                            RoutingTypeId = 1,//?
                            Origin = NotNull(itinerary.Departure),
                            Destination = NotNull(itinerary.Arrival),
                            Airline = NotNull(itinerary.Airline),
                            Flight = NotNull(itinerary.FlightNumberOnly),
                            FlightDate = itinerary.ItineraryDateFull.ToString("yyyy-MM-ddTHH:mm:ss.fffZ")
                        }
                    );
            }


            List<Log> Logs = new List<Log>
            {
                new Log()
                {
                    RequestID = requestId,
                    LogID = 1,
                    Username = onHandCase.CreatedBy,
                    LogDate = onHandCase.CreatedDate.ToString("yyyy-MM-ddTHH:mm:ss.fffZ"),
                    Comments = "Creación de Baggage Service - OHD Request - Mobile App"
                }
            };

            CreateOhd newOHD = new CreateOhd()
            {
                bagFileInput = new BagFileInput()
                {
                    RequestID = requestId,
                    RequestType = 1,
                    SupplementalInformation = NotNull(onHandCase.ComplementaryInfo),
                    AgentName = onHandCase.AgentInitials,
                    NumberOfBags = !string.IsNullOrEmpty(onHandCase.BaggageCount) ? int.Parse(onHandCase.BaggageCount) : 0,
                    Airline = onHandCase.Airline.Split('-')[0].Trim(),
                    Station = onHandCase.Station.Split('-')[0].Trim(),
                    ReasonForLoss = 0,
                    FaultStation = "",
                    FaultAirline = "",
                    FaultTerminal = "",
                    CommentsOnLoss = "",
                    RelatedFileLocatorStation = "",
                    RelatedFileLocatorAirline = "",
                    RelatedFileLocatorSeqNumber = "",
                    BagFileStatus = "NO ENCONTRADO",
                    CreationDate = onHandCase.CreatedDate.ToString("yyyy-MM-ddTHH:mm:ss.fffZ"),
                    Passengers = Passengers,
                    Bags = Bags,
                    Flights = Flights,
                    Logs = Logs,
                    BPMCaseID = 0,
                    Routings = new List<RushRoutingFWD>(),
                    DestinationPlatform = ""
                },
                codeDestination = ""
            };

            string jsonToCreateOHD = JsonConvert.SerializeObject(newOHD);


            return await Task.FromResult(jsonToCreateOHD);
        }

        private async Task<string> JsonGeneratorToCreateFWD(ForwardItem forwardCase)
        {
            List<string> addresses = new List<string>();
            string requestId = forwardCase.CaseId;
            string baggages = forwardCase.Baggages;
            string itineraries = forwardCase.Itinerary;
            string passengers = forwardCase.Passengers;
            string rushrouting = forwardCase.RushRoutings;
            string userName = forwardCase.CreatedBy;

            List<FWDPassenger> passengerList = string.IsNullOrEmpty(passengers) ? null : JsonConvert.DeserializeObject<List<FWDPassenger>>(passengers);
            List<PassengerFWD> PassengersBPM = new List<PassengerFWD>();
            foreach (FWDPassenger paxItem in passengerList)
            {
                PassengersBPM.Add(
                    new PassengerFWD()
                    {
                        RequestID = requestId,
                        PassengerID = paxItem.Id,
                        PNR = "",
                        PNames = paxItem.LastName,
                        GivenName = paxItem.Name,
                        Initials = "",
                        Title = "",
                        FrequentFlyerNumber = "",
                        EmailAddress = "",
                        AddressOnBag = "",
                        PhoneOnBag = "",
                        Addresses = new List<string>()
                    }
                );
            }


            List<BagFWD> Bags = new List<BagFWD>();

            List<FWDBaggage> baggageInfo = string.IsNullOrEmpty(baggages) ? null : JsonConvert.DeserializeObject<List<FWDBaggage>>(baggages);


            int bagSequential = 0;
            int contentSequential = 0;

            foreach (FWDBaggage bag in baggageInfo)
            {
                List<ContentFWD> Contents = new List<ContentFWD>();


                contentSequential = 0;

                bagSequential = bagSequential + 1;


                Bags.Add
                    (
                        new BagFWD()
                        {
                            RequestID = requestId,
                            BagID = bagSequential,
                            TagNumber = NotNull(bag.TagNumber),
                            Airline = (NotNull(bag.Airline) != "") ? bag.Airline.Substring(0, 2) : "",
                            Aerolinea = "",
                            Color = "",
                            Type = "",
                            DescriptorMaterial = "",
                            DescriptorBasic = "",
                            DescriptorExternal = "",
                            Descriptors = "",
                            BrandInformation = "",
                            BaggageWeight = "",
                            RushAirline = (NotNull(bag.FWDAirline) != "") ? bag.FWDAirline.Substring(0, 2) : "",
                            RushTagNumber = NotNull(bag.FWDTagNumber),
                            PassengerID = 1,//Revisar
                            RelatedFileLocatorStation = "",
                            RelatedFileLocatorAirline = "",
                            RelatedFileLocatorSeqNumber = "",
                            RelatedFileLocator = 0,
                            AddressOnBag = "",
                            PhoneOnBag = "",
                            ValidacionDescriptores = "",
                            ValidacionDescriptoresMin = "",
                            Contents = Contents,
                            BagImageURL = ""
                        }
                    );


            }

            List<Itinerary> itineraryInfo = string.IsNullOrEmpty(itineraries) ? null : JsonConvert.DeserializeObject<List<Itinerary>>(itineraries);
            List<Itinerary> itineraryPassengerInfo = string.IsNullOrEmpty(forwardCase.ItineraryPassenger) ? null : JsonConvert.DeserializeObject<List<Itinerary>>(forwardCase.ItineraryPassenger);
            int itinerarySequential = 0;
            List<FlightFWD> Flights = new List<FlightFWD>();
            List<RushRouting> routingList = string.IsNullOrEmpty(forwardCase.RushRoutings) ? null : JsonConvert.DeserializeObject<List<RushRouting>>(forwardCase.RushRoutings);
            List<RushRoutingFWD> rushRoutingFWD = new List<RushRoutingFWD>();

            foreach (Itinerary itinerary in itineraryInfo)
            {
                itinerarySequential++;

                Flights.Add
                    (
                        new FlightFWD
                        {
                            RequestID = requestId,
                            Sequential = itinerarySequential,//?
                            RoutingTypeId = 2,//Itinerario del equipaje?
                            Origin = "",
                            Destination = "",
                            Airline = NotNull(itinerary.Airline) != "" ? itinerary.Airline.Substring(0, 2) : "",
                            Flight = NotNull(itinerary.FlightNumberOnly),
                            FlightDate = itinerary.ItineraryDateFull.ToString("yyyy-MM-ddTHH:mm:ss.fffZ")
                        }
                    );
            }

            itinerarySequential = 0;
            foreach (Itinerary itinerary in itineraryPassengerInfo)
            {
                itinerarySequential++;

                Flights.Add
                    (
                        new FlightFWD
                        {
                            RequestID = requestId,
                            Sequential = itinerarySequential,//?
                            RoutingTypeId = 1,//Itinerario del pasajero
                            Origin = "",
                            Destination = "",
                            Airline = NotNull(itinerary.Airline) != "" ? itinerary.Airline.Substring(0, 2) : "",
                            Flight = NotNull(itinerary.FlightNumberOnly),
                            FlightDate = itinerary.ItineraryDateFull.ToString("yyyy-MM-ddTHH:mm:ss.fffZ")
                        }
                    );
            }
            foreach (RushRouting rushRoutingItem in routingList)
            {
                rushRoutingFWD.Add(
                        new RushRoutingFWD()
                        {
                            AirlineCode = rushRoutingItem.AirlineCode,
                            StationCode = rushRoutingItem.StationCode,
                            Sequential = rushRoutingItem.Sequential
                        }
                    );
            }
            List<LogFWD> Logs = new List<LogFWD>
            {
                new LogFWD()
                {
                    RequestID = requestId,
                    LogID = 1,
                    Username = forwardCase.CreatedBy,
                    LogDate = forwardCase.CreatedDate.ToString("yyyy-MM-ddTHH:mm:ss.fffZ"),
                    Comments = "Creación de FWD desde Mobile App"
                }
            };

            CreateFWD newFWD = new CreateFWD()
            {
                bagFileInput = new BagFileInputFWD()
                {
                    RequestID = requestId,
                    RequestType = 2, // Forward
                    SupplementalInformation = NotNull(forwardCase.FurtherInformation),
                    AgentName = (NotNull(forwardCase.AgentLastName) != "") ? forwardCase.AgentLastName : forwardCase.CreatedBy,
                    NumberOfBags = baggageInfo.Count,
                    Airline = (NotNull(forwardCase.AirlineCode) != "") ? forwardCase.AirlineCode.Substring(0, 2) : "",
                    Station = (NotNull(forwardCase.StationCode) == "") ? "" : forwardCase.StationCode.Substring(0, 3),
                    ReasonForLoss = NotNull(forwardCase.ReasonForLostCode) == "" ? 0 : Convert.ToInt32(forwardCase.ReasonForLostCode.Substring(0, 2)),
                    FaultStation = (NotNull(forwardCase.ResponsibleStationCode) != "") ? forwardCase.ResponsibleStationCode.Substring(0, 3) : "",
                    FaultAirline = (NotNull(forwardCase.ResponsibleAirlineCode) != "") ? forwardCase.ResponsibleAirlineCode.Substring(0, 2) : "",
                    FaultTerminal = "",
                    CommentsOnLoss = NotNull(forwardCase.ReasonLostComment),
                    RelatedFileLocatorStation = NotNull(forwardCase.RelatedFileLocatorStation),
                    RelatedFileLocatorAirline = NotNull(forwardCase.RelatedFileLocatorAirline),
                    RelatedFileLocatorSeqNumber = NotNull(forwardCase.RelatedFileLocatorSequenceNumber),
                    BagFileStatus = "ENVIADO",
                    CreationDate = forwardCase.CreatedDate.ToString("yyyy-MM-ddTHH:mm:ss.fffZ"),
                    Passengers = PassengersBPM,
                    Bags = Bags,
                    Flights = Flights,
                    Logs = Logs,
                    Routings = rushRoutingFWD,
                    DestinationPlatform = "",
                    BPMCaseID = 0
                },
                codeDestination = GetLastCopaStation(rushRoutingFWD)
            };

            string jsonToCreateOHD = JsonConvert.SerializeObject(newFWD);


            return await Task.FromResult(jsonToCreateOHD);
        }
        private async Task<string> JsonGeneratorToCreateDPR(DamageReport damageCase)
        {
            List<DprAddress> addresses = new List<DprAddress>();
            string requestId = damageCase.CaseId;
            string baggages = damageCase.Baggages;
            string itineraries = damageCase.Itinerary;
            string userName = Settings.CurrentUser;
            string jsonToCreateDPR = null;
            try
            {
                List<DprPassenger> Passengers = new List<DprPassenger>();
                DprPassenger passengerItem = new DprPassenger()
                {
                    RequestID = requestId,
                    PassengerID = 1,
                    PNR = NotNull(damageCase.RecordLocator),
                    PNames = NotNull(damageCase.LastName),
                    GivenName = NotNull(damageCase.FirstName),
                    Initials = NotNull(damageCase.Initials),
                    Title = NotNull(damageCase.PassengerTitle),
                    FrequentFlyerNumber = NotNull(damageCase.FrequentFlyer),
                    EmailAddress = NotNull(damageCase.EmailAddress),
                    Status = NotNull(damageCase.LoyaltyLevel),
                    dprAddresses = addresses
                };
                //Permanent Address
                DprAddress permanentAddress = new DprAddress()
                {
                    AddressType = 1,
                    Address = damageCase.PermanentAddress,
                    AddressState = NotNull(damageCase.State),
                    AreaCode = ConvertAreaCodeToInt(damageCase.PermanentAreaCode1),
                    City = NotNull(damageCase.City),
                    CountryCode = NotNull(damageCase.Country),
                    PassengerID = 1,
                    Phone1 = NotNull(damageCase.PermanentPhone1),
                    RequestID = requestId,
                    Phone2 = NotNull(damageCase.PermanentPhone2),
                    TypePhone1 = GetPhoneTypeId(damageCase.PhoneType1),
                    TypePhone2 = GetPhoneTypeId(damageCase.PhoneType2),
                    time24hrs = "",
                    ValidityDate = null,
                    ZipCode = damageCase.PostalCode
                };

                //Temporary Address
                DprAddress temporaryAddress = new DprAddress()
                {
                    AddressType = 2,
                    Address = NotNull(damageCase.TemporaryAddress),
                    AddressState = NotNull(damageCase.TemporaryState),
                    AreaCode = ConvertAreaCodeToInt(damageCase.TemporaryAreaCode),
                    City = NotNull(damageCase.TemporaryCity),
                    CountryCode = NotNull(damageCase.TemporaryCountry),
                    PassengerID = 1,
                    Phone1 = NotNull(damageCase.TemporaryPhone),
                    RequestID = requestId,
                    Phone2 = "",
                    TypePhone1 = GetPhoneTypeId(damageCase.TemporaryPhoneType),
                    TypePhone2 = 0,
                    ValidityDate = (damageCase.ValidityDateAddress <= DateTime.Now && !string.IsNullOrEmpty(damageCase.TemporaryAddress)) ? DateTime.Now.ToString("yyyy-MM-ddTHH:mm:ss.fffZ") : (string.IsNullOrEmpty(damageCase.TemporaryAddress)) ? null : Convert.ToDateTime(damageCase.ValidityDateAddress).ToString("yyyy-MM-ddTHH:mm:ss.fffZ"),
                    time24hrs = (damageCase.ValidityTimeAddress.Hours > 0) ? string.Format("{0}{1}", damageCase.ValidityTimeAddress.Hours, damageCase.ValidityTimeAddress.Minutes) : "0",
                    ZipCode = damageCase.PostalCode

                };
                passengerItem.dprAddresses.Add(permanentAddress);
                passengerItem.dprAddresses.Add(temporaryAddress);
                Passengers.Add(passengerItem);

                List<DprBag> Bags = new List<DprBag>();

                List<DPRBaggage> baggageInfo = string.IsNullOrEmpty(baggages) ? null : JsonConvert.DeserializeObject<List<DPRBaggage>>(baggages);


                int bagSequential = 0;

                foreach (DPRBaggage bag in baggageInfo)
                {
                    List<string> bagPhotos = new List<string>();
                    bagSequential = bagSequential + 1;


                    if (bag.BaggagePhotos != null)
                    {
                        string apiApp = Settings.BCAApiApp;
                        string auth = Settings.ApiAuth;

                        foreach (BaggagePhotoItem photos in bag.BaggagePhotos)
                        {
                            bagPhotos.Add(
                                string.Format("{0}/Home/DisplayImage?container=dprphotos&name={1}&auth={2}", apiApp, photos.FileName, auth)
                                );
                        }
                    }

                    BaggageDamageItem firstDamage = bag.BaggageDamages.FirstOrDefault();
                    Bags.Add
                        (
                            new DprBag()
                            {
                                RequestID = requestId,
                                BagID = bagSequential,
                                TagNumber = NotNull(bag.BaggageTagNumber),
                                Airline = NotNull(bag.Carrier),
                                Color = NotNull(bag.ColourType),
                                Type = NotNull(bag.BaggageType),
                                Descriptors = string.Format(@"{0}{1}{2}", bag.DescriptorFirst, bag.DescriptorSecond, bag.DescriptorThird),
                                BrandInformation = NotNull(bag.BrandName),
                                BaggageWeight = bag.BaggageWeight,
                                PassengerID = 1,//Revisar
                                BagImageURL = string.Join(",", bagPhotos),
                                BagStatus = "Reportada",
                                DamageCode = GetDamageTypeCode(firstDamage.DamageType),
                                DamageLocation = GetDamageLocationCode(firstDamage.DamageLocation),
                                DamageSeverity = GetDamageSeverityCode(firstDamage.DamageSeverity),
                                WeightInDelivery = NotNull(bag.BaggageDeliveryWeight),
                                ahlBagSequential = 0
                            }
                        );


                }

                List<Itinerary> itineraryInfo = string.IsNullOrEmpty(itineraries) ? null : JsonConvert.DeserializeObject<List<Itinerary>>(itineraries);

                int itinerarySequential = 0;
                List<DprFlight> Flights = new List<DprFlight>();

                foreach (Itinerary itinerary in itineraryInfo)
                {
                    itinerarySequential++;

                    Flights.Add
                        (
                            new DprFlight
                            {
                                RequestID = requestId,
                                Sequential = itinerarySequential,//?
                                RoutingTypeId = 1,//?
                                Origin = NotNull(itinerary.Departure),
                                Destination = NotNull(itinerary.Arrival),
                                Airline = NotNull(itinerary.Airline),
                                Flight = NotNull(itinerary.FlightNumberOnly),
                                FlightDate = itinerary.ItineraryDateFull.ToString("yyyy-MM-ddTHH:mm:ss.fffZ"),
                                flightArrivalHour = itinerary.FlightArrivalHour
                            }
                        );
                }

                int RL = Convert.ToInt32(damageCase.ReasonForLoss.Split('-')[0].Trim());
                string FS = GetFaultStation(damageCase.FaultStation);
                string RLAndFSLogText = string.Format("RL: {0} y FS: {1}", RL, FS);
                List<DprLog> Logs = new List<DprLog>
                {
                new DprLog()
                {
                    RequestID = requestId,
                    ActivityID = 0,
                    ActivityName = "Registro",
                    LogID = 1,
                    Username = damageCase.CreatedBy,
                    LogDate = damageCase.CreatedDate.ToString("yyyy-MM-ddTHH:mm:ss.fffZ"),
                    Comments = string.Format("Creación de Reporte de daños/saqueo - Mobile App. {0}", RLAndFSLogText)
                }
                };

                CreateDPR newDPR = new CreateDPR()
                {
                    boBagFileInput = new DprBagFileInput()
                    {
                        RequestID = requestId,
                        RequestType = GetRequestType(damageCase.ClaimType),
                        SupplementalInformation = NotNull(damageCase.ComplementaryInfo),
                        InitiatorUserName = damageCase.AgentInitials,
                        Airline = damageCase.Airline.Split('-')[0].Trim(),
                        Station = damageCase.Station.Split('-')[0].Trim(),
                        ReasonForLoss = RL,
                        FaultStation = FS,
                        FaultTerminal = NotNull(damageCase.FaultTerminal),
                        CommentsOnLoss = "",
                        BagFileStatus = "Abierto",
                        ClaimAmount = GetAmountValue(damageCase.ClaimAmount),
                        Affectation = GetAffectationCode(damageCase.AffectationType),
                        ClaimCreationTime = damageCase.CreatedDate,
                        ClaimInitialTime = damageCase.StartTime,
                        CostPayment = 0,
                        LiabilityTag = GetLiabilityTagCode(damageCase.LiabilityTag),
                        InitiatorEmail = "",
                        InitiatorFullName = "",
                        DamagedContents = NotNull(damageCase.DamagedContents),
                        PilferedContents = NotNull(damageCase.PilferedContents),
                        EndDate = null,
                        CreationDate = damageCase.CreatedDate.ToString("yyyy-MM-ddTHH:mm:ss.fffZ"),
                        dprPassengers = Passengers,
                        dprBags = Bags,
                        dprFlights = Flights,
                        dprLogs = Logs,
                        BPMCaseID = 0,
                        WTID = "",
                        manifest = damageCase.Manifest,
                        pnrInformation = damageCase.PNRInformation,
                        ahlid = "",
                        creationType = damageCase.CreationType
                    },
                };

                jsonToCreateDPR = JsonConvert.SerializeObject(newDPR);
            }
            catch (Exception ex)
            {
                var customError = new CustomError(ex);
                var errorString = SerializerUtil.SerializeObject(customError);
                var dprString = SerializerUtil.SerializeObject(damageCase);
                string additionalInfo = string.Format("Error durante construcción de mensaje JSON del DPR {0}. Datos de la excepción: {1}. Datos del DPR: {2}",
                    damageCase.CaseId, errorString, dprString);
                var exception = new ApplicationException(additionalInfo);
                //Crashes.TrackError(exception);

            }
            return await Task.FromResult(jsonToCreateDPR);
        }

        private int ConvertAreaCodeToInt(string areaCodeText)
        {
            if (!string.IsNullOrEmpty(areaCodeText))
            {
                int areaCodeValue;
                var areaCodeDigits = Regex.Replace(areaCodeText, @"\s+", "");
                bool isValid = int.TryParse(areaCodeDigits, out areaCodeValue);
                return (isValid) ? areaCodeValue : 0;
            }
            else
            {
                return 0;
            }

        }

        private string GetFaultStation(string faultStation)
        {
            if (!string.IsNullOrEmpty(faultStation) && faultStation.Length >= 3)
            {
                return (faultStation.Length == 3) ? faultStation.ToUpper() : faultStation.Substring(0, 3).ToUpper();
            }
            else
            {
                return null;
            }
        }

        private int GetAmountValue(string amount)
        {
            int result = 0;
            var integerPart = amount.Contains(".") ? amount.Substring(0, amount.IndexOf(".", 0)) : amount;
            bool parsed = int.TryParse(integerPart, out result);
            return (parsed) ? result : 0;
        }

        private string GetDamageTypeCode(string damageType)
        {
            string code = Regex.Match(damageType, @"\d+").Value;
            return code;
        }

        private string GetDamageSeverityCode(string damageSeverity)
        {
            string result = string.Empty;
            switch (damageSeverity)
            {
                case "Total":
                    result = "TL";
                    break;
                case "Major":
                    result = "MA";
                    break;
                case "Minor":
                    result = "MI";
                    break;
            }
            return result;
        }

        private string GetDamageLocationCode(string damageLocation)
        {
            string result = string.Empty;
            switch (damageLocation)
            {
                case "Top":
                    result = "TOP";
                    break;
                case "Bottom":
                    result = "BOTTOM";
                    break;
                case "End":
                    result = "END";
                    break;
                case "Side":
                    result = "SIDE";
                    break;
            }
            return result;
        }

        private int GetPhoneTypeId(string phoneType)
        {
            int result = 0;
            switch (phoneType)
            {
                case "Residencial":
                    result = 1;
                    break;
                case "Celular":
                    result = 2;
                    break;
            }
            return result;
        }

        private int GetRequestType(string claimType)
        {
            int result = 0;
            switch (claimType)
            {
                case "Daño":
                    result = 1;
                    break;
                case "Saqueo":
                    result = 2;
                    break;
                case "Ambos":
                    result = 3;
                    break;
            }
            return result;
        }

        private string GetLiabilityTagCode(string liabilityTag)
        {
            string result = string.Empty;
            switch (liabilityTag)
            {
                case "Si":
                    result = "Y";
                    break;
                case "No":
                    result = "N";
                    break;
            }
            return result;
        }

        private string GetAffectationCode(string affectationType)
        {
            string result = "0";
            switch (affectationType)
            {
                case "Aplica (DPRU)":
                    result = "1";
                    break;
                case "No Aplica (DPRU2)":
                    result = "2";
                    break;
            }
            return result;
        }

        private string GetLastCopaStation(List<RushRoutingFWD> rushRoutingFWD)
        {
            var lastCopaStation = rushRoutingFWD.Where(x => x.AirlineCode == "CM").LastOrDefault();
            return (lastCopaStation == null) ? "" : lastCopaStation.StationCode;
        }

        private async Task<string> JsonGeneratorToCreate(PIR baggageCase)
        {
            string caseId = baggageCase.CaseId;
            string baggages = baggageCase.Baggages;
            string itineraries = baggageCase.Itinerary;

            List<BoBagsInput> bagList = new List<BoBagsInput>();

            List<Baggage> baggageInfo = string.IsNullOrEmpty(baggages) ? null : JsonConvert.DeserializeObject<List<Baggage>>(baggages);

            int contentSequential = 0;
            int bagSequential = 0;
            List<BoBagContentsInput> bagContents = new List<BoBagContentsInput>();

            string phoneType1 = baggageCase.PhoneType1;
            string phoneType2 = baggageCase.PhoneType2;
            string temporaryPhoneType = baggageCase.TemporaryPhoneType;

            foreach (Baggage bag in baggageInfo)
            {
                bagSequential++;

                bagList.Add
                    (
                        new BoBagsInput
                        {
                            ProcessBagID = caseId,
                            TagNumber = NotNull(bag.BaggageTagNumber),
                            BagSequential = bagSequential,
                            Airline = NotNull(bag.Carrier),
                            Color = NotNull(bag.ColourType),
                            Type = NotNull(bag.BaggageType),
                            Descriptors = string.Format(@"{0}{1}{2}", bag.DescriptorFirst, bag.DescriptorSecond, bag.DescriptorThird),
                            BrandInformation = NotNull(bag.BrandName),
                            DeliveryDate = DateTime.Now.ToString("yyyy-MM-ddTHH:mm:ss.fffZ"),
                            Time24HR = 0,
                            LocalDeliveryInformation = NotNull(bag.LocalDelivery != null && bag.LocalDelivery.Equals("[Seleccione la dirección de entrega]") ? "" : bag.LocalDelivery),
                            LocationWhereBagLastSeen = NotNull(bag.BagLastSeen),
                            ContentsGender = string.IsNullOrEmpty(bag.ContentsGender) ? 0 : bag.ContentsGender.Equals("Female") ? 1 : 2,//?
                            DestinationBaggageClaimCheck = bag.DestinationBagTag, //?
                            bagFound = bag.BagFound,
                            caseRelatedID = NotNull(bag.CaseRelatedID),
                            caseRelatedBagId = bag.CaseRelatedBagId,
                            matchFileType = "",
                            matchSystemSource = ""
                        }
                    );

                if (bag.BaggageContents != null)
                {
                    foreach (BaggageContentItem content in bag.BaggageContents)
                    {
                        contentSequential++;

                        bagContents.Add
                            (
                                new BoBagContentsInput
                                {
                                    ProcessBagID = caseId,
                                    BagSequential = bagSequential.ToString(),
                                    Sequential = contentSequential,
                                    Category = NotNull(content.CategoryId),
                                    Description1 = NotNull(content.ContentDescription),
                                    Description2 = NotNull(content.ContentDescription),
                                    CategoryWeight = content.CategoryWeight
                                }
                            );
                    }
                }
            }

            List<Itinerary> itineraryInfo = string.IsNullOrEmpty(itineraries) ? null : JsonConvert.DeserializeObject<List<Itinerary>>(itineraries);

            int itinerarySequential = 0;
            List<BoFlightsInput> itineraryList = new List<BoFlightsInput>();

            foreach (Itinerary itinerary in itineraryInfo)
            {
                itinerarySequential++;

                itineraryList.Add
                    (
                        new BoFlightsInput
                        {
                            ProcessBagID = caseId,
                            Sequential = itinerarySequential,//?
                            RoutingTypeId = itinerarySequential,//?
                            Origin = NotNull(itinerary.Departure),
                            Destination = NotNull(itinerary.Arrival),
                            Airline = NotNull(itinerary.Airline),
                            Flight = NotNull(itinerary.FlightNumberOnly),
                            FDate = itinerary.ItineraryDateFull.ToString("yyyy-MM-ddTHH:mm:ss.fffZ"),
                            flightArrivalHour = itinerary.FlightArrivalHour
                        }
                    );
            }


            CreateCaseBpm newCase = new CreateCaseBpm()
            {
                boBagFileInput = new BoBagFileInput()
                {
                    ProcessBagID = caseId,
                    PNR = baggageCase.RecordLocator,
                    PNames = baggageCase.LastName.Length > 16 ? baggageCase.LastName.Substring(0, 16) : baggageCase.LastName,
                    GivenName = baggageCase.Name.Length > 16 ? baggageCase.Name.Substring(0, 16) : baggageCase.Name,
                    Initials = baggageCase.Initials,
                    Title = baggageCase.PassengerTitle,
                    ClassServiceStatus = baggageCase.CabinClass,
                    Status = NotNull(baggageCase.PassengerStatus),
                    FrequentFlyerNumber = string.Format("{0}{1}", NotNull(baggageCase.Carrier), NotNull(baggageCase.FrequentFlyer)),
                    LocationWhereBagLastSeen = "",//?
                    ContentsGender = 0,//?
                    DestinationBaggageClaimCheck = "",
                    manifest = baggageCase.Manifest,
                    pnrInformation = baggageCase.PNRInformation
                },

                boPassengerPermanentAddressInput = new BoPassengerPermanentAddressInput()
                {
                    ProcessBagID = caseId,
                    PermanentAddress = NotNull(baggageCase.PermanentAddress),
                    City = NotNull(baggageCase.City),
                    PState = NotNull(baggageCase.State),
                    ZipCode = NotNull(baggageCase.PostalCode),
                    CountryCode = NotNull(baggageCase.CountryCode),
                    AreaCode = !string.IsNullOrEmpty(baggageCase.PermanentAreaCode1) ? int.Parse(baggageCase.PermanentAreaCode1.Remove(0, 1).Replace(" ", "")) : 0,
                    HomeBusinessPhone1 = (phoneType1.Equals("Residencial") ? NotNull(baggageCase.PermanentPhone1) : ""),
                    HomeBusinessPhone2 = (phoneType2.Equals("Residencial") ? NotNull(baggageCase.PermanentPhone2) : ""),
                    CellMobilePhone1 = (phoneType1.Equals("Celular") ? NotNull(baggageCase.PermanentPhone1) : ""),
                    CellMobilePhone1AreaCode = !string.IsNullOrEmpty((phoneType1.Equals("Celular") ? baggageCase.PermanentPhone1 : "")) ? int.Parse(baggageCase.PermanentAreaCode1.Remove(0, 1).Replace(" ", "")) : 0,
                    CellMobilePhone2 = (phoneType2.Equals("Celular") ? NotNull(baggageCase.PermanentPhone2) : ""),
                    CellMobilePhone2AreaCode = !string.IsNullOrEmpty((phoneType2.Equals("Celular") ? baggageCase.PermanentPhone2 : "")) ? int.Parse(baggageCase.PermanentAreaCode2.Remove(0, 1).Replace(" ", "")) : 0,
                    EmailAddress1 = NotNull(baggageCase.EmailAddress),
                    Fax1 = 0,
                    PLanguage = NotNull(baggageCase.Language.Equals("[Seleccione]") ? "" : baggageCase.Language)
                },

                boPassengerTemporaryAddressInput = new BoPassengerTemporaryAddressInput()
                {
                    ProcessBagID = caseId,
                    Id = 1,
                    TemporaryAddress = NotNull(baggageCase.TemporaryAddress),
                    City = NotNull(baggageCase.TemporaryCity),
                    TState = NotNull(baggageCase.TemporaryState),
                    ZipCode = NotNull(baggageCase.TemporaryPostalCode),
                    CountryCode = NotNull(baggageCase.TemporaryCountryCode),//NotNull(baggageCase.TemporaryCountry),
                    AreaCode = !string.IsNullOrEmpty(baggageCase.TemporaryAreaCode) ? int.Parse(baggageCase.TemporaryAreaCode.Remove(0, 1).Replace(" ", "")) : 0,
                    ValidityDate = (baggageCase.ValidityDateAddress <= DateTime.Now && !string.IsNullOrEmpty(baggageCase.TemporaryAddress)) ? DateTime.Now.ToString("yyyy-MM-ddTHH:mm:ss.fffZ") : (string.IsNullOrEmpty(baggageCase.TemporaryAddress)) ? null : Convert.ToDateTime(baggageCase.ValidityDateAddress).ToString("yyyy-MM-ddTHH:mm:ss.fffZ"),
                    Time24HR = !string.IsNullOrEmpty(baggageCase.ValidityTimeAddress) ? int.Parse(baggageCase.ValidityTimeAddress.Replace(":", "")) : 0,//?baggageCase.ValidityTimeAddress
                    Phone1 = (temporaryPhoneType.Equals("Residencial") ? NotNull(baggageCase.TemporaryPhone) : ""),
                    Phone2 = (temporaryPhoneType.Equals("Celular") ? NotNull(baggageCase.TemporaryPhone) : ""),
                    EntregaLocal = !string.IsNullOrEmpty(baggageCase.LocalDelivery) ? baggageCase.LocalDelivery : "Dirección permanente"
                },

                boBagsInput = bagList,
                boBagContentsInput = bagContents,
                boFlightsInput = itineraryList,

                boClaimInput = new BoClaimInput()
                {
                    ProcessBagID = caseId,
                    Reasonforloss = int.Parse(baggageCase.ReasonForLossCode),
                    FaultStation = baggageCase.ResponsibleStation,
                    SupplementalInformation = NotNull(baggageCase.AdditionalInformation)
                },
                boProcessBagFilesInput = new BoProcessBagFilesInput()
                {
                    ProcessBagID = caseId,
                    InitiatorUserName = baggageCase.CreatedBy,
                    claimInitialTime = baggageCase.CreatedDate.ToString("yyyy-MM-ddTHH:mm:ss.fffZ"),
                    claimCreationTime = baggageCase.SavedDate.ToString("yyyy-MM-ddTHH:mm:ss.fffZ"),
                    FWDRelated = baggageCase.FWDReferenceId,
                    OHDRelated = baggageCase.OHDReferenceId,
                    ClaimStation = baggageCase.StationCreatedBy,
                    initSearchDecision = baggageCase.SearchDecisionStatus,
                    creationType = baggageCase.CreationTypeOrigin
                }

            };

            string jsonToCreateCase = JsonConvert.SerializeObject(newCase);


            return await Task.FromResult(jsonToCreateCase);
        }
        public async Task<bool> GetSessionContext()
        {
            if (CookiesContainer == null)
            {
                Settings.BPMToken = "";
                return await Task.FromResult(false);
            }

            string url = BpmUrl;

            var handler = new HttpClientHandler
            {
                CookieContainer = CookiesContainer
            };

            string sessionContextPath = @"/bonita/API/system/session/1";
            bool sessionAlive = false;

            using (var client = new HttpClient(handler))
            {
                var uri = new Uri(url);
                client.BaseAddress = uri;
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                using (HttpResponseMessage response = await client.GetAsync(sessionContextPath))
                {

                    if (response.IsSuccessStatusCode)
                    {
                        var responseBodyAsText = await response.Content.ReadAsStringAsync();

                        if (!string.IsNullOrEmpty(responseBodyAsText))
                        {
                            BpmSessionContext sessionContext = (BpmSessionContext)JsonConvert.DeserializeObject(responseBodyAsText, typeof(BpmSessionContext));
                            sessionAlive = sessionContext != null;
                        }

                    }

                    return await Task.FromResult(sessionAlive);

                }
            }
        }

        private string NotNull(string campo)
        {
            return (string.IsNullOrEmpty(campo) || string.IsNullOrWhiteSpace(campo)) ? "" : campo;
        }

        public async Task<AHLDelayedBaggageDTO> GetAHLDetailsById(string processBagId)
        {
            List<AHLDelayedBaggageDTO> AHLList = null;
            AHLDelayedBaggageDTO AHLFileItem = null;
            string getAHLByIdPath = string.Format(@"GetAHLByID(ProcessBagID='{0}')", processBagId);
            try
            {
                APIResponse response = await ApiClient.ExecuteAPICallSimple(Settings.BCAApiApp, getAHLByIdPath, new Dictionary<string, string>());
                if (response != null && response.IsSuccessful)
                {
                    AHLList = JsonConvert.DeserializeObject<List<AHLDelayedBaggageDTO>>(response.Data);
                }
            }
            catch (Exception)
            {
                throw;
            }
            if (AHLList != null && AHLList.Count > 0)
            {
                AHLFileItem = AHLList.FirstOrDefault();
            }
            return AHLFileItem;
        }

        public async Task<List<DPRResumeInfoDTO>> GetDPRMatchesByCriteria(string PNR, string bagTagNumbers)
        {
            List<DPRResumeInfoDTO> dPRList = null;
            string getDPRByCriteria = string.Format(@"GetDPRByCriteria(PNR='{0}', BagTagNumbers='{1}')", PNR, bagTagNumbers);
            try
            {
                APIResponse response = await ApiClient.ExecuteAPICallSimple(Settings.BCAApiApp, getDPRByCriteria, new Dictionary<string, string>());
                if (response != null && response.IsSuccessful)
                {
                    var container = JsonConvert.DeserializeObject<DPRMatchesResponse>(response.Data);
                    dPRList = container.DPRMatchesList;
                }
            }
            catch (Exception)
            {
                throw;
            }

            return dPRList;
        }

        public async Task<bool> RelateAHLWithFWD(PIR baggageCase)
        {
            bool result = false;
            try
            {
                if (baggageCase.IsCreatedFromFWD)
                {
                    FWDBagsAHLRelationDTO message = CreateJSONMessage(baggageCase);
                    string requestContent = JsonConvert.SerializeObject(message);
                    string pathname = "ChangeFWDBagsValues";
                    APIResponse response = await ApiClient.ExecutePOSTAPICallSimple(Settings.BCAApiApp, pathname, requestContent, new Dictionary<string, string>());
                    if (response != null && response.IsSuccessful)
                    {
                        result = JsonConvert.DeserializeObject<bool>(response.Data);
                    }
                }
            }
            catch (Exception ex)
            {
                var properties = new Dictionary<string, string>
                {
                    { "BpmApiService", "RelateAHLWithFWD" }
                };
                //Crashes.TrackError(ex, properties);
            }

            return result;
        }

        private FWDBagsAHLRelationDTO CreateJSONMessage(PIR baggageCase)
        {
            FWDBagsAHLRelationDTO requestMessage = new FWDBagsAHLRelationDTO();
            var baggage = JsonConvert.DeserializeObject<List<Baggage>>(baggageCase.Baggages);
            var baggageSelected = baggage.Where(x => x.BagFound).ToList();
            requestMessage.FWDBags = new List<MFBagsDTO>();
            foreach (Baggage bagItem in baggageSelected)
            {
                MFBagsDTO newBagItem = new MFBagsDTO()
                {
                    AIRLINE = bagItem.Carrier,
                    TAGNUMBER = bagItem.BaggageTagNumber
                };
                requestMessage.FWDBags.Add(newBagItem);
                requestMessage.FWDRequestNumber = bagItem.CaseRelatedID;
            }
            requestMessage.AHLRequestNumber = baggageCase.CaseId;
            requestMessage.RelatedFileLocator = true;
            requestMessage.RelatedFileLocatorSeqNumber = baggageCase.CaseId;
            requestMessage.RelatedFileLocatorStation = baggageCase.StationCreatedBy;
            requestMessage.RelatedFileLocatorAirline = "CM";

            return requestMessage;
        }

    }
}
