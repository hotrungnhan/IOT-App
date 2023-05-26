using System.Reflection;
using System.Text;
using MQTTnet;
using MQTTnet.Client;
using MQTTnet.Extensions.ManagedClient;
using MQTTnet.Packets;
using Newtonsoft.Json;
using Processor.Core;

namespace Processor;

public class BackgroundWorker : BackgroundService
{
    private readonly ILogger<Worker> _logger;
    private readonly IManagedMqttClient _mqttClient;
    private readonly IRoutingTable _routingTable;
    private readonly IServiceProvider _serviceProvider;

    public BackgroundWorker(ILogger<Worker> logger, IManagedMqttClient mqttClient, IRoutingTable routingTable,
        IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
        _logger = logger;
        _mqttClient = mqttClient;
        _routingTable = routingTable;
    }

    private Task InvokeEvent(MqttApplicationMessageReceivedEventArgs args)
    {
        var topic = args.ApplicationMessage.Topic;
        var handlers = _routingTable.Trace(topic);
        foreach (var methodInfo in handlers)
        {
            var classConstructorInfor = methodInfo.DeclaringType.GetConstructors().FirstOrDefault();
            var passingClassParams = new List<object?>();
            if (classConstructorInfor != null)
            {
                passingClassParams = new List<object?>(classConstructorInfor.GetParameters().Length);
                var i = 0;
                foreach (var parameterInfo in classConstructorInfor.GetParameters())
                {
                    passingClassParams[i] = _serviceProvider.GetService(parameterInfo.ParameterType);
                    i++;
                }
            }

            var parameters = methodInfo.GetParameters();
            var passingParams = new List<object>(parameters.Length);
            var index = parameters.ToList().FindIndex((s) =>
                s.IsDefined(typeof(MqttContext<MqttApplicationMessageReceivedEventArgs>)));
            if (index > 0)
            {
                passingParams[index] = new MqttContext<MqttApplicationMessageReceivedEventArgs>
                {
                    Client = _mqttClient,
                    Event = args,
                };
            }

            var parentClass = Activator.CreateInstance(methodInfo.DeclaringType, passingClassParams.ToArray());
            return methodInfo.Invoke(parentClass, passingParams.ToArray()) as Task;
        }

        return Task.CompletedTask;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        // _mqttClient.ApplicationMessageReceivedAsync += args => { };
        while (!stoppingToken.IsCancellationRequested)
        {
            await Task.Delay(1000, stoppingToken);
            await InvokeEvent(new MqttApplicationMessageReceivedEventArgs(
                "12d12d1", new MqttApplicationMessage
                {
                    Topic = "topic",
                }, new MqttPublishPacket(), null));
        }
    }
}