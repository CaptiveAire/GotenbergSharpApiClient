﻿// Gotenberg.Sharp.API.Client - Copyright (c) 2019 CaptiveAire

using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using Gotenberg.Sharp.API.Client.Domain.Requests.Assets;
using Gotenberg.Sharp.API.Client.Infrastructure;
using JetBrains.Annotations;

namespace Gotenberg.Sharp.API.Client.Domain.Requests.Merge
{
    /// <summary>
    /// A request to merge the specified items into one pdf file
    /// </summary>
    public abstract class MergeRequest<TAsset> : IConvertToHttpContent where TAsset : class
    {
        readonly Func<TAsset, HttpContent> _converter;

        internal MergeRequest(Func<TAsset, HttpContent> converter)
            => _converter = converter;
       
        /// <summary>
        /// Gets the request configuration containing fields that all Gotenberg endpoints accept
        /// </summary>
        // ReSharper disable once MemberCanBeProtected.Global
        [UsedImplicitly]
        public RequestConfig Config { get; set; } = new RequestConfig();

        /// <summary>
        /// Key = file name; value = the document content
        /// </summary>
        [UsedImplicitly]
        public AssetRequest<TAsset> Items { get; set; }

        /// <summary>
        /// Transforms the merge items to http content items
        /// </summary>
        /// <returns></returns>
        public IEnumerable<HttpContent> ToHttpContent()
        {
            return this.Items.Where(_ => _.Value != null)
                .Select(_ =>
                {
                    var item = _converter(_.Value);
                    
                    item.Headers.ContentDisposition = new ContentDispositionHeaderValue(Constants.Http.Disposition.Types.FormData) {
                        Name = Constants.Gotenberg.FormFieldNames.Files,
                        FileName = _.Key
                    };

                    item.Headers.ContentType = new MediaTypeHeaderValue(Constants.Http.MediaTypes.ApplicationPdf);

                    return item;
                    
                }).Concat(Config.ToHttpContent());
        }
    }
}