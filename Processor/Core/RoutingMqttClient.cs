using System.Reflection;
using MQTTnet.Client;
using MQTTnet.Extensions.ManagedClient;
using Processor.Core.Attribute;
using Processor.Core.Lib;
using Processor.Interface;

namespace Processor.Core;

public class RoutingMqttClient
{
    private readonly ILogger<RoutingMqttClient> _logger;
    private readonly IManagedMqttClient _mqttClient;
    private readonly IRoutingTable _routingTable;
    private readonly IServiceProvider _serviceProvider;

    public RoutingMqttClient(ILogger<RoutingMqttClient> logger, IManagedMqttClient mqttClient,
        IRoutingTable routingTable,
        IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
        _logger = logger;
        _mqttClient = mqttClient;
        _routingTable = routingTable;
    }

    internal Task? InvokeReceivedEvent(MqttApplicationMessageReceivedEventArgs args)
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
            var passingParams = new List<object>(parameters.Length);

            var index = parameters.ToList().FindIndex((s) =>
                s.IsDefined(typeof(FromEventAttribute)) ||
                s.ParameterType == typeof(MqttApplicationMessageReceivedEventArgs));
            if (index > 0)
            {
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

    internal async Task? GradefulShutdown()
    {
        await _mqttClient.StopAsync();
        Console.WriteLine("Grateful shutdown !");
    }
}