# AutoInjectRegister
Auto register classes and interfaces for dependency injection. Add the auto inject attribute to the top of any class file to be marked for auto injection. This will help you know the scope of each class just by looking at the class file.

For .Net and C#.

## Adding Auto Inject to DI

Add to your program or startup file

  ```c-sharp
   builder.Services.AutoInjectRegisterServices();
  ```

Alternatively, if you would like to be more specific with which assembly you would like added, add a type from an assembly you want added. This will only register files in that assembly, so be aware that it won't take everything plus that assembly. It will only take the assembly specified.

 ```c-sharp
   builder.Services.AutoInjectRegisterServices(typeof(IDbReader), typeof(ICache), typeof(IProxy));
  ```

  If you would like to use exclude specific types you will need to pass in an options class into the register services method.

   ```c-sharp
   public class AutoInjectorOptions
   {
       public IEnumerable<Type> TypesToScan { get; set; } = [];
       public IEnumerable<Type> TypesToExclude { get; set; } = [];
   }
  ```

  You can then pass the options class in like this.

  ```c-sharp
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

```c-sharp  
    [AutoInjectTransient]

    [AutoInjectScoped]

    [AutoInjectsSingleton]
  ```

You can implement them like so.

```c-sharp  
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

  ```c-sharp  
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

  ```c-sharp
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

  ```c-sharp
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

## Avaliable Service lifetimes

This is just from the microsoft enum, ServiceLifetime.

  ```c-sharp
    Transient,
    Scoped,
    Singleton
  ```

  ## Avaliable AddTypes

  ```c-sharp
    Add,
    TryAdd
  ```
