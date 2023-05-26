using Processor;
using Processor.Core;
using Processor.Core.Extension;
using Processor.Core.Lib;

async void ConfigureDelegate(IServiceCollection services)
{
    services.Configure<HostOptions>(
        opts => opts.ShutdownTimeout = TimeSpan.FromMinutes(2));

    await services.AddMqtt("localhost", 1883, "admin", "admin");
    services.AddMqttSerializer<JsonSerializerAdapter>();
    services.AddMqttSubcribeRouting();
    await services.AddMqttController();
    services.AddHostedService<MqttClientRoutingService>();
}

IHost host = Host.CreateDefaultBuilder(args)
    .ConfigureServices(ConfigureDelegate)
    .Build();
host.Run();