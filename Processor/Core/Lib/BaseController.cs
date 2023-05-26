using MQTTnet.Extensions.ManagedClient;

namespace Processor.Core.Lib;

public class BaseMqttController
{
    public IManagedMqttClient Client { get; set; }
    public EventArgs Event { get; set; }
}