using System.Text;
using MQTTnet;
using MQTTnet.AspNetCore.Client.Routing;
using MQTTnet.AspNetCore.Client.Routing.Extension;
using MQTTnet.AspNetCore.Client.Routing.Lib;
using MQTTnet.Client;
using MQTTnet.Packets;
using Newtonsoft.Json;
using Processor;

async void ConfigureDelegate(IServiceCollection services)
{
    services.Configure<HostOptions>(
        opts => opts.ShutdownTimeout = TimeSpan.FromMinutes(2));

    await services.AddMqtt("localhost", 1883, "admin", "admin");
    services.AddMqttSerializer<JsonSerializerAdapter>();
    services.AddMqttSubcribe();
    services.AddRouter();
    services.AddHostedService<MqttClientRoutingService>();
}

IHost host = Host.CreateDefaultBuilder(args)
    .ConfigureServices(ConfigureDelegate)
    .Build();
var router = host.Services.GetService<MqttRouter>();
while (true)
{
    var json = JsonConvert.SerializeObject(new TemperatureDataUnit());
    var bytes = Encoding.UTF8.GetBytes(json);
    await router?.OnMessageReceiveEvent(host.Services
        , new MqttApplicationMessageReceivedEventArgs(
            "12d12d1", new MqttApplicationMessage
            {
                Topic = "topic2/1231231231",
                PayloadSegment = bytes,
            }, new MqttPublishPacket(), null),
        false)!;
    await Task.Delay(1000);
}

// host.Run();