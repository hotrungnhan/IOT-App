using MQTTnet;
using MQTTnet.Client;
using MQTTnet.Extensions.ManagedClient;
using Newtonsoft.Json;

namespace Processor;

public static class UseMQTT
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
        Console.WriteLine(await manageClient.InternalClient.TryPingAsync());
        await manageClient.StartAsync(manageClientOptions);
        service.AddSingleton<IManagedMqttClient>(manageClient);
        return manageClient;
    }
}