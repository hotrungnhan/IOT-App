using System.Net;
using Microsoft.AspNetCore.Builder;
using MQTTnet;
using MQTTnet.AspNetCore.Routing;
using MQTTnet.Client;
using Processor;
using Processor.Core;

IHost host = Host.CreateDefaultBuilder(args)
    .ConfigureServices(async services =>
    {
        await services.AddMqtt("localhost", 1883, "admin", "admin");
        services.AddMqttSubcribeRouting();
        await services.AddMqttController();
        services.AddHostedService<BackgroundWorker>();
    })
    .Build();

host.Run();