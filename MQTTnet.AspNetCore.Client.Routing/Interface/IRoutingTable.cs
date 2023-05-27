using System.Reflection;

namespace MQTTnet.AspNetCore.Client.Routing.Interface;

public interface IRoutingTable
{
    public void AddMethod(string path, MethodInfo method);
    public List<MethodInfo>? Trace(string path);
    public void Print();
}