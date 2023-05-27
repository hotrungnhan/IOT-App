using MQTTnet.Packets;
using MQTTnet.Protocol;

namespace MQTTnet.AspNetCore.Client.Routing.Attribute;

public class MqttEventSubcribeAttribute : System.Attribute
{
    internal readonly ICollection<MqttTopicFilter> TopicFilters;

    public MqttEventSubcribeAttribute(ICollection<MqttTopicFilter> topicFilters)
    {
        this.TopicFilters = topicFilters;
    }

    public MqttEventSubcribeAttribute(string topic,
        MqttQualityOfServiceLevel qos = MqttQualityOfServiceLevel.AtMostOnce)
    {
        TopicFilters = new List<MqttTopicFilter>
        {
            new()
            {
                Topic = topic,
                QualityOfServiceLevel = qos
            }
        };
    }
}

public class MqttEventConnectAttribute : System.Attribute // fail connect, success, pending....
{
}