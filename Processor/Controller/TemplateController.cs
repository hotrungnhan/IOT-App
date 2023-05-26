using MQTTnet.Protocol;
using Processor.Core;

namespace Example.MqttControllers;

[MqttController]
public class TemplateController
{
    [MqttSubcribe("topic")]
    public Task Method()
    {
        Console.WriteLine("topic invoke");
        return Task.CompletedTask;
    }
}
