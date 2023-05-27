namespace MQTTnet.AspNetCore.Client.Routing.Attribute;

public class FromEventAttribute :
    System.Attribute
{
}

public class FromPayloadAttribute :
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