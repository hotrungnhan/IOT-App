using MQTTnet.AspNetCore.Client.Routing;
using MQTTnet.AspNetCore.Client.Routing.Extension;
using MQTTnet.AspNetCore.Client.Routing.Lib;

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
host.Run();