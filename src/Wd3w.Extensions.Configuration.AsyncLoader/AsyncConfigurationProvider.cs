using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Primitives;

namespace Wd3w.Extensions.Configuration.AsyncLoader
{
    public class AsyncConfigurationProvider : ConfigurationProvider
    {
        private readonly AsyncConfigurationSource _source;

        public AsyncConfigurationProvider(AsyncConfigurationSource source)
        {
            _source = source;

            if (!source.ReloadPeriod.HasValue)
                return;
            
            ChangeToken.OnChange(
                () => new CancellationChangeToken(new CancellationTokenSource(source.ReloadPeriod.Value).Token), 
                async () => await LoadAsync(true).ConfigureAwait(false));
        }


        public override void Load()
        {
            LoadAsync(false).ConfigureAwait(false).GetAwaiter().GetResult();
        }

        public async Task LoadAsync(bool reload)
        {
            try
            {
                var data = await _source.LoadAsync().ConfigureAwait(false);
                if (data != null && !EquivalentTo(Data, data))
                {
                    Data = data;
                    OnReload();
                }
            }
            catch (Exception e)
            {
                var context = new AsyncLoaderExceptionContext
                {
                    Exception = e,
                    Ignore = _source.Optional,
                    Reload = reload
                };
                if (_source.OnLoadExceptionAsync != default)
                    await _source.OnLoadExceptionAsync(context);

                if (!context.Ignore)
                    throw;

            }
        }

        private static bool EquivalentTo(IDictionary<string, string> first, IDictionary<string, string> second)
        {
            if (first == second) return true;
            if (first == null || second == null) return false;
            if (first.Count != second.Count) return false;
            foreach (var (key, value) in first)
            {
                if (!second.TryGetValue(key, out var secondValue)) return false;
                if (value != secondValue) return false;
            }
            return true;
        }
    }
}