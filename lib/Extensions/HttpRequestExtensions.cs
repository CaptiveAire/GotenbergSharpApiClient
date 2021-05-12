using System;
using System.Collections.Generic;
using System.Net.Http;

using JetBrains.Annotations;

namespace Gotenberg.Sharp.API.Client.Extensions
{
    /// <summary>
    ///     Request extensions
    /// </summary>
    public static class HttpRequestExtensions
    {
        const string TimeoutPropertyKey = "RequestTimeout";

        /// <summary>
        /// Sets the timeout.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <param name="timeout">The timeout.</param>
        /// <exception cref="ArgumentOutOfRangeException">request</exception>
        [UsedImplicitly]
        public static void SetTimeout(this HttpRequestMessage request, TimeSpan? timeout)
        {
            if (request == null) throw new ArgumentNullException(nameof(request));

            #if NET5_0_OR_GREATER
                request.Options.TryAdd(TimeoutPropertyKey, timeout);
            #else
                request.Properties[TimeoutPropertyKey] = timeout;
            #endif
        }

        /// <summary>
        /// Gets the timeout.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns></returns>
        public static TimeSpan? GetTimeout(this HttpRequestMessage request)
        {
            if (request == null) throw new ArgumentNullException(nameof(request));

                #if NET5_0_OR_GREATER
                    if (request.Options.TryGetValue(new HttpRequestOptionsKey<TimeSpan>(TimeoutPropertyKey), out var timeout))
                    {
                        return timeout;
                    }
                #else
                    if (request.Properties.TryGetValue(TimeoutPropertyKey, out var value) && value is TimeSpan timeout)
                    {
                        return timeout;
                    }
               #endif

            return null;
        }
    }
}