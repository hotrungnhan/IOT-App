using MQTTnet;
using MQTTnet.Client;
using MQTTnet.Extensions.ManagedClient;
using Newtonsoft.Json;

namespace Processor;

public class Worker : BackgroundService
{
    private readonly ILogger<Worker> _logger;
    IManagedMqttClient _mqttClient;

    public Worker(ILogger<Worker> logger, IManagedMqttClient mqttClient)
    {
        _logger = logger;
        _mqttClient = mqttClient;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        await _mqttClient.SubscribeAsync("hello");
        _mqttClient.ApplicationMessageReceivedAsync += args =>
        {
            var topic = args.ApplicationMessage.Topic;
            Console.WriteLine(topic);

            var payload = args.ApplicationMessage.ConvertPayloadToString();
            Console.WriteLine(payload);
            var data = JsonConvert.DeserializeObject<TemperatureDataUnit>(payload);
            Console.WriteLine(data.ToString());
            _mqttClient.EnqueueAsync(new MqttApplicationMessageBuilder().WithTopic("hello2")
                .WithPayload(JsonConvert.SerializeObject(data)).Build());

            // var payload = args.ApplicationMessage.ApplicationMessage.PayloadSegment.ToArray();
            // var user = ProtoBuf.Serializer.Deserialize<DTO.Proto.Person>(payload);
            return Task.CompletedTask;
        };
        _logger.LogInformation("Worker running at: {time}", DateTimeOffset.Now);
        while (!stoppingToken.IsCancellationRequested)
        {
            await Task.Delay(1000, stoppingToken);
        }
    }
}