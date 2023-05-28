// Copyright (c) .NET Foundation. All rights reserved. Licensed under the Apache License, Version 2.0. See License.txt
// in the project root for license information.

namespace MQTTnet.AspNetCore.Client.Routing.Interface
{
    public interface ITypeActivatorCache
    {
        TInstance CreateInstance<TInstance>(IServiceProvider serviceProvider, Type implementationType);
    }
}