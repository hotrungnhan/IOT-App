namespace MQTTnet.AspNetCore.Client.Routing.Attribute;

public class MqttControllerAttribute : System.Attribute
{
    public MqttControllerAttribute(string? mqttName = "Default")
    {
        MqttName = mqttName;
    }

    public string? MqttName { get; set; } // for multiple MQTT client
}