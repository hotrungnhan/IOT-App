using System.Text;
using Microsoft.Extensions.Logging;
using MQTTnet.AspNetCore.Client.Routing.Interface;
using MQTTnet.AspNetCore.Client.Routing.Lib;
using MQTTnet.Client;
using MQTTnet.Extensions.ManagedClient;
using MQTTnet.Packets;
using Newtonsoft.Json;

namespace MQTTnet.AspNetCore.Client.Routing;

public class RoutingMqttClient
{
    private readonly ILogger<RoutingMqttClient> _logger;

    private readonly IManagedMqttClient _mqttClient;

    private readonly MqttRouter _router;
    private readonly IServiceProvider _serviceProvider;
    private readonly ManagedMqttClientOptions _options;
    private readonly ISerializer _serializer;

    public RoutingMqttClient(ILogger<RoutingMqttClient> logger, IManagedMqttClient mqttClient,
        MqttRouter router,
        IServiceProvider serviceProvider, ManagedMqttClientOptions options, ISerializer serializer)
    {
        _serviceProvider = serviceProvider;
        _options = options;
        _serializer = serializer;
        _logger = logger;
        _mqttClient = mqttClient;
        _router = router;
    }

    internal async Task? Start(CancellationToken stoppingToken)
    {
        await _mqttClient.StartAsync(_options);
        _mqttClient.ApplicationMessageReceivedAsync += (e) => _router.OnMessageReceiveEvent(_serviceProvider, e, false);
        // _mqttClient.ConnectedAsync += InvokeConnectEvent;
        // _mqttClient.ConnectingFailedAsync += InvokeConnectEvent;
        // _mqttClient.DisconnectedAsync += InvokeConnectEvent;
        
        _logger.LogInformation("Start !");
    }

    internal async Task? GradeflShutdown()
    {
        await _mqttClient.StopAsync();
        _logger.LogInformation("Grateful shutdown !");
    }
}