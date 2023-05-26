using System.Reflection;
using Zyborg.Collections;

namespace Processor.Core;

public interface IRoutingTable
{
    public void AddMethod(string path, MethodInfo method);
    public List<MethodInfo>? Trace(string path);
    public void Print();
}

public class RoutingTable : IRoutingTable
{
    private readonly RadixTree<List<MethodInfo>> _table = new RadixTree<List<MethodInfo>>();

    public void AddMethod(string path, MethodInfo method)
    {
        var (value, found) = _table.GoGet(path);
        if (!found)
            _table.GoInsert(path, new List<MethodInfo> { method });
        else
        {
            value.Add(method);
        }
    }

    public List<MethodInfo>? Trace(string path)
    {
        var (value, found) = _table.GoGet(path);
        return found ? value : null;
    }

    public void Print()
    {
        StreamWriter standardOutput = new StreamWriter(Console.OpenStandardOutput());
        Console.SetOut(standardOutput);
        standardOutput.AutoFlush = true;
        _table.Print(standardOutput.BaseStream);
    }
}