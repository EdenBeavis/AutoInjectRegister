# AutoInjectRegister
Auto register classes and interfaces for dependency injection. Add the auto inject attribute to the top of any class file to be marked for auto injection. This will help you know the scope of each class just by looking at the class file.

For .Net and C#.

## Adding Auto Inject to DI

Add to your program or startup file

  ```c-sharp
   builder.Services.AutoInjectRegisterServices();
  ```

Alternatively, if you would like to be more specific with which assembly you would like added, add a type from an assembly you want added.

 ```c-sharp
   builder.Services.AutoInjectRegisterServices(typeof(IDbReader), typeof(ICache), typeof(IProxy));
  ```

This will only register files in that assembly, so be aware that it won't take everything plus that assembly. It will only take the assembly specified.


## Register your classes

Add the auto inject attribute to your classes

  ```c-sharp
    // class with interface implementation
    [AutoInject(Lifetime.Scoped)]
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

## Avaliable lifetimes

  ```c-sharp
    Transient,
    Scoped,
    Singleton
  ```
