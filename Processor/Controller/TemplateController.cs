using MQTTnet.Client;
using MQTTnet.Protocol;
using Processor.Core;
using Processor.Core.Attribute;
using Processor.Core.Lib;

namespace Example.MqttControllers;

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
    public Task Method2()
    {
        var e = this.Event as MqttApplicationMessageReceivedEventArgs;
        Console.WriteLine(e.ApplicationMessage.Topic);
        Console.WriteLine("topic invoke");
        return Task.CompletedTask;
    }
}