using System;
using System.Threading.Tasks;
using Amazon.S3;
using Amazon.S3.Model;
using Microsoft.Extensions.Configuration;
using Wd3w.Extensions.Configuration.AsyncLoader;

namespace Wd3w.Extensions.Configuration.S3
{
    public static class S3ConfigurationExtensions
    {
        public static IConfigurationBuilder AddAwsS3(this IConfigurationBuilder builder,
            string bucket, 
            string key,
            bool optional = false,
            TimeSpan? period = default,
            string prefix = default,
            Func<AsyncLoaderExceptionContext, Task> onException = default)
        {
            return builder.AddAwsS3(() => new AmazonS3Client(), new GetObjectRequest
            {
                BucketName = bucket,
                Key = key
            }, optional, period, prefix, onException);
        }
        
        public static IConfigurationBuilder AddAwsS3(this IConfigurationBuilder builder,
            Func<AmazonS3Client> clientCreator,
            string bucket, 
            string key,
            bool optional = false,
            TimeSpan? period = default,
            string prefix = default,
            Func<AsyncLoaderExceptionContext, Task> onException = default)
        {
            return builder.AddAwsS3(clientCreator, new GetObjectRequest
            {
                BucketName = bucket,
                Key = key
            }, optional, period, prefix, onException);
        }

        public static IConfigurationBuilder AddAwsS3(this IConfigurationBuilder builder, 
            Func<IAmazonS3> clientCreator,
            GetObjectRequest objectRequest, 
            bool optional = false, 
            TimeSpan? period = default, 
            string prefix = default,
            Func<AsyncLoaderExceptionContext, Task> onException = default)
        {
            builder.AddLoadAsync(async () =>
            {
                using var client = clientCreator();
                var response = await client.GetObjectAsync(objectRequest);
                return JsonConfigurationParser.Parse(response.ResponseStream);
            }, optional, period, prefix, onException);
            return builder;
        }
    }
}