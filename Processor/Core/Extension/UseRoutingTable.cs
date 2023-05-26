using Processor.Core.Lib;
using Processor.Interface;

namespace Processor.Core.Extension;

public static class UseMqttSubcriberRouting
{
    public static void AddMqttSubcribeRouting(this IServiceCollection service)
    {
        service.AddSingleton<IRoutingTable>(new RoutingTable());
    }
}