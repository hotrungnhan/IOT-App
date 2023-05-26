using MQTTnet;
using MQTTnet.Client;
using MQTTnet.Packets;

namespace Processor.Core;

public class MqttClientRoutingService : BackgroundService
{
    private readonly ILogger<MqttClientRoutingService> _logger;

    private readonly RoutingMqttClient _routingMqttClient;
    // private readonly ISerializer _serializer;

    public MqttClientRoutingService(ILogger<MqttClientRoutingService> logger,
        RoutingMqttClient routingMqttClient)
    {
        _logger = logger;
        _routingMqttClient = routingMqttClient;
    }


    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        // _mqttClient.ApplicationMessageReceivedAsync += async args => { };
        while (!stoppingToken.IsCancellationRequested)
        {
            await Task.Delay(1000, stoppingToken);
            await _routingMqttClient.InvokeReceivedEvent(new MqttApplicationMessageReceivedEventArgs(
                "12d12d1", new MqttApplicationMessage
                {
                    Topic = "topic2",
                }, new MqttPublishPacket(), null))!;
        }

        // Todo : fix this
        await _routingMqttClient.GradefulShutdown()!;
    }
}