using MQTTnet.Extensions.ManagedClient;

namespace MQTTnet.AspNetCore.Client.Routing.Lib;

public abstract class BaseMqttController
{
    internal IManagedMqttClient? Client { get; set; }
    public EventArgs? Event { get; set; }

    // Only work in Mqtt event router
    public Task? Enqueue(MqttApplicationMessage applicationMessage) =>
        Client?.EnqueueAsync(applicationMessage);

    // Only work in Mqtt event router
    public Task? Enqueue(ManagedMqttApplicationMessage applicationMessage) =>
        Client?.EnqueueAsync(applicationMessage);

    // Only work in Mqtt event router
    public Task? Ping(CancellationToken c) =>
        Client?.PingAsync(c);
}