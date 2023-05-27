using Microsoft.Extensions.DependencyInjection;
using MQTTnet.AspNetCore.Client.Routing.Interface;
using MQTTnet.AspNetCore.Client.Routing.Lib;

namespace MQTTnet.AspNetCore.Client.Routing.Extension;

public static class UseMqttSubcriberRouting
{
    public static void AddMqttSubcribeRouting(this IServiceCollection service)
    {
        service.AddSingleton<IRoutingTable>(new RoutingTable());
    }
}