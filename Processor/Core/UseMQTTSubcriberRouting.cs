using MQTTnet;
using MQTTnet.Client;
using MQTTnet.Extensions.ManagedClient;

namespace Processor.Core;

public static class UseMQTTSubcriberRouting
{
    public static void AddMqttSubcribeRouting(this IServiceCollection service)
    {
        service.AddSingleton<IRoutingTable>(new RoutingTable());
    }
}