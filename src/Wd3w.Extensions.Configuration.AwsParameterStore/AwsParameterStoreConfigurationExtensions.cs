using System;
using System.Threading.Tasks;
using Amazon.SimpleSystemsManagement;
using Amazon.SimpleSystemsManagement.Model;
using Microsoft.Extensions.Configuration;
using Wd3w.Extensions.Configuration.AsyncLoader;

namespace Wd3w.Extensions.Configuration.AwsParameterStore
{
    public static class AwsParameterStoreConfigurationExtensions
    {
        public static IConfigurationBuilder AddAwsJsonParameter(this IConfigurationBuilder builder, string name,
            bool withDecryption = false,
            bool optional = false,
            TimeSpan? period = default,
            string prefix = default,
            Func<AsyncLoaderExceptionContext, Task> onException = default)
        {
            return builder.AddAwsJsonParameter(() => new AmazonSimpleSystemsManagementClient(), name, withDecryption,
                optional, period, prefix, onException);
        }
        
        public static IConfigurationBuilder AddAwsJsonParameter(this IConfigurationBuilder builder, Func<IAmazonSimpleSystemsManagement> clientFactory, string name,
            bool withDecryption = false,
            bool optional = false, 
            TimeSpan? period = default, 
            string prefix = default,
            Func<AsyncLoaderExceptionContext, Task> onException = default)
        {

            builder.AddLoadAsync(async () =>
            {
                using var client = clientFactory();
                var response = await client.GetParameterAsync(new GetParameterRequest
                {
                    Name = name,
                    WithDecryption = withDecryption
                });

                return JsonConfigurationParser.Parse(response.Parameter.Value);
            }, optional, period, prefix, onException);
            
            return builder;
        }
    }
}