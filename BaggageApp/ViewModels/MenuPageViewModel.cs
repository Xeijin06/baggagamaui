using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace BaggageApp.ViewModels
{
    public class MenuPageViewModel : BaseViewModel
    {
        #region Private members variables
        private string _searchText = string.Empty;
        private bool _showSearchBoxStack;
        private bool _showActionButtons;
        private bool _showSearchTagEntry;
        private Models.BagTagDetail _currentBagTagDetail;
        #endregion

        #region Properties
        public Models.BagTagDetail CurrentBagTagDetail
        {
            get { return _currentBagTagDetail; }
            set { _currentBagTagDetail = value; }
        }
        public string Incremental { get; set; }
        public string Station { get; set; }

        public string SearchText
        {
            get
            {
                return _searchText;
            }
            set
            {
                SetProperty(ref _searchText, ToUpper(value));
            }
        }
        public bool ShowSearchBoxStack
        {
            get
            {
                return _showSearchBoxStack;
            }
            set
            {
                SetProperty(ref _showSearchBoxStack, value);
            }
        }
        public bool ShowActionButtons
        {
            get
            {
                return _showActionButtons;
            }
            set
            {
                SetProperty(ref _showActionButtons, value);
            }
        }

        public bool ShowSearchTagEntry
        {
            get
            {
                return _showSearchTagEntry;
            }
            set
            {
                SetProperty(ref _showSearchTagEntry, value);
            }
        }
        public ICommand OnTouchHandlerCommand { get; set; }
        public ICommand OnAppearingCommand { get; set; }
        public ICommand OpenScannerCommand { get; set; }
        #endregion

        #region Constructors, destructors and finalizers
        public MenuPageViewModel()
        {
            try
            {
                OnTouchHandlerCommand = new Command(async (obj) => await ExecuteMenuManager(obj));
                OnAppearingCommand = new Command(async () => await LoadMenuView());
                OpenScannerCommand = new Command(async () => await ExecuteQRScanner());
                //Incremental = Settings.BCAIncremenal;
                //Station = Settings.ArrivalStation;
                ShowActionButtons = true;
            }
            catch (Exception ex)
            {
                //Crashes.TrackError(ex);
            }

        }

        private async Task ExecuteQRScanner()
        {
            /*MasterDetailPage mdp;
            //setup options
            var options = new MobileBarcodeScanningOptions
            {
                AutoRotate = true,
                UseFrontCameraIfAvailable = false,
                TryHarder = true
            };

            var overlay = new ZXingDefaultOverlay
            {
                ShowFlashButton = false,
                TopText = "Align the barcode within the frame",
                BottomText = "Scanning will happen automatically"

            };
            overlay.BindingContext = overlay;
            var ScannerPage = new ZXingScannerPage(options, overlay);

            mdp = Application.Current.MainPage as MasterDetailPage;
            await mdp.Detail.Navigation.PushModalAsync(ScannerPage, true);

            ScannerPage.OnScanResult += (result) =>
            {
                // Parar de escanear
                ScannerPage.IsScanning = false;
                // Alert com o código escaneado
                Device.BeginInvokeOnMainThread(async () =>
                {
                    await mdp.Detail.Navigation.PopModalAsync();
                    if (result != null && result.Text.IsBagTagNumber())
                    {
                        await mdp.Detail.Navigation.PushAsync(new SearchResultPage(result.Text), true);
                    }
                    else
                    {
                        await Application.Current.MainPage.DisplayAlert("Resultado del escaner", "Código escaneado no corresponde a una colilla válida", "OK");
                    }
                });
            };


            //mdp = Application.Current.MainPage as MasterDetailPage;
            //await mdp.Detail.Navigation.PushModalAsync(ScannerPage);
            //await Navigation.PushAsync(ScannerPage);*/
        }
        #endregion

        #region Commands implementations

        private async Task LoadMenuView()
        {
            try
            {
                //await Task.Delay(1000);
                await ConnenctionManager.ValidateAuthentication();
            }
            catch (Exception ex)
            {
                //Crashes.TrackError(ex);
            }

        }
        private async Task ExecuteMenuManager(object obj)
        {
            var menuType = obj as string;
            if (menuType == null)
            {
                return;
            }
            if (IsBusy)
                return;

            IsBusy = true;
            try
            {
                //var masterDetailPage = Application.Current.MainPage as MasterDetailPage;

                var key = (string)obj;

                switch (key)
                {
                    case "home":
                        //masterDetailPage.IsPresented = true;
                        break;
                    case "ahl":
                        ShowSearchBoxStack = !ShowSearchBoxStack;
                        break;
                    case "ohd":
                        //await masterDetailPage.Detail.Navigation.PushAsync(new OnHandReportPage(null, true), true);
                        break;
                    case "fwd":
                        //await masterDetailPage.Detail.Navigation.PushAsync(new FWDReportPage(null, true), true);
                        break;
                    case "dpr":
                        //await masterDetailPage.Detail.Navigation.PushAsync(new DPRReportPage(null, true), true);
                        //await masterDetailPage.Detail.Navigation.PushAsync(new FlightListPage(ReportType.DPR), true);
                        break;
                    case "close_search_box":
                        ShowSearchBoxStack = false;
                        break;
                    case "scanner":
                        await ExecuteQRScanner();
                        break;
                    case "search":
                        var searchtext = SearchText;
                        await SearchByTagNumberAsync();
                        break;
                    case "tag":
                        ShowActionButtons = false;
                        ShowSearchTagEntry = true;
                        SearchText = "";
                        break;
                    case "cancel_tag_search":
                        ShowSearchTagEntry = false;
                        ShowActionButtons = true;
                        SearchText = "";
                        break;
                    case "rfp":
                        //DependencyService.Get<IMessage>().LongAlert("Esta funcionalidad estará disponible próximamente.");
                        break;
                    case "flights":
                        //await masterDetailPage.Detail.Navigation.PushAsync(new FlightListPage(ReportType.AHL), true);
                        break;


                }
            }
            catch (Exception exception)
            {
                var properties = new Dictionary<string, string>
                {
                    { "MenuPageViewModel", "ExecuteMenuManager" }
                };
                //Crashes.TrackError(exception, properties);
            }
            finally
            {
                IsBusy = false;
            }
        }

        private async Task SearchByTagNumberAsync()
        {
            /*MasterDetailPage mdp;
            mdp = Application.Current.MainPage as MasterDetailPage;
            try
            {
                if (SearchText.IsBagTagNumber() || SearchText.IsBagTagAlphanumeric() || SearchText.IsCopaAirlineBagTag())
                {
                    await mdp.Detail.Navigation.PushAsync(new SearchResultPage(SearchText), true);
                }
                else
                {
                    DependencyService.Get<IMessage>().LongAlert("Debe ingresar un número de colilla válido.");
                }
            }
            catch (Exception ex)
            {
                DependencyService.Get<IMessage>().LongAlert(ex.Message);
            }*/

        }

        private string ToUpper(string value)
        {
            return !string.IsNullOrEmpty(value) ? value.ToUpper() : value;
        }
        #endregion
    }
}
