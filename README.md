# Watts.Shared.Functions.DependencyInjection
Basic dependency injection for Azure functions V2

## About
This is basic dependency injection library we are using in our projects for Azure Functions V2. Available as [nuget](https://www.nuget.org/packages/Watts.Shared.Functions.DependencyInjection)

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
   private readonly IConfiguration _configuration;
   
   public PersistenceModule(IConfiguration configuration)
   {
      _configuration = configuration;
   }

   protected override void Load(ContainerBuilder builder)
   {
      builder.Register(c => new DatabaseContext(_configuration["DatabaseConnection"]));

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

# Azure
When you are deploying your function into Azure environment you will encountered issue that ```extensions.json``` is not created properly. The issue is discussed [here](https://github.com/Azure/azure-functions-host/issues/3386#issuecomment-419565714). To solve that we are using custom build targets, which will be included into your project with the nuget.
