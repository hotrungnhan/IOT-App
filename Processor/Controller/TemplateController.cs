using MQTTnet.AspNetCore.Client.Routing.Attribute;
using MQTTnet.AspNetCore.Client.Routing.Lib;
using MQTTnet.Client;

namespace Processor.Controller;

[MqttController]
public class TemplateController : BaseMqttController
{
    [MqttEventSubcribe("topic")]
    public Task Method()
    {
        var e = this.Event as MqttApplicationMessageReceivedEventArgs;
        Console.WriteLine(e.ApplicationMessage.Topic);
        Console.WriteLine("topic invoke");
        return Task.CompletedTask;
    }

    [MqttEventSubcribe("topic2")]
    public Task Method2([FromPayload] TemperatureDataUnit payload)
    {
        var e = this.Event as MqttApplicationMessageReceivedEventArgs;
        Console.WriteLine(payload);
        Console.WriteLine("topic invoke");
        return Task.CompletedTask;
    }
}