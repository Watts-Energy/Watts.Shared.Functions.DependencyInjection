using System;
using Microsoft.Azure.WebJobs.Description;

namespace Watts.Shared.Functions.DependencyInjection.Composition
{
    [Binding]
    [AttributeUsage(AttributeTargets.Parameter)]
    public class ImportAttribute: Attribute { }
}