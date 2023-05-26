using Processor.Core.Lib;

namespace Processor.Core.Extension;

public static class UseMQTTSerializerExtension
{
    public static IServiceCollection AddMqttSerializer<T>(this IServiceCollection service, T? instance = null)
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

        return service;
    }
}