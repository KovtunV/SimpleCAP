# SimpleCAP

[![Build Status](https://travis-ci.org/KovtunV/SimpleCAP.svg?branch=master)](https://travis-ci.org/KovtunV/SimpleCAP)
[![NuGet version (SimpleCAP)](https://img.shields.io/nuget/v/SimpleCAP.svg?style=flat-square)](https://www.nuget.org/packages/SimpleCAP)
[![NuGet Download](https://img.shields.io/nuget/dt/SimpleCAP.svg?style=flat-square)](https://www.nuget.org/packages/SimpleCAP)

Use your distributed system easier. based on [DotNetCore.CAP](https://cap.dotnetcore.xyz/).

<!--ts-->
   * [Getting Started](#Getting-Started)
      * [Configuration](#Configuration)
      * [Handlers](#Handlers)
      * [Event bus](#Event-bus)
<!--te-->

## Getting Started
### Configuration
First, you need to configure CAP in your Startup.cs：

```csharp
public void ConfigureServices(IServiceCollection services)
{
    //......

    services.AddSimpleCap(q =>
    {
        q.UseInMemoryStorage(); // If you don't have a storage
        q.UseKafka("ConnectionString"); // If you use kafka
    });
    
    // Optional
    // services.Configure<CallbackOptions>(q => q.Timeout = TimeSpan.FromMilliseconds(10));
}
```

### Handlers
Implement handlers. Use one of two interfaces:
* `ICapEventHandler<MyRequestModel, MyResponseModel>`
* `ICapEventHandler<MyRequestModel>`

```csharp
// Optional
// [OverrideSubscribe(Name = "my topic name", Group = "my group name")]
public class MyHandler : ICapEventHandler<MyRequestModel, MyResponseModel>
{
    public Task<MyResponseModel> HandleAsync(MyRequestModel model)
    {
        var res = new MyResponseModel {Message = $"Hi {model.Name}! Are you {model.Age} years old?" };
        return Task.FromResult(res);
    }
}
```

By default the topic name is **MyRequestModel** and Group is **null**, but you can override this by meance of using the attribute **OverrideSubscribe**. 
Furthermore, you can use this attribute with request class **MyRequestModel**.

### Event bus
Inject `ISimpleCapBus` in your Controller, Service, etc. 

This interface has three methods:
* `Task SendAsync<TRequest>(TRequest model, string name = null)`
* `Task<TResponse> SendAsync<TRequest, TResponse>(TRequest model, string name = null)`
* `SimpleCapBusWrapper<TRequest> Request<TRequest>(TRequest model, string name = null)`

Controller for example:
```scharp
[ApiController]
public class Web1Controller : ControllerBase
{
    private readonly ISimpleCapBus _bus;

    public Web1Controller(ISimpleCapBus bus)
    {
        _bus = bus;
    }

    [HttpGet("/api/send")]
    public async Task<MyResponseModel> Get()
    {
        var model = new MyRequestModel
        {
            Name = "Vitaly",
            Age = 26.256m
        };

        // if you don't need a result
        // await _bus.SendAsync(model);
        // return default;

        // if you need a result
        // var res = await _bus.SendAsync<MyRequestModel, MyResponseModel>(model);
        // return res;

        // If you don't want to write a request type in generic method
        // var res = await _bus.Request(model).SendAsync<MyResponseModel>();
        // return res;
    }
}

```
