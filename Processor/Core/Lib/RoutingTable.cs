using System.Reflection;
using Processor.Interface;
using Zyborg.Collections;

namespace Processor.Core.Lib;

public class RoutingTable : IRoutingTable
{
    private readonly RadixTree<List<MethodInfo>> _table = new();

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