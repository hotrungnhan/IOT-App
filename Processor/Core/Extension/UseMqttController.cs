using System.Reflection;
using MQTTnet.Extensions.ManagedClient;
using Processor.Core.Attribute;
using Processor.Interface;

namespace Processor.Core.Extension;

public static class AddMqttControllerExtension
{
    //TODO :Handler connected, connection close,  fail connecting , message skip routing, ... 
    public static async Task AddMqttController(this IServiceCollection service)
    {
        // TODO: Replace Assembly.GetExecutingAssembly() with AppDomain.CurrentDomain.GetAssemblies() when release package
        var types = Assembly.GetExecutingAssembly().GetTypes().Where(t => t.IsDefined(typeof(MqttControllerAttribute)));

        var subcribers = types.Aggregate(new List<MethodInfo>(), (acc, cur) =>
        {
            var sub = cur.GetMethods().Where(m => m.IsDefined(
                typeof(MqttEventSubcribeAttribute))).ToList();
            acc.AddRange(sub);
            return acc;
        });

        var providerService = service.BuildServiceProvider();
        var routingTable = providerService.GetRequiredService<IRoutingTable>();
        var mqttClient = providerService.GetRequiredService<IManagedMqttClient>();

        var topics = subcribers.SelectMany(methodInfo =>
        {
            var attr = methodInfo.GetCustomAttribute<MqttEventSubcribeAttribute>()!;

            //add to routing table
            foreach (var topic in attr.TopicFilters)
            {
                routingTable.AddMethod(topic.Topic, methodInfo);
            }

            return attr.TopicFilters;
        }).ToList();

        // register subcribe
        // TODO: Handle permission fail
        await mqttClient.SubscribeAsync(topics);
    }
}