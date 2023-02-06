using BaggageApp.DataStores;
using BaggageApp.Helpers;
using BaggageApp.Models;
using BaggageApp.Views;
using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace BaggageApp.ViewModels
{
    public class AuthViewModel : BaseViewModel
    {
        #region Private members variables
        private string _user = "lagonzalez";
        private string _password = "123";
        private Color _loginButtonBackgroundColor = Color.FromHex("EFEFEE");
        private Color _logintButtonTextColor = Color.FromHex("#DBDBDB");
        private bool _loginButtonTransparent = true;
        #endregion

        #region Properties
        public string User
        {
            get
            {
                return _user;
            }
            set
            {
                SetProperty(ref _user, value);
                ChangeLoginButtonBackground();
            }
        }
        public string Password
        {
            get
            {
                return _password;
            }
            set
            {
                SetProperty(ref _password, value);
                ChangeLoginButtonBackground();
            }
        }

        public Color LoginButtonBackgroundColor
        {
            get
            {
                return _loginButtonBackgroundColor;
            }
            set
            {
                SetProperty(ref _loginButtonBackgroundColor, value);
            }
        }
        public Color LogintButtonTextColor
        {
            get
            {
                return _logintButtonTextColor;
            }
            set
            {
                SetProperty(ref _logintButtonTextColor, value);
            }
        }
        public bool LoginButtonTransparent
        {
            get
            {
                return _loginButtonTransparent;
            }
            set
            {
                SetProperty(ref _loginButtonTransparent, value);
            }
        }
        public ICommand LoginCommand { get; set; }
        public ICommand LogoutCommand { get; set; }
        public ICommand OnLoadCommand { get; set; }

        IConnectionStatus connectionStatus;


        #endregion

        #region Constructors, destructors and finalizers
        public AuthViewModel()
        {
            LoginCommand = new Command(async () => await ExecuteLoginBPMCommand());
            LogoutCommand = new Command(async () => await ExecuteLogout());
            OnLoadCommand = new Command(async () => await ExecuteLoadLoginPage());
            connectionStatus = new ConnectionStatus();
            OnLoadCommand.Execute(null);
        }



        #endregion

        #region Commands implementations
        private async Task ExecuteLoginBPMCommand()
        {
            /*var databasePath = DependencyService.Get<IFileHelper>().GetLocalFilePath("bca.db");

            var db = new SQLiteAsyncConnection(databasePath);
            await db.CreateTableAsync<Stock>();*/

            //var pirDataStore = new PIRDataStore();
            //await pirDataStore.AddItemAsync(new Models.PIR());

            /*var forward = new ForwardItem()
            {
                Id = 1,
                CaseId = "1",
                BpmCaseId = "1",
                AirlineCode = "1",
                StationCode = "1",
                ReasonForLostCode = "1",
                ResponsibleAirlineCode = "1",
                ResponsibleStationCode = "1",
                RelatedFileLocatorSequenceNumber = "1",
                RelatedFileLocatorAirline = "1",
                RelatedFileLocatorStation = "1",

                AgentLastName = "1",
                ReasonLostComment = "1",
                FurtherInformation = "1",
                CreatedBy = "1",
                CreatedDate = DateTime.Now,
                StationCreatedBy = "1",
                SaveDate = DateTime.Now,
                Baggages = "1",
                Itinerary = "1",
                ItineraryPassenger = "1",
                Passengers = "1",
                RushRoutings = "1",
            };
            var forwardDt = new ForwardDataStore();
            await forwardDt.AddItemAsync(forward);

            //var value = await forwardDt.GetItemAsync("1");*/

            if (IsBusy)
                return;

            IsBusy = true;
            try
            {

                Settings.IsConnected = await GetConnectionStatus();
                if (Settings.IsConnected)
                {
                    if (!string.IsNullOrEmpty(User) && !string.IsNullOrEmpty(Password))
                    {
                        string user = User;
                        string password = Password;
                        bool login = await App.BpmApiManager.LoginToApiBpm(user, password);
                        if (login)
                        {
                            Settings.IsAuthenticated = true;
                            Application.Current.MainPage = new MenuPage();
                        }
                        else
                        {
                            Settings.IsAuthenticated = false;
                            await Application.Current.MainPage.DisplayAlert("Error", "Credenciales no válidas", "Ok");
                        }
                    }
                    else
                    {
                        await Application.Current.MainPage.DisplayAlert("Error", "Complete los campos requeridos antes de continuar.", "Ok");
                    }

                }
                else
                {
                    DependencyService.Get<IMessage>().LongAlert("No hay conexión a internet. Revise su conexión a internet antes de hacer login.");
                }
            }
            catch (Exception exception)
            {
                var properties = new Dictionary<string, string>
                {
                    { "AuthViewModel", "ExecuteLoginBPMCommand" }
                };
                //Crashes.TrackError(exception, properties);
            }
            finally
            {
                IsBusy = false;
            }
        }

        private async Task ExecuteLoginCommand()
        {

            if (IsBusy)
                return;

            IsBusy = true;
            try
            {
                var response = await DependencyService.Get<IAuthenticator>().Authenticate(Settings.TenantId, Settings.ResourceId, Settings.ClientId, Settings.GraphResourceUri, Settings.ReturnUrl);

                if (response != null)
                {
                    string _graphHeader = response.GraphAuthHeader;
                    string _groupToValidate = Settings.AuthorizedGroup;

                    bool userIsMember = await App.GraphApiManager.UserMemberOfGroup(_groupToValidate, _graphHeader);
                    if (userIsMember)
                    {
                        //Application.Current.MainPage = new TransitionNavigationPage(new MenuPage());
                        Settings.CurrentUser = response.Profile.DisplayableId.Replace("@copaair.com", "").Replace("t-", "");
                        Settings.AgentFullName = string.Format(@"{0}-{1}", response.Profile.GivenName, response.Profile.FamilyName);
                        Application.Current.MainPage = new MainPage();
                    }
                    else
                    {
                        await Application.Current.MainPage.DisplayAlert("Advertencia", "El usuario ingresado no pertenece a un grupo autorizado", "Cerrar");
                    }
                }
            }
            catch (Exception exception)
            {
                var properties = new Dictionary<string, string>
                {
                    { "AuthViewModel", "ExecuteLoginCommand" }
                };
                //Crashes.TrackError(exception, properties);
            }
            finally
            {
                IsBusy = false;
            }
        }

        private async Task ExecuteLogout()
        {
            if (IsBusy)
                return;

            IsBusy = true;
            try
            {
                await DependencyService.Get<IAuthenticator>().ClearToken(Settings.TenantId);
            }
            catch (Exception exception)
            {
                var properties = new Dictionary<string, string>
                {
                    { "AuthViewModel", "ExecuteLogout" }
                };
                //Crashes.TrackError(exception, properties);
            }
            finally
            {
                IsBusy = false;
            }
        }

        private async Task ExecuteLoadLoginPage()
        {

            if (IsBusy)
                return;

            IsBusy = true;
            try
            {
                int port = 80;
                string urlTestConnection = Settings.TestConnectionDefaultURL;
                Settings.IsConnected = await connectionStatus.ConnectionEnabled(urlTestConnection, port, true);

            }
            catch (Exception exception)
            {
                var properties = new Dictionary<string, string>
                {
                    { "AuthViewModel", "ExecuteLoadLoginPage" }
                };
                //Crashes.TrackError(exception, properties);
            }
            finally
            {
                IsBusy = false;
            }
        }

        private void ChangeLoginButtonBackground()
        {
            if (!string.IsNullOrEmpty(User) && !string.IsNullOrEmpty(Password))
            {
                LoginButtonTransparent = false;
                LoginButtonBackgroundColor = Color.FromHex("0060A9");
                LogintButtonTextColor = Color.FromHex("FFFFFF");
            }
            else
            {
                LoginButtonTransparent = true;
                LoginButtonBackgroundColor = Color.FromHex("EFEFEE");
                LogintButtonTextColor = Color.FromHex("#DBDBDB");
            }
        }
        #endregion
    }

    public class Stock
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        public string Symbol { get; set; }
    }

    
}
