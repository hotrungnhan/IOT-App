using MQTTnet.Extensions.ManagedClient;

namespace MQTTnet.AspNetCore.Client.Routing.Lib;

public abstract class BaseMqttController
{
    public IManagedMqttClient Client { get; set; }
    public EventArgs? Event { get; set; }
}