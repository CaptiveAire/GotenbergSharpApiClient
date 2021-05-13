using Gotenberg.Sharp.API.Client.Domain.Requests;

using JetBrains.Annotations;

namespace Gotenberg.Sharp.API.Client.Domain.Builders
{
    public abstract class BaseBuilder<TRequest> where TRequest : RequestBase
    {
        protected virtual TRequest Request { get; [UsedImplicitly] set; }

        protected const string CallBuildAsyncErrorMessage = "Request has asynchronous items. Call BuildAsync instead.";
    }
}