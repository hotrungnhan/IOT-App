# MQTT Routing for Subcriber 

## Why we have this project ?

I make my IOT project and choose C#,MQTT as my core to process stream data from IOT device.

```
                  Authenticate HTTP Client
                            ^
                            |
                            |           -> Subcriber Processor via C# dotnet 
publisher IOT device -> Broker EMQX     -> Subcriber Processor via C# dotnet -> Click house DB 
                                        -> Subcriber Processor via C# dotnet    
                            |
                            |
                            V
                    Application Mobile 
```

I kinda frustrated when using MQTTNet lib to connect to MQTT, cus it have no Routing, no Serializer,... by default.
So i decide to make my own project to use what i need.

## Package Main Architecture


```
                          -> Topic 3,4 -> handler 3
MQTT event -> Multiplexer -> Topic 2,3 -> handler 2 , handler 4
                          -> Topic 1,b -> Handler 1
```
## Goal /Feature

* Fast-Enough multiplexer routing (implement via Radix)
* Topic Path params
* Controller Constructor DI (like ASPdotnet)
* Queue event
* Protobuff/ Json Serilize (have two option, depend use case).
* Allow multiple MQTT client at the same time (optional feature) 
* full Unit Test
## PS 
> This is very begining state of this project, only routing work now.
