using Plugin.Settings;
using Plugin.Settings.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BaggageApp.Helpers
{
    public static class Settings
    {
        private static ISettings AppSettings
        {
            get
            {
                return CrossSettings.Current;
            }
        }
        #region Setting Constants
        private const string UnsubscribeMessangingKey = "unsubscribemessanging_key";
        private static readonly bool UnsubscribeMessangingDefault = false;
        private const string SettingsKey = "settings_key";
        private static readonly string SettingsDefault = string.Empty;
        private const string ArrivalStationKey = "arrivalstation_key";
        private static readonly string ArrivalStationDefault = string.Empty;
        private const string FlightsEndpointKey = "flightsendpoint_key";
        private static readonly string FlightsEndpointDefault = string.Empty;
        private const string FlightsSubscriptionKey = "flightssubscription_key";
        private static readonly string FlightsSubscriptionDefault = string.Empty;
        private const string BCAIncrementalKey = "bcaincremental_key";
        private static readonly string BCAIncrementalDefault = string.Empty;
        private const string OHDIncrementalKey = "ohdincremental_key";
        private static readonly string OHDIncrementalDefault = string.Empty;
        private const string FWDIncrementalKey = "fwdincremental_key";
        private static readonly string FWDIncrementalDefault = string.Empty;
        private const string DPRIncrementalKey = "dprincremental_key";
        private static readonly string DPRIncrementalDefault = string.Empty;
        private const string BCAApiAppKey = "bcaapiapp_key";
        private static readonly string BCAApiAppDefault = "https://api-copa-bca-qa.azurewebsites.net/";
        private const string BonitaBPMKey = "bonitabpm_key";
        private static readonly string BonitaBPMDefault = "http://azr-qa-bbpm-01.eastus.cloudapp.azure.com:8080/bonita/";
        private const string BCAApiAppCleanKey = "bcaapiappclean_key";
        private static readonly string BCAApiAppCleanDefault = "azr-qa-bbpm-01.eastus.cloudapp.azure.com";
        private const string BCAApiAppPortKey = "bcaapiappport_key";
        private static readonly int BCAApiAppPortDefault = 8080;
        //-----------------ADAL Settings-------------------//
        private const string AppServiceURLKey = "appserviceurl_key";
        private static readonly string AppServiceURLDefault = "https://bcaadal.azurewebsites.net/";
        private const string TenantIdKey = "tenantid_key";
        private static readonly string TenantIdDefault = "https://login.microsoftonline.com/copadev.onmicrosoft.com";
        private const string ResourceIdKey = "resourceid_key";
        private static readonly string ResourceIdDefault = "99d2857d-e04e-4011-bef3-3012dbdb591f";
        private const string ClientIdKey = "clientid_key";
        private static readonly string ClientIdDefault = "e8deab77-abfa-4987-bd81-552b82b3561b";
        private const string ReturnUrlKey = "returnurl_key";
        private static readonly string ReturnUrlDefault = "https://bcaadal.azurewebsites.net/";
        private const string GraphResourceUriKey = "graphresourceuri_key";
        private static readonly string GraphResourceUriDefault = "https://graph.microsoft.com/";
        private const string AuthorizedGroupKey = "authorizedgroup_key";
        private static readonly string AuthorizedGroupDefault = "BCAUsers";
        private const string CurrentUserKey = "currentuser_key";
        private static readonly string CurrentUserDefault = "";
        private const string AgentFullNameKey = "agentfullname_key";
        private static readonly string AgentFullNameDefault = "";
        private const string StorageConnectionKey = "storageconnection_key";
        private static readonly string StorageConnectionDefault = "DefaultEndpointsProtocol=https;AccountName=bcastoragetst;AccountKey=A8b0b8jKLa2GUGXbNRVKh5Bvd11/2qPEzu3UNM2RrDQ4iw0V7CmcxZmdEWbadkenp8BciStW/r+OtO+pdmBqrA==;EndpointSuffix=core.windows.net";
        private const string ContainerTypeKey = "containertype_key";
        private static readonly string ContainerTypeDefault = "ohdphotos";
        private const string BPMTokenKey = "bpmtoken_key";
        private static readonly string BPMTokenDefault = "";
        private const string ApiManagementKey = "apimanagement_key";
        private static readonly string ApiManagementDefault = "developer-uat.copa.com";
        private const string ApiAuthKey = "apiauth_key";
        private static readonly string ApiAuthDefault = "QkNBOjEyMzQ1Ng==";
        private const string AHLProcessKey = "ahlprocess_key";
        private static readonly string AHLProcessDefault = "Baggage Service Automation";
        private const string OHDProcessKey = "ohdprocess_key";
        private static readonly string OHDProcessDefault = "Baggage Service Request";
        private const string BPMSessionIDKey = "bpmsessionid_key";
        private static readonly string BPMSessionIDDefault = "";
        private const string DPRProcessKey = "dprprocess_key";
        private static readonly string DPRProcessDefault = "Damaged and Pilferred Bag";
        private const string TestConnectionURLKey = "testconnectionurl_key";
        private static readonly string TestConnectionURLDefault = "www.google.com";
        private const string IsAuthenticatedKey = "isauthenticated_key";
        private static readonly bool IsAuthenticatedDefault = false;
        private const string IsConnectedKey = "isconnected_key";
        private static readonly bool IsConnectedDefault = false;
        private const string ShowUnAuthorizedMessageKey = "showUnAuthorizedMessage_key";
        private static readonly bool ShowUnAuthorizedMessageDefault = false;
        //----------------ADAL Settings------------------//
        #endregion
        //----------------ADAL Settings------------------//
        public static string AppServiceURL
        {
            get
            {
                return AppSettings.GetValueOrDefault(AppServiceURLKey, AppServiceURLDefault);
            }
            set
            {
                AppSettings.AddOrUpdateValue(AppServiceURLKey, value);
            }
        }
        public static string TenantId
        {
            get
            {
                return AppSettings.GetValueOrDefault(TenantIdKey, TenantIdDefault);
            }
            set
            {
                AppSettings.AddOrUpdateValue(TenantIdKey, value);
            }
        }
        public static string ResourceId
        {
            get
            {
                return AppSettings.GetValueOrDefault(ResourceIdKey, ResourceIdDefault);
            }
            set
            {
                AppSettings.AddOrUpdateValue(ResourceIdKey, value);
            }
        }
        public static string ClientId
        {
            get
            {
                return AppSettings.GetValueOrDefault(ClientIdKey, ClientIdDefault);
            }
            set
            {
                AppSettings.AddOrUpdateValue(ClientIdKey, value);
            }
        }
        public static string ReturnUrl
        {
            get
            {
                return AppSettings.GetValueOrDefault(ReturnUrlKey, ReturnUrlDefault);
            }
            set
            {
                AppSettings.AddOrUpdateValue(ReturnUrlKey, value);
            }
        }
        public static string GraphResourceUri
        {
            get
            {
                return AppSettings.GetValueOrDefault(GraphResourceUriKey, GraphResourceUriDefault);
            }
            set
            {
                AppSettings.AddOrUpdateValue(GraphResourceUriKey, value);
            }
        }
        public static string AuthorizedGroup
        {
            get
            {
                return AppSettings.GetValueOrDefault(AuthorizedGroupKey, AuthorizedGroupDefault);
            }
            set
            {
                AppSettings.AddOrUpdateValue(AuthorizedGroupKey, value);
            }
        }
        public static string CurrentUser
        {
            get
            {
                return AppSettings.GetValueOrDefault(CurrentUserKey, CurrentUserDefault);
            }
            set
            {
                AppSettings.AddOrUpdateValue(CurrentUserKey, value);
            }
        }
        //----------------ADAL Settings------------------//
        public static bool UnsubscribeMessanging
        {
            get
            {
                return AppSettings.GetValueOrDefault(UnsubscribeMessangingKey, UnsubscribeMessangingDefault);
            }
            set
            {
                AppSettings.AddOrUpdateValue(UnsubscribeMessangingKey, value);
            }
        }
        public static string GeneralSettings
        {
            get
            {
                return AppSettings.GetValueOrDefault(SettingsKey, SettingsDefault);
            }
            set
            {
                AppSettings.AddOrUpdateValue(SettingsKey, value);
            }
        }
        public static string ArrivalStation
        {
            get
            {
                return AppSettings.GetValueOrDefault(ArrivalStationKey, ArrivalStationDefault);
            }
            set
            {
                AppSettings.AddOrUpdateValue(ArrivalStationKey, value);
            }
        }
        public static string FlightsEndpoint
        {
            get
            {
                return AppSettings.GetValueOrDefault(FlightsEndpointKey, FlightsEndpointDefault);
            }
            set
            {
                AppSettings.AddOrUpdateValue(FlightsEndpointKey, value);
            }
        }
        public static string FlightsSubscription
        {
            get
            {
                return AppSettings.GetValueOrDefault(FlightsSubscriptionKey, FlightsSubscriptionDefault);
            }
            set
            {
                AppSettings.AddOrUpdateValue(FlightsSubscriptionKey, value);
            }
        }
        public static string BCAIncremenal
        {
            get
            {
                return AppSettings.GetValueOrDefault(BCAIncrementalKey, BCAIncrementalDefault);
            }
            set
            {
                AppSettings.AddOrUpdateValue(BCAIncrementalKey, value);
            }
        }
        public static string OHDIncremenal
        {
            get
            {
                return AppSettings.GetValueOrDefault(OHDIncrementalKey, OHDIncrementalDefault);
            }
            set
            {
                AppSettings.AddOrUpdateValue(OHDIncrementalKey, value);
            }
        }
        public static string FWDIncremenal
        {
            get
            {
                return AppSettings.GetValueOrDefault(FWDIncrementalKey, FWDIncrementalDefault);
            }
            set
            {
                AppSettings.AddOrUpdateValue(FWDIncrementalKey, value);
            }
        }
        public static string BCAApiApp
        {
            get
            {
                return AppSettings.GetValueOrDefault(BCAApiAppKey, BCAApiAppDefault);
            }
            set
            {
                AppSettings.AddOrUpdateValue(BCAApiAppKey, value);
            }
        }
        public static string BonitaBPM
        {
            get
            {
                return AppSettings.GetValueOrDefault(BonitaBPMKey, BonitaBPMDefault);
            }
            set
            {
                AppSettings.AddOrUpdateValue(BonitaBPMKey, value);
            }
        }
        public static string AgentFullName
        {
            get
            {
                return AppSettings.GetValueOrDefault(AgentFullNameKey, AgentFullNameDefault);
            }
            set
            {
                AppSettings.AddOrUpdateValue(AgentFullNameKey, value);
            }
        }
        public static string StorageConnection
        {
            get
            {
                return AppSettings.GetValueOrDefault(StorageConnectionKey, StorageConnectionDefault);
            }
            set
            {
                AppSettings.AddOrUpdateValue(StorageConnectionKey, value);
            }
        }
        public static string ContainerType
        {
            get
            {
                return AppSettings.GetValueOrDefault(ContainerTypeKey, ContainerTypeDefault);
            }
            set
            {
                AppSettings.AddOrUpdateValue(ContainerTypeKey, value);
            }
        }
        public static string BPMToken
        {
            get
            {
                return AppSettings.GetValueOrDefault(BPMTokenKey, BPMTokenDefault);
            }
            set
            {
                AppSettings.AddOrUpdateValue(BPMTokenKey, value);
            }
        }
        public static string BPMSessionID
        {
            get
            {
                return AppSettings.GetValueOrDefault(BPMSessionIDKey, BPMSessionIDDefault);
            }
            set
            {
                AppSettings.AddOrUpdateValue(BPMSessionIDKey, value);
            }
        }
        public static string ApiManagement
        {
            get
            {
                return AppSettings.GetValueOrDefault(ApiManagementKey, ApiManagementDefault);
            }
            set
            {
                AppSettings.AddOrUpdateValue(ApiManagementKey, value);
            }
        }
        public static string BCAApiAppClean
        {
            get
            {
                return AppSettings.GetValueOrDefault(BCAApiAppCleanKey, BCAApiAppCleanDefault);
            }
            set
            {
                AppSettings.AddOrUpdateValue(BCAApiAppCleanKey, value);
            }
        }
        public static int BCAApiAppPort
        {
            get
            {
                return AppSettings.GetValueOrDefault(BCAApiAppPortKey, BCAApiAppPortDefault);
            }
            set
            {
                AppSettings.AddOrUpdateValue(BCAApiAppPortKey, value);
            }
        }
        public static string ApiAuth
        {
            get
            {
                return AppSettings.GetValueOrDefault(ApiAuthKey, ApiAuthDefault);
            }
            set
            {
                AppSettings.AddOrUpdateValue(ApiAuthKey, value);
            }
        }
        public static string AHLProcess
        {
            get
            {
                return AppSettings.GetValueOrDefault(AHLProcessKey, AHLProcessDefault);
            }
            set
            {
                AppSettings.AddOrUpdateValue(AHLProcessKey, value);
            }
        }
        public static string OHDProcess
        {
            get
            {
                return AppSettings.GetValueOrDefault(OHDProcessKey, OHDProcessDefault);
            }
            set
            {
                AppSettings.AddOrUpdateValue(OHDProcessKey, value);
            }
        }
        public static string DPRProcess
        {
            get
            {
                return AppSettings.GetValueOrDefault(DPRProcessKey, DPRProcessDefault);
            }
            set
            {
                AppSettings.AddOrUpdateValue(DPRProcessKey, value);
            }
        }
        public static string DPRIncremental
        {
            get
            {
                return AppSettings.GetValueOrDefault(DPRIncrementalKey, DPRIncrementalDefault);
            }
            set
            {
                AppSettings.AddOrUpdateValue(DPRIncrementalKey, value);
            }
        }
        public static string TestConnectionDefaultURL
        {
            get
            {
                return AppSettings.GetValueOrDefault(TestConnectionURLKey, TestConnectionURLDefault);
            }
            set
            {
                AppSettings.AddOrUpdateValue(TestConnectionURLKey, value);
            }
        }
        public static bool IsAuthenticated
        {
            get
            {
                return AppSettings.GetValueOrDefault(IsAuthenticatedKey, IsAuthenticatedDefault);
            }
            set
            {
                AppSettings.AddOrUpdateValue(IsAuthenticatedKey, value);
            }
        }
        public static bool IsConnected
        {
            get
            {
                return AppSettings.GetValueOrDefault(IsConnectedKey, IsConnectedDefault);
            }
            set
            {
                AppSettings.AddOrUpdateValue(IsConnectedKey, value);
            }
        }
        public static bool ShowUnAuthorizedMessage
        {
            get
            {
                return AppSettings.GetValueOrDefault(ShowUnAuthorizedMessageKey, ShowUnAuthorizedMessageDefault);
            }
            set
            {
                AppSettings.AddOrUpdateValue(ShowUnAuthorizedMessageKey, value);
            }
        }
    }
}
