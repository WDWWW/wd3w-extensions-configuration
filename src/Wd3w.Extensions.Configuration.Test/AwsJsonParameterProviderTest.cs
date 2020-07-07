using Microsoft.Extensions.Configuration;
using Wd3w.Extensions.Configuration.AwsParameterStore;
using Xunit;

namespace Wd3w.Extensions.Configuration.Test
{
    public class AwsJsonParameterProviderTest
    {
        [Fact]
        public void LoadFromParamterStore()
        {
            var configuration = new ConfigurationBuilder()
                .AddAwsJsonParameter("wd3w-configuration-library-key")
                .Build();

            Assert.Equal("b", configuration.GetSection("a").Value);
            Assert.Equal("4", configuration.GetSection("c").Value);
        }
    }
}
