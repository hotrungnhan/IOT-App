using MQTTnet.AspNetCore.Client.Routing.Attribute;
using MQTTnet.AspNetCore.Client.Routing.Lib;
using MQTTnet.Client;

namespace Processor.Controller;

[MqttController]
public class TemplateController : BaseMqttController
{
    [MqttEventSubscribe("topic2/{id:int}")]
    public void Method2(int id, [FromPayload] TemperatureDataUnit payload, [FromTopic] string topic)
    {
        var e = Event as MqttApplicationMessageReceivedEventArgs;
        Console.WriteLine(payload);
        Console.WriteLine(topic);
        Console.WriteLine(id);
        // return Task.CompletedTask;
    }
}