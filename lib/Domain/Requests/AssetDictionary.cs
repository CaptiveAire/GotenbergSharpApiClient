using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;

using Gotenberg.Sharp.API.Client.Domain.Requests.Content;
using Gotenberg.Sharp.API.Client.Extensions;
using Gotenberg.Sharp.API.Client.Infrastructure;

using JetBrains.Annotations;

using Microsoft.AspNetCore.StaticFiles;

namespace Gotenberg.Sharp.API.Client.Domain.Requests
{
    public sealed class AssetDictionary: Dictionary<string, ContentItem>, IConvertToHttpContent 
    {
        readonly FileExtensionContentTypeProvider _contentTypeProvider = new FileExtensionContentTypeProvider();

        [UsedImplicitly]
        public void AddRange([NotNull] IEnumerable<KeyValuePair<string, ContentItem>> items)
        {
            if (items == null) throw new ArgumentNullException(nameof(items));
            foreach (var item in items.ToList())
            {
                this.Add(item.Key, item.Value);
            }
        }
        
        public IEnumerable<HttpContent> ToHttpContent()
        {
            return this.Select(item =>
                {
                    _contentTypeProvider.TryGetContentType(item.Key, out var contentType);
                    return new {Asset = item, MediaType = contentType};
                })
                .Where(i => i.MediaType.IsSet())
                .Select(item =>
                {
                    var asset = item.Asset.Value.ToHttpContentItem();

                    asset.Headers.ContentDisposition =
                        new ContentDispositionHeaderValue(Constants.HttpContent.Disposition.Types.FormData)
                        {
                            Name = Constants.Gotenberg.FormFieldNames.Files,
                            FileName = item.Asset.Key
                        };

                    asset.Headers.ContentType = new MediaTypeHeaderValue(item.MediaType);

                    return asset;
                });
        }

        internal AssetDictionary FluentAddRange([NotNull] IEnumerable<KeyValuePair<string, ContentItem>> items)
        {
            if (items == null) throw new ArgumentNullException(nameof(items));
            foreach (var item in items.ToList())
            {
                this.Add(item.Key, item.Value);
            }

            return this;
        }
    }
}