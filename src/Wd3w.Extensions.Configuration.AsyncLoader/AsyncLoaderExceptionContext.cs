using System;

namespace Wd3w.Extensions.Configuration.AsyncLoader
{
    public class AsyncLoaderExceptionContext
    {
        public Exception Exception { get; set; }

        public bool Ignore { get; set; }

        public bool Reload { get; set; }
    }
}