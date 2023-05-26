namespace Processor.Core.Attribute;

public class FromEventAttribute :
    System.Attribute
{
}

public class FromTopicAttribute : System.Attribute
{
}

public class FromPathAttribute : System.Attribute
{
    public string Key { get; set; }
}