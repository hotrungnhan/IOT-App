using System.Runtime.Serialization;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Processor;

public class DataUnit
{
}

[JsonConverter(typeof(StringEnumConverter))]
public enum TemperatureUnit
{
    [EnumMember(Value = "F")] F,
    [EnumMember(Value = "C")] C
}

public class TemperatureDataUnit : DataUnit
{
    public float Value { get; set; }
    public TemperatureUnit Unit { get; set; }

    public override string ToString()
    {
        return $"{GetType()}:{Value}:{Enum.GetName(Unit)}";
    }
}

public class Model : Creatable
{
    public string Id { get; set; }
}

public interface Deleteable
{
    public DateTime DeletedAt { get; set; }
}

public interface Updatable
{
    public DateTime UpdatedAt { get; set; }
}

public class Creatable
{
    public DateTime CreatedAt { get; set; }
}