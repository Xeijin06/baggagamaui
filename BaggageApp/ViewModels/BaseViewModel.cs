using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BaggageApp.Services;
using BaggageApp.Helpers;
using BaggageApp.Models;
using BaggageApp.DataStores;
using BaggageApp.Models.DisplayBookingProxy;
using static BaggageApp.Helpers.StaticListTypes;
using BaggageApp.Models.BagDetails;
using BaggageApp.Autopopulate;

namespace BaggageApp.ViewModels
{
    public class BaseViewModel : ObservableObject
    {
        #region Constants
        public const string baggageClaimCreationEvent = "Baggage Claim File Created";
        #endregion

        #region Private members variables
        private const string onlineMessage = @"El {0} {1} fue creado exitosamente.";
        private string offlineMessage = "El {0} {1} fue guardado localmente, pero no sincronizará hasta que tenga conexión a internet.";
        private string unAuthenticatedMessage = "El {0} {1} fue guardado localmente, pero no sincronizará hasta que haga login nuevamente.";
        private IBagTagService _bagTagService;
        private Passenger _passengerSelected;
        #endregion

        public IDataStore<Models.Flight> FlightDataStore => DependencyService.Get<IDataStore<Models.Flight>>();

        #region Properties

        public string OnlineMessage
        {
            get
            {
                return onlineMessage;
            }
        }
        public string OfflineMessage
        {
            get
            {
                return offlineMessage;
            }
        }
        public string UnAuthenticatedMessage
        {
            get
            {
                return unAuthenticatedMessage;
            }
        }


        bool isBusy = false;
        public bool IsBusy
        {
            get { return isBusy; }
            set { SetProperty(ref isBusy, value); }
        }

        bool isRefreshing = false;
        public bool IsRefreshing
        {
            get { return isRefreshing; }
            set { SetProperty(ref isRefreshing, value); }
        }

        string title = string.Empty;
        public string Title
        {
            get { return title; }
            set { SetProperty(ref title, value); }
        }

        private IConnectionStatus _connectionManager;
        public IConnectionStatus ConnenctionManager
        {
            get
            {
                if (_connectionManager == null)
                {
                    _connectionManager = new ConnectionStatus();
                }
                return _connectionManager;
            }
            set
            {
                _connectionManager = value;
            }
        }

        private ObservableRangeCollection<Itinerary> _itineraryList;

        public ObservableRangeCollection<Itinerary> ItineraryList
        {
            get
            {
                if (_itineraryList == null)
                {
                    _itineraryList = new ObservableRangeCollection<Itinerary>();
                }
                return _itineraryList;
            }
            set
            {
                SetProperty(ref _itineraryList, value);
            }
        }
        private AirReservationDetail _reservationInfo = null;
        public AirReservationDetail ReservationDetail
        {
            get
            {
                return _reservationInfo;
            }
            set
            {
                _reservationInfo = value;
            }
        }

        public Passenger PassengerSelected
        {
            get
            {
                return _passengerSelected;
            }
            set
            {
                SetProperty(ref _passengerSelected, value);
            }
        }

        public IBagTagService BagTagServiceClient
        {
            get
            {
                if (_bagTagService == null)
                {
                    _bagTagService = new BagTagService();
                }
                return _bagTagService;
            }
            set { _bagTagService = value; }
        }
        #endregion

        #region Construsctor, destructor and finalizer
        public BaseViewModel()
        {

        }

        public async Task<bool> GetConnectionStatus()
        {
            int port = 80;
            string urlTestConnection = Settings.TestConnectionDefaultURL;
            bool isConnected = await ConnenctionManager.ConnectionEnabled(Settings.TestConnectionDefaultURL, port, false);
            return isConnected;
        }

        public async Task SendSuccessMessageAsync(ReportType fileType, string bcaRecord, bool isConnected, bool isAuthenticated)
        {
            const string titleMessage = "Registro exitoso";
            const string closeButtonLabel = "Cerrar";
            const string makeLoginButtonLabel = "Hacer Login";
            if (isConnected)
            {
                if (isAuthenticated)
                {
                    await Application.Current.MainPage.DisplayAlert(titleMessage, string.Format(OnlineMessage, fileType.ToString(), bcaRecord), closeButtonLabel);
                    await GoToHome();
                }
                else
                {
                    bool makeLogin = await Application.Current.MainPage.DisplayAlert(titleMessage, string.Format(UnAuthenticatedMessage, fileType.ToString(), bcaRecord), makeLoginButtonLabel, closeButtonLabel);
                    if (makeLogin)
                    {
                        Application.Current.MainPage = new MainPage();
                    }
                    else
                    {
                        await GoToHome();
                    }
                }
            }
            else
            {
                await Application.Current.MainPage.DisplayAlert(titleMessage, string.Format(OfflineMessage, fileType.ToString(), bcaRecord), closeButtonLabel);
                await GoToHome();
            }
        }

        protected async Task SendErrorMessage(CreationMethodType creationType, string PNR, Exception ex, string errorText)
        {
            string commonErrorMessage = "Revise el código de PNR ingresado e intente de nuevo.";
            if (ex != null)
            {
                commonErrorMessage = string.Format("Ocurrió un error inesperado durante la búsqueda de información relacionada a la reserva. {0}", PNR);
            }
            if (creationType == CreationMethodType.DIRECT)
            {
                await Application.Current.MainPage.DisplayAlert("No se pudo obtener el PNR",
                                    commonErrorMessage, "Aceptar");
            }
            else
            {
                var errorMessage = errorText != null ? string.Format("Mensaje de error: {0}", errorText) : string.Empty;
                DependencyService.Get<IMessage>().LongAlert(string.Format("Ocurrió un error inesperado al buscar la información detallada de la reserva  {0}. {1}", PNR, errorMessage));
            }
        }

        private static async Task GoToHome()
        {
            //TODO
            //var mdp = Application.Current.MainPage as MasterDetailPage;
            //await mdp.Detail.Navigation.PopToRootAsync();
        }

        public string GetDeviceRegisterId()
        {
            return (!string.IsNullOrEmpty(Settings.BCAIncremenal)) ? Settings.BCAIncremenal.Substring(2, 5) : "NA";
        }

        public async void AutopopulateItineary(BagTagDetail bagTagInfo)
        {
            BagTagContext bagTagDetailRS = null;

            try
            {
                if (await GetConnectionStatus())
                {
                    if (bagTagInfo != null)
                    {
                        ItineraryList.Clear();
                        bagTagDetailRS = await BagTagServiceClient.GetBagTagDetail(bagTagInfo.CarrierCode, bagTagInfo.SerialNumber);
                        if (bagTagDetailRS != null)
                        {
                            ItineraryList.Clear();
                            var flights = ExtractItineraryInfo.GetItineraryList(bagTagDetailRS, ReservationDetail);
                            ItineraryList.AddRange(flights);
                        }
                    }

                }
            }
            catch (Exception ex)
            {
                DependencyService.Get<IMessage>().LongAlert("Ocurrió un error inesperado mientras se autocompletaba el itineario.");
                var properties = new Dictionary<string, string>
                {
                    { "BaseViewModel", "AutopopulateItineary" },
                };
                //Crashes.TrackError(ex, properties);
            }
        }
        #endregion


    }
}
