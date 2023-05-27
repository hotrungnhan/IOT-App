using Microsoft.Extensions.DependencyInjection;
using MQTTnet.Extensions.ManagedClient;

namespace MQTTnet.AspNetCore.Client.Routing.Extension;

public static class UseMqtt
{
    public static Task<IManagedMqttClient> AddMqtt(this IServiceCollection service, string host, int port,
        string username, string password)
    {
        var mqttFactory = new MqttFactory();
        var clientOptions = mqttFactory.CreateClientOptionsBuilder()
            .WithClientId(Guid.NewGuid().ToString())
            .WithTcpServer(host, port)
            .WithCredentials(username, password)
            .WithCleanSession()
            .Build();
        var manageClientOptions = new ManagedMqttClientOptionsBuilder().WithClientOptions(clientOptions).Build();
        var manageClient = mqttFactory.CreateManagedMqttClient();

        service.AddSingleton(manageClientOptions);
        service.AddSingleton<IManagedMqttClient>(manageClient);
        service.AddSingleton<RoutingMqttClient>();
        return Task.FromResult(manageClient);
    }
}