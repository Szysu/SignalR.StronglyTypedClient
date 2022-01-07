using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR.Client;

namespace SignalR.StronglyTypedClient
{
    public static class HubConnectionBuilderExtensions
    {
        public static ClientHubConnection<TInvokeMethods> UseStronglyTypedClient<TInvokeMethods, THandlerMethods>(
            this IHubConnectionBuilder builder,
            THandlerMethods handler)
        {
            var hubConnection = builder.Build();

            var serverMethods = handler.GetType().GetMethods();
            foreach (var method in serverMethods)
            {
                var methodName = method.Name;
                var parameters = method.GetParameters().Select(p => p.ParameterType);
                hubConnection.On(methodName, parameters.ToArray(), objects =>
                {
                    method.Invoke(handler, objects);
                    return Task.CompletedTask;
                });
            }

            return new ClientHubConnection<TInvokeMethods>(hubConnection);
        }
    }
}