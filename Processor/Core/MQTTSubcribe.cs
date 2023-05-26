using MQTTnet.Packets;
using MQTTnet.Protocol;

namespace Processor.Core;

public class MqttSubcribeAttribute : Attribute
{
    internal ICollection<MqttTopicFilter> topicFilters;

    public MqttSubcribeAttribute(ICollection<MqttTopicFilter> topicFilters)
    {
        this.topicFilters = topicFilters;
    }

    public MqttSubcribeAttribute(string topic, MqttQualityOfServiceLevel qos = MqttQualityOfServiceLevel.AtMostOnce)
    {
        topicFilters = new List<MqttTopicFilter>
        {
            new()
            {
                Topic = topic,
                QualityOfServiceLevel = qos
            }
        };
    }
}