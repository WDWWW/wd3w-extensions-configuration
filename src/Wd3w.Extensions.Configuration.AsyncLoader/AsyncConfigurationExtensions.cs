using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;

namespace Wd3w.Extensions.Configuration.AsyncLoader
{
    public static class AsyncConfigurationExtensions
    {
        public static IConfigurationBuilder AddLoadAsync(this IConfigurationBuilder builder,
            Func<Task<IDictionary<string, string>>> loader, 
            bool optional = false, 
            TimeSpan? period = default,
            string prefix = default, 
            Func<AsyncLoaderExceptionContext, Task> onException = default)
        {
            return builder.Add(new AsyncConfigurationSource
            {
                LoadAsync = loader,
                Optional = optional,
                Prefix = prefix,
                ReloadPeriod = period,
                OnLoadExceptionAsync = onException
            });
        }
    }
}