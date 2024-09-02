# AutoInjectRegister
Auto register classes and interfaces for dependency injection. Add the auto inject attribute to the top of any class file to be marked for auto injection. This will help you know the scope of each class just by looking at the class file.

## Adding Auto Inject to DI

Add to your program or startup file

  ```c-sharp
   builder.Services.AutoInjectRegisterServices();
  ```

## Register your classes

Add the auto inject attribute to your classes

  ```c-sharp
    // class with interface implementation
    [AutoInject(Lifetime.Scoped)]
    internal class ScopedTestClass : ScopedTestInterface
    {
        ....
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