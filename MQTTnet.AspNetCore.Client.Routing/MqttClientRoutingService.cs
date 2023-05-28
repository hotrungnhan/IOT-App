using System.Text;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using MQTTnet.Client;
using MQTTnet.Packets;

namespace MQTTnet.AspNetCore.Client.Routing;

public class MqttClientRoutingService : BackgroundService
{
    private readonly ILogger<MqttClientRoutingService> _logger;

    private readonly RoutingMqttClient _routingMqttClient;

    public MqttClientRoutingService(ILogger<MqttClientRoutingService> logger,
        RoutingMqttClient routingMqttClient)
    {
        _logger = logger;
        _routingMqttClient = routingMqttClient;
    }


    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        await _routingMqttClient.Start(stoppingToken)!;

        // Todo : fix this
        // await _routingMqttClient.GradefulShutdown()!;
    }
}