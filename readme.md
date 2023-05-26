# MQTT Routing for Subcriber 

## Why we have this project ?

enhanced features in the context of an IoT project implemented in C# using MQTT as the core protocol for processing streaming data from IoT devices.

The project architecture involves the following components:

```
                  Authenticate HTTP Client
                            ^
                            |
                            |          -> Subcriber Processor via C# dotnet 
publisher IOT device ->   Broker EMQX  -> Subcriber Processor via C# dotnet -> Click house DB 
                            |           -> Subcriber Processor via C# dotnet    
                            |
                            |
                            V
                    Application Mobile 
```

While working with the MQTTNet library to connect to MQTT, it was found that the library lacks built-in support for routing and serialization, which led to frustration. Consequently, a decision was made to create a custom project that fulfills the required functionalities

## Package Architecture

The package architecture revolves around the concept of a multiplexer, which enables efficient routing of MQTT events based on topics to their respective handlers. The structure is as follows:

```
                          -> Topic 3,4 -> handler 3
MQTT event -> Multiplexer -> Topic 2,3 -> handler 2 , handler 4
                          -> Topic 1,b -> Handler 1
```
## Goal /Feature

* Fast and efficient multiplexer routing utilizing a Radix implementation
* Support for topic path parameters
* Dependency Injection in controller constructors, akin to ASP.NET
* Event queuing
* Protobuf and JSON serialization options (depending on use case)
* Ability to handle multiple MQTT clients concurrently (optional feature)
* Comprehensive unit testing

## PS 
> Please note that the project is currently in its initial stages, with only the routing functionality implemented so far.
