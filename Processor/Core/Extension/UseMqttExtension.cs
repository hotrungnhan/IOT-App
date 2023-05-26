using MQTTnet;
using MQTTnet.Extensions.ManagedClient;

namespace Processor.Core.Extension;

public static class UseMqtt
{
    public static async Task<IManagedMqttClient> AddMqtt(this IServiceCollection service, string host, int port,
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

        // await manageClient.PingAsync(CancellationToken.None);
        manageClient.ConnectedAsync += (args =>
        {
            Console.WriteLine(args.ConnectResult.ResultCode);
            return Task.CompletedTask;
        });
        await manageClient.StartAsync(manageClientOptions);
        service.AddSingleton<IManagedMqttClient>(manageClient);
        service.AddSingleton<RoutingMqttClient>();
        return manageClient;
    }
}