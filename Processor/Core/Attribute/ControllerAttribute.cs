namespace Processor.Core.Attribute;

public class MqttControllerAttribute : System.Attribute
{
    public string? MqttName { get; set; } // for multiple MQTT client
}