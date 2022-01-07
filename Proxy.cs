using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR.Client;

namespace SignalR.StronglyTypedClient
{
    public class Proxy<T> : DispatchProxyAsync
    {
        private HubConnection HubConnection { get; set; }

        public static T Create(HubConnection hubConnection)
        {
            object proxy = Create<T, Proxy<T>>();
            ((Proxy<T>) proxy).SetParameters(hubConnection);
            return (T) proxy;
        }

        public override object Invoke(MethodInfo method, object[] args)
        {
            var name = method.Name;
            return HubConnection.SendCoreAsync(name, args);
        }

        public override async Task InvokeAsync(MethodInfo method, object[] args)
        {
            var name = method.Name;
            await HubConnection.SendCoreAsync(name, args);
        }

        public override async Task<T1> InvokeAsyncT<T1>(MethodInfo method, object[] args)
        {
            var name = method.Name;
            var returnType = method.ReturnType;
            return (T1) await HubConnection.InvokeCoreAsync(name, returnType.GetGenericArguments().First(),
                args);
        }

        private void SetParameters(HubConnection hubConnection)
        {
            HubConnection = hubConnection;
        }
    }
}