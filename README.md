# Watts.Shared.Functions.DependencyInjection
Basic dependency injection for Azure functions V2

## About
This is basic dependency injection library we are using in our projects for Azure Functions V2.

# Usage
First of all you have to declare ```Startup``` class for your azure function

```
[assembly: WebJobsStartup(typeof(Startup))]
namespace Watts.Utilities.Readings.Integration
{
    public class Startup : IWebJobsStartup
    {
        public void Configure(IWebJobsBuilder builder)
        {
            builder.UseServiceProvider<AutofacScopedServiceProvider>();
        }
    }
}
```

and then use ```builder.UseServiceProvider<>()``` extension to register custom ```IScopedServiceProvider```

For Autofac implementation can be the following

```
 public class AutofacScopedServiceProvider : IScopedServiceProvider
 {
     public IServiceProvider CurrentFor(IConfiguration configuration)
     {
         var builder = new ContainerBuilder();
         builder.RegisterModule(new PersistenceModule(configuration));
         return new AutofacServiceProvider(builder.Build());
     }
 }
```

```IScopedServiceProvider``` has only one method you have to implement ```IServiceProvider CurrentFor(IConfiguration configuration)```, as you can see you get instence of ```IConfiguration```, so you can easily access your configurations from ```local.settings.json```

```
public class PersistenceModule
{
   public PersistenceModule(IConfiguration configuration)
   {
   }

   protected override void Load(ContainerBuilder builder)
   {
      builder.Register(c => new DatabaseContext(Configuration["DatabaseConnection"]));

      builder.RegisterAssemblyTypes(typeof(StorageRepository<,>).Assembly)
		        .Where(t => t.Name.EndsWith("Repository") && !t.IsAbstract)
		        .AsImplementedInterfaces();
   }
}
```

After all configuration is done, simply use ```[Import]``` attribute with you function method

```
public static async Task Integrate(
            [ServiceBusTrigger(
                "%topic%",
                "%subscription%",
                Connection = "ServiceBusConnection")]
            Message meterContractImportedMessage,
            [Import] ILogger log,
            [Import] IService service)
        {...}
```


