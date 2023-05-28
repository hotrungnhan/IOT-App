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
        await _routingMqttClient.Start()!;
        while (!stoppingToken.IsCancellationRequested)
        {
            // var json = JsonConvert.SerializeObject(new TemperatureDataUnit());

            // moving to testing
            // await _routingMqttClient.InvokeReceivedEvent(new MqttApplicationMessageReceivedEventArgs(
            //     "12d12d1", new MqttApplicationMessage
            //     {
            //         Topic = "topic2",
            //         PayloadSegment = bytes,
            //     }, new MqttPublishPacket(), null))!;
            await Task.Delay(1000, stoppingToken);
        }

        // Todo : fix this
        await _routingMqttClient.GradefulShutdown()!;
    }
}