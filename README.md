# AutoInjectRegister
Auto register classes and interfaces for dependency injection. Add the auto inject attribute to the top of any class file to be marked for auto injection. This will help you know the scope of each class just by looking at the class file.

For .Net and C#.

## Adding Auto Inject to DI
Add to your program or startup file

  ```csharp
   builder.Services.AutoInjectRegisterServices();
  ```

Alternatively, if you would like to be more specific with which assembly you would like added, add a type from an assembly you want added. This will only register files in that assembly, so be aware that it won't take everything plus that assembly. It will only take the assembly specified.

 ```csharp
   builder.Services.AutoInjectRegisterServices(typeof(IDbReader), typeof(ICache), typeof(IProxy));
  ```

If you would like to use exclude specific types you will need to pass in an options class into the register services method. You can use the InclusionType enum to specify you want to ONLY use the types passed into types to scan.

```csharp
public class AutoInjectorOptions
{
    public IEnumerable<Type> TypesToScan { get; set; } = [];
    public IEnumerable<Type> TypesToExclude { get; set; } = [];
    public InclusionType InclusionType { get; set; } = InclusionType.All;
}
```

You can then pass the options in as function.

```csharp
builder.Services.AutoInjectRegisterServices(options =>
    {
        options.TypesToScan = [typeof(MyClass), typeof(YourClass)];
        options.InclusionType = [typeof(ExclusionClass), typeof(ExclusionClass)];
    });
```


Or create the object yourself.

```csharp
builder.Services.AutoInjectRegisterServices(
    new AutoInjectorOptions 
    {
        TypesToScan = [typeof(MyClass), typeof(YourClass)],
        TypesToExclude = [typeof(ExclusionClass), typeof(ExclusionClass)] 
    }
);
```

## Register your classes
Add the auto inject attribute to your classes in two different ways.

### No paramter attribute
You will have access to three attributes.

```csharp  
[AutoInjectTransient]

[AutoInjectScoped]

[AutoInjectsSingleton]
```

You can implement them like so.

```csharp  
[AutoInjectTransient]
internal class ExampleTransientClass : IExampleTransientInterface
{
    ...
}

[AutoInjectScoped]
internal class ExampleScopedClass : IExampleScopedInterface
{
    ...
}

    
    
[AutoInjectsSingleton]
internal class SingletonClassOnly
{
    ...
}
```

Whilst you can implement multiple attributes of different lifetimes, avoid doing so as it could cause confusion of the lifetime of the service. If a class does have multiple attributes it will be registered in the order of the enum, ServiceLifetime. 

```csharp  
[AutoInjectScoped]
[AutoInjectsSingleton] // This should be what it is registered as
[AutoInjectTransient]
internal class ExampleMultipleServiceClass : IExampleMultipleServiceInterface
{
    ...
}
```


### Base attribute with a parameter
You can also use the base class if you prefer that. The benefit of this is you will get compiler issues if you use the attribute multiple times.

```csharp
// class with interface implementation
[AutoInject(ServiceLifetime.Scoped)]
internal class ScopedTestClass : ScopedTestInterface
{
    ...
}

// class only
[AutoInject(Lifetime.Scoped)]
internal class ScopedTestClassOnly
{
    ...
}
```

### Using TryAddService instead of AddService
In the case you want to try add the service instead of add, you can use the addtype enum as a parameter for all attributes types.

```csharp
// class with interface implementation
[AutoInject(ServiceLifetime.Scoped, AddType.TryAdd)]
internal class ScopedTestClass : ScopedTestInterface
{
    ...
}

// class only
[AutoInjectSingleton(AddType.TryAdd)]
internal class SingletonClass
{
    ...
}
```

## Enums Types Available

### Service lifetimes
This is just from the microsoft enum, ServiceLifetime.

```csharp
Transient,
Scoped,
Singleton
```

### AddTypes

```csharp
Add,
TryAdd
```

### InclusionTypes

```csharp
All,
TypesToScanOnly
```
