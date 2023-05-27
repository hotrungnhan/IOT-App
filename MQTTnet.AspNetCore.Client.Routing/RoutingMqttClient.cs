using System.Reflection;
using Microsoft.Extensions.Logging;
using MQTTnet.AspNetCore.Client.Routing.Attribute;
using MQTTnet.AspNetCore.Client.Routing.Interface;
using MQTTnet.AspNetCore.Client.Routing.Lib;
using MQTTnet.Client;
using MQTTnet.Extensions.ManagedClient;

namespace MQTTnet.AspNetCore.Client.Routing;

public class RoutingMqttClient
{
    private readonly ILogger<RoutingMqttClient> _logger;
    private readonly IManagedMqttClient _mqttClient;
    private readonly IRoutingTable _routingTable;
    private readonly IServiceProvider _serviceProvider;
    private readonly ManagedMqttClientOptions _options;
    private readonly ISerializer _serializer;

    public RoutingMqttClient(ILogger<RoutingMqttClient> logger, IManagedMqttClient mqttClient,
        IRoutingTable routingTable,
        IServiceProvider serviceProvider, ManagedMqttClientOptions options, ISerializer serializer)
    {
        _serviceProvider = serviceProvider;
        _options = options;
        _serializer = serializer;
        _logger = logger;
        _mqttClient = mqttClient;
        _routingTable = routingTable;
    }

    internal Task? InvokeReceivedEvent(MqttApplicationMessageReceivedEventArgs? args)
    {
        var topic = args.ApplicationMessage.Topic;
        var handlers = _routingTable.Trace(topic);
        if (handlers == null)
        {
            return Task.CompletedTask;
        }

        foreach (var methodInfo in handlers)
        {
            var classConstructorInfo = methodInfo.DeclaringType?.GetConstructors().FirstOrDefault();
            var passingClassParams = new List<object?>();
            if (classConstructorInfo != null)
            {
                passingClassParams = new List<object?>(classConstructorInfo.GetParameters().Length);
                var i = 0;
                foreach (var parameterInfo in classConstructorInfo.GetParameters())
                {
                    passingClassParams[i] = _serviceProvider.GetService(parameterInfo.ParameterType);
                    i++;
                }
            }

            var parameters = methodInfo.GetParameters();
            List<object?> passingParams = Enumerable.Repeat<object>(null, parameters.Length).ToList();

            var index = parameters.ToList().FindIndex((s) =>
                s.IsDefined(typeof(FromEventAttribute)) ||
                s.ParameterType == typeof(MqttApplicationMessageReceivedEventArgs));

            if (index >= 0)
            {
                passingParams[index] = args;
                // implement serialize
            }

            index = parameters.ToList().FindIndex((s) =>
                s.IsDefined(typeof(FromPayloadAttribute)));
            if (index >= 0)
            {
                var method = _serializer.GetType().GetMethod(nameof(ISerializer.Deserialize));
                var genericSerialize = method?.MakeGenericMethod(parameters[index].ParameterType);
                var payload =
                    genericSerialize?.Invoke(_serializer,
                        new object?[] { args.ApplicationMessage.PayloadSegment.ToArray() });

                passingParams[index] = payload;
                // implement serialize  
            }

            var parrent =
                Activator.CreateInstance(methodInfo.DeclaringType, passingClassParams.ToArray());
            if (parrent is BaseMqttController parrentBaseController)
            {
                parrentBaseController.Event = args;
                parrentBaseController.Client = _mqttClient;
            }

            return (Task?)(methodInfo.Invoke(parrent, passingParams.ToArray()) ?? Task.CompletedTask);
        }

        return Task.CompletedTask;
    }


    internal Task? InvokeConnectEvent<T>(T args) where T : EventArgs
    {
        // MqttClientConnectedEventArgs
        // ConnectingFailedEventArgs
        // MqttClientDisconnectedEventArgs
        return Task.CompletedTask;
    }

    internal async Task? Start()
    {
        await _mqttClient.StartAsync(_options);
        Console.WriteLine("Start !");
    }

    internal async Task? GradefulShutdown()
    {
        await _mqttClient.StopAsync();
        Console.WriteLine("Grateful shutdown !");
    }
}