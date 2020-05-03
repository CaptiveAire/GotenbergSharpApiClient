using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Gotenberg.Sharp.API.Client.Domain.Builders.FacetedBuilders;
using Gotenberg.Sharp.API.Client.Domain.Requests;

using JetBrains.Annotations;

namespace Gotenberg.Sharp.API.Client.Domain.Builders
{

    [PublicAPI]
    public class HtmlRequestBuilder: BaseBuilder<HtmlRequest>
    {
        readonly List<Task> _asyncTasks = new List<Task>();

        protected sealed override HtmlRequest Request { get; set; }

        public HtmlRequestBuilder() => this.Request = new HtmlRequest();

        [PublicAPI]
        public HtmlRequestBuilder(bool containsMarkdown = false) 
            => this.Request = new HtmlRequest(containsMarkdown);

        public HtmlRequestBuilder AddDocument(Action<DocumentBuilder> action)
        {
            if (action == null) throw new ArgumentNullException(nameof(action));

            action(new DocumentBuilder(this.Request));
            return this;
        }

        public HtmlRequestBuilder AddAsyncDocument(Func<DocumentBuilder, Task> asyncAction)
        {
            if (asyncAction == null) throw new ArgumentNullException(nameof(asyncAction));

            this._asyncTasks.Add(asyncAction(new DocumentBuilder(this.Request)));
            return this;
        }
        
        public HtmlRequestBuilder WithDimensions(Action<DimensionBuilder> action)
        {
            if (action == null) throw new ArgumentNullException(nameof(action));

            action(new DimensionBuilder(this.Request));
            return this;
        }

        public HtmlRequestBuilder WithAssets(Action<AssetBuilder> action)
        {
            if (action == null) throw new ArgumentNullException(nameof(action));
            action(new AssetBuilder(this.Request));
            return this;
        }

        public HtmlRequestBuilder WithAsyncAssets(Func<AssetBuilder, Task> asyncAction)
        {
            if (asyncAction == null) throw new ArgumentNullException(nameof(asyncAction));
            this._asyncTasks.Add(asyncAction(new AssetBuilder(this.Request)));
            return this;
        }

        public HtmlRequestBuilder ConfigureRequest(Action<ConfigBuilder> action)
        {
            if (action == null) throw new ArgumentNullException(nameof(action));
            action(new ConfigBuilder(this.Request));
            return this;
        }


        [PublicAPI]
        public HtmlRequest Build()
        {
            if (_asyncTasks.Any()) throw new InvalidOperationException("Call BuildAsync");
            if (Request.Content?.Body == null) throw new NullReferenceException("Request.Content or Content.Body is null");
            return Request;
        }


        [PublicAPI]
        public async Task<HtmlRequest> BuildAsync()
        {
            if (_asyncTasks.Count == 0) throw new InvalidOperationException("Call Build");
            if (this.Request.Content?.Body == null) throw new NullReferenceException("Request.Content or Content.Body is null");

            await Task.WhenAll(_asyncTasks).ConfigureAwait(false);

            return Request;
        }

    }
 }