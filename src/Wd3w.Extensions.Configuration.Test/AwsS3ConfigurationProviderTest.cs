using Microsoft.Extensions.Configuration;
using Wd3w.Extensions.Configuration.S3;
using Xunit;

namespace Wd3w.Extensions.Configuration.Test
{
    public class AwsS3ConfigurationProviderTest
    {
        [Fact]
        public void LoadFromS3()
        {
            var configuration = new ConfigurationBuilder()
                .AddAwsS3("wd3w-configuration-test-fixture", "sample.json")
                .Build();
            
            Assert.Equal("b", configuration.GetSection("a").Value);
            Assert.Equal("4", configuration.GetSection("c").Value);
        }
    }
}