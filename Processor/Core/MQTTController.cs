using System.Reflection;
using MQTTnet;
using MQTTnet.Client;
using MQTTnet.Extensions.ManagedClient;

namespace Processor.Core;

public class MqttControllerAttribute : Attribute
{
}

public class MqttContext<T> where T : EventArgs
{
    public IManagedMqttClient Client { get; set; }
    public T Event { get; set; }
}

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
                typeof(MqttSubcribeAttribute))).ToList();
            acc.AddRange(sub);
            return acc;
        });

        var providerService = service.BuildServiceProvider();
        var routingTable = providerService.GetRequiredService<IRoutingTable>();
        var mqttClient = providerService.GetRequiredService<IManagedMqttClient>();

        var topics = subcribers.SelectMany(methodInfo =>
        {
            var attr = methodInfo.GetCustomAttribute<MqttSubcribeAttribute>()!;

            //add to routing table
            foreach (var topic in attr.topicFilters)
            {
                routingTable.AddMethod(topic.Topic, methodInfo);
            }

            return attr.topicFilters;
        }).ToList();

        // register subcribe
        // TODO: Handle permission fail
        await mqttClient.SubscribeAsync(topics);
    }
}