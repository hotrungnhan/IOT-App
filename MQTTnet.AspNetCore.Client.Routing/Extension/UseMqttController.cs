using System.Reflection;
using Microsoft.Extensions.DependencyInjection;
using MQTTnet.AspNetCore.Client.Routing.Attribute;
using MQTTnet.AspNetCore.Client.Routing.Interface;
using MQTTnet.Extensions.ManagedClient;

namespace MQTTnet.AspNetCore.Client.Routing.Extension;

public static class AddMqttControllerExtension
{
    //TODO :Handler connected, connection close,  fail connecting , message skip routing, ... 
    public static async Task AddMqttController(this IServiceCollection service)
    {
        // TODO: Replace Assembly.GetExecutingAssembly() with AppDomain.CurrentDomain.GetAssemblies() when release package
        var types = AppDomain.CurrentDomain.GetAssemblies().Select(s => s.GetType())
            .Where(t => t.IsDefined(typeof(MqttControllerAttribute)));

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