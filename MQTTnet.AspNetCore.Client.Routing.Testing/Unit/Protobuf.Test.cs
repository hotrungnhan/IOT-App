using MQTTnet.AspNetCore.Client.Routing.Lib;
using MQTTnet.AspNetCore.Client.Routing.Tests.Model;

namespace MQTTnet.AspNetCore.Client.Routing.Tests.Unit;

[TestClass]
public class ProtobufTest
{
    [TestMethod]
    public void TestSerialize()
    {
        var serializer = new ProtobufSerializerAdapter();
        var data = serializer.Serialize(new Temperature
        {
            Value = 5,
            Unit = 4,
        });
        var temperature = serializer.Deserialize<Temperature>(data);
        Assert.AreEqual(5, temperature?.Value);
    }
}