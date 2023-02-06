using Plugin.Connectivity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BaggageApp.Helpers
{
    public class ConnectionStatus : IConnectionStatus
    {

        public async Task<bool> ConnectionEnabled(string url, int port = 80, bool showErrorMessage = true)
        {
            bool reachable = await CrossConnectivity.Current.IsRemoteReachable(url, port);
            bool isConnected = CrossConnectivity.Current.IsConnected;
            bool connectionStatus = false;

            if (isConnected && reachable)
            {
                connectionStatus = true;
            }
            else if (!isConnected && showErrorMessage)
            {
                DependencyService.Get<IMessage>().LongAlert("Conexión a internet no disponible");
            }
            else if (!reachable && showErrorMessage)
            {
                DependencyService.Get<IMessage>().LongAlert("Conexión a recursos remotos no disponible.");
            }
            return await Task.FromResult(connectionStatus);
        }

        public async Task<bool> ConnectionServiceEnabled(string url, int port = 80)
        {
            bool reachable = await CrossConnectivity.Current.IsRemoteReachable(url, port);
            bool isConnected = CrossConnectivity.Current.IsConnected;
            bool connectionStatus = false;

            if (isConnected && reachable)
            {
                connectionStatus = true;
            }
            else if (!isConnected)
            {
                //DependencyService.Get<IMessage>().LongAlert("Conexión a internet no disponible");
            }
            else if (!reachable)
            {
                //DependencyService.Get<IMessage>().LongAlert("Conexión a recursos remotos no disponible.");
            }
            return await Task.FromResult(connectionStatus);
        }

        public async Task<bool> ValidateAuthentication()
        {
            bool IsValidToken = false;
            try
            {
                string urlTestConnection = Settings.TestConnectionDefaultURL;

                bool connected = await ConnectionEnabled(urlTestConnection, 80, false);
                if (connected)
                {
                    bool isSessionAlive = await App.BpmApiManager.GetSessionContext();
                    if (isSessionAlive)
                    {
                        IsValidToken = true;
                        Settings.IsAuthenticated = true;
                        Settings.ShowUnAuthorizedMessage = false;
                    }
                    else
                    {
                        Settings.IsAuthenticated = false;
                        Settings.ShowUnAuthorizedMessage = true;
                    }
                }
            }
            catch (Exception exception)
            {
                var properties = new Dictionary<string, string>
                {
                    { "ConnectionStatus", "ValidateAuthentication()" }
                };
                //Crashes.TrackError(exception, properties);
            }
            return await Task.FromResult(IsValidToken);
        }

    }
}
