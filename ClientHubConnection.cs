using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR.Client;

namespace SignalR.StronglyTypedClient
{
    public class ClientHubConnection<TInvokeMethods>
    {
        public ClientHubConnection(HubConnection hubConnection)
        {
            HubConnection = hubConnection;
            Methods = Proxy<TInvokeMethods>.Create(hubConnection);
        }

        private HubConnection HubConnection { get; }

        public TInvokeMethods Methods { get; }

        public async Task StartAsync()
        {
            await HubConnection.StartAsync();
        }

        public async Task StopAsync()
        {
            await HubConnection.StopAsync();
        }
    }
}