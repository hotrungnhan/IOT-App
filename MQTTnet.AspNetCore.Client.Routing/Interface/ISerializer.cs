namespace MQTTnet.AspNetCore.Client.Routing.Interface;

public interface ISerializer
{
    public byte[] Serialize<T>(T obj);
    public T? Deserialize<T>(byte[] data);
}