using System;

using Gotenberg.Sharp.API.Client.Domain.Requests;
using Gotenberg.Sharp.API.Client.Extensions;

using JetBrains.Annotations;

namespace Gotenberg.Sharp.API.Client.Domain.Builders.FacetedBuilders
{
    public sealed class ConfigBuilder<TParent>: BaseBuilder<RequestBase>
    {
        public ConfigBuilder(RequestBase request, TParent parent)
        {
            this.Parent = parent;
            this.Request = request ?? throw new ArgumentNullException(nameof(request));
            this.Request.Config ??= new RequestConfig();
        }

        [PublicAPI]
        public TParent Parent { get; }

        [PublicAPI]
        public ConfigBuilder<TParent> TimeOut(float value)
        {
            this.Request.Config.TimeOut = value;
            return this;
       }
        
        [PublicAPI]
        public ConfigBuilder<TParent> ChromeRpccBufferSize(int value)
        {
            this.Request.Config.ChromeRpccBufferSize = value;
            return this;
        }
        
        [PublicAPI]
        public ConfigBuilder<TParent> PageRanges(string value)
        {
            this.Request.Config.PageRanges = value;
            return this;
        }

        [PublicAPI]
        public ConfigBuilder<TParent> ResultFileName(string value)
        {
            if(value.IsNotSet()) throw new ArgumentException("ResultFileName was null || empty");
            this.Request.Config.ResultFileName = value;
            return this;
        }
        
        [PublicAPI]
        public ConfigBuilder<TParent> WebHook(string value)
        {
            if(value.IsNotSet()) throw new ArgumentException("WebHook was null || empty");
            if(!Uri.IsWellFormedUriString(value, UriKind.Absolute)) throw new ArgumentException("WebHook was not well formed");
            this.Request.Config.WebHook =new Uri(value);
            return this;
        }
        
        [PublicAPI]
        public ConfigBuilder<TParent> WebHook(Uri value)
        {
            if(value == null) throw new ArgumentNullException(nameof(value));
            if(!value.IsAbsoluteUri) throw new ArgumentException("WebHook must be absolute");
            this.Request.Config.WebHook = value;
            return this;
        }

        [PublicAPI]
        public ConfigBuilder<TParent> WebHookTimeOut(float value)
        {
            this.Request.Config.WebHookTimeOut = value;
            return this;
        }
 
    }
}