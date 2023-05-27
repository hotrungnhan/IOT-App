using System.Text;
using MQTTnet.AspNetCore.Client.Routing.Interface;
using Newtonsoft.Json;

namespace MQTTnet.AspNetCore.Client.Routing.Lib;

public class JsonSerializerAdapter : ISerializer
{
    public byte[] Serialize<T>(T obj)
    {
        var s = JsonConvert.SerializeObject(obj);

        return Encoding.UTF8.GetBytes(s);
    }

    public T? Deserialize<T>(byte[] data)
    {
        var s = Encoding.UTF8.GetString(data);
        return JsonConvert.DeserializeObject<T>(s);
    }
}