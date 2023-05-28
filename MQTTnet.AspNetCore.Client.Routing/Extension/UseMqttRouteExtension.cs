using System.Reflection;
using Microsoft.Extensions.DependencyInjection;
using MQTTnet.AspNetCore.Client.Routing.Interface;
using MQTTnet.AspNetCore.Client.Routing.Lib;
using MQTTnet.AspNetCore.Routing;

namespace MQTTnet.AspNetCore.Client.Routing.Extension;

public static class UseMqttRouteExtension
{
    public static void AddMqttSubcribe(this IServiceCollection service)
    {
        service.AddSingleton(_ => MqttRouteTableFactory.Create(AppDomain.CurrentDomain.GetAssemblies()));
    }

    public static void AddRouter(this IServiceCollection service)
    {
        service.AddSingleton<MqttRouter>();
        service.AddSingleton<ITypeActivatorCache, TypeActivatorCache>();
    }
}