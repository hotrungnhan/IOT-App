namespace Processor.Core.Lib;

public interface ISerializer
{
    public byte[] Serialize<T>(T obj);
    public T? Deserialize<T>(Byte[] data);
}