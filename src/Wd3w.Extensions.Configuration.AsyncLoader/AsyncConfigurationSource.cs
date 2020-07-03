using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;

namespace Wd3w.Extensions.Configuration.AsyncLoader
{
    public class AsyncConfigurationSource : IConfigurationSource
    {
        public string Prefix { get; set; }
        
        public bool Optional { get; set; }
        
        public TimeSpan? ReloadPeriod { get; set; }

        public Func<Task<IDictionary<string, string>>> LoadAsync { get; set; }

        public Func<AsyncLoaderExceptionContext, Task> OnLoadExceptionAsync { get; set; }
        
        public IConfigurationProvider Build(IConfigurationBuilder builder)
        {
            return new AsyncConfigurationProvider(this);
        }
    }
}