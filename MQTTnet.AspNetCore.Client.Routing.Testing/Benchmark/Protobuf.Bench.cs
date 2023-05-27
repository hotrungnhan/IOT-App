using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Jobs;
using MQTTnet.AspNetCore.Client.Routing.Lib;
using MQTTnet.AspNetCore.Client.Routing.Tests.Model;

namespace MQTTnet.AspNetCore.Client.Routing.Testing.Benchmark;

[SimpleJob(RuntimeMoniker.NativeAot70)]
[SimpleJob(RuntimeMoniker.Net70)]
[RPlotExporter]
public class ProtobufBench
{
    [Params(1000000)] public int Length;

    [GlobalSetup]
    public void Setup()
    {
    }

    [Benchmark]
    public void Deserialize100Field()
    {
        for (var i = 0; i < Length; i++)
        {
            var serializer = new ProtobufSerializerAdapter();
            var data = serializer.Serialize(new Temperature
            {
                Value = 5,
                Unit = 4,
            });
            var temperature = serializer.Deserialize<Temperature>(data);
        }
    }

    [Benchmark]
    public void DeserializeRepeatedField()
    {
    }
}