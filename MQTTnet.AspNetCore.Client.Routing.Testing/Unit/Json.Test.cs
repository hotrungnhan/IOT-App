using MQTTnet.AspNetCore.Client.Routing.Lib;
using MQTTnet.AspNetCore.Client.Routing.Testing.Model;

namespace MQTTnet.AspNetCore.Client.Routing.Testing.Unit;

[TestClass]
public class JsonTest
{
    [TestMethod]
    public void TestSerialize()
    {
        var serializer = new JsonSerializerAdapter();
        var data = serializer.Serialize(new Temperature
        {
            Value = 5,
            Unit = 4,
        });
        var temperature = serializer.Deserialize<Temperature>(data);
        Assert.AreEqual(5, temperature?.Value);
    }
}