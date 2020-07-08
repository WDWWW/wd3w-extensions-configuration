using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Wd3w.Extensions.Configuration.AsyncLoader;
using Xunit;

namespace Wd3w.Extensions.Configuration.Test
{
    public class AsyncLoadTest
    {
        [Fact]
        public void AddLoadAsync_Should_LoadValue_When_LoaderIsSucceed()
        {
            var builder = new ConfigurationBuilder()
                .AddLoadAsync(async () => new Dictionary<string, string>
                {
                    {"A", "B"},
                    {"B:A", "A"}
                });

            var configuration = builder.Build();
            Assert.Equal("B", configuration.GetSection("A").Value);
            Assert.Equal("A", configuration.GetSection("B").GetSection("A").Value);
        }

        [Fact]
        public async Task AddLoadAsync_Should_ReloadValue_When_PeriodIsProvided()
        {
            var result = new Dictionary<string, string>
            {
                {"A", "B"},
            };
            var builder = new ConfigurationBuilder().AddLoadAsync(async () => result, period: TimeSpan.FromMilliseconds(100));
            var configuration = builder.Build();
            Assert.Equal("B", configuration.GetSection("A").Value);

            result["A"] = "C";
            await Task.Delay(150);
            Assert.Equal("C", configuration.GetSection("A").Value);
            
            result["A"] = "D";
            await Task.Delay(150);
            Assert.Equal("D", configuration.GetSection("A").Value);
        }

        [Fact]
        public async Task AddLoadAsync_Should_IgnoreException_When_OptionalIsTrue()
        {
            var builder = new ConfigurationBuilder()
                .AddLoadAsync(async () => throw new Exception(), optional: true);
            
            builder.Build();
        }
        
        [Fact]
        public async Task AddLoadAsync_Should_ThrowException_When_OptionalIsFalse()
        {
            var builder = new ConfigurationBuilder()
                .AddLoadAsync(async () => throw new Exception(), optional: false);

            Assert.Throws<Exception>(() => builder.Build());
        }
        
        [Fact]
        public async Task AddLoadAsync_Should_IgnoreException_When_OptionalIsTrueAndReloadPeriodIsProvided()
        {
            var onExceptionCallCount = 0;
            var builder = new ConfigurationBuilder()
                .AddLoadAsync(async () => throw new Exception(), optional: true, period: TimeSpan.FromMilliseconds(100),
                    onException: async context => onExceptionCallCount++);

            builder.Build();

            await Task.Delay(250);
            Assert.True(onExceptionCallCount >= 3);
        }
    }
}