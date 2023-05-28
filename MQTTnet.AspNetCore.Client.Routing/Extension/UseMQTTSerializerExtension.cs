using Microsoft.Extensions.DependencyInjection;
using MQTTnet.AspNetCore.Client.Routing.Interface;

namespace MQTTnet.AspNetCore.Client.Routing.Extension;

public static class UseMQTTSerializerExtension
{
    public static void AddMqttSerializer<T>(this IServiceCollection service, T? instance = null)
        where T : class, ISerializer
    {
        if (instance != null)
        {
            service.AddSingleton<ISerializer>(instance);
        }
        else
        {
            service.AddSingleton<ISerializer, T>();
        }
    }
}