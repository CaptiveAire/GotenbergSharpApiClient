﻿using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Linq;

using Gotenberg.Sharp.API.Client.Domain.Requests;
using Gotenberg.Sharp.API.Client.Domain.Requests.Content;
using Gotenberg.Sharp.API.Client.Extensions;

using JetBrains.Annotations;

namespace Gotenberg.Sharp.API.Client.Domain.Builders.FacetedBuilders
{
    public sealed class AssetBuilder<TParent>: BaseBuilder<RequestBase>
    {
        public AssetBuilder(RequestBase request, TParent parent)
        {
            this.Parent = parent;
            this.Request = request ?? throw new ArgumentNullException(nameof(request));
            Request.Assets ??= new AssetDictionary();
        }

        [PublicAPI]
        public TParent Parent { get; }

        #region one asset

        /// <remarks>
        ///     CA1307 b/c of multi-targeting net standard 2.0 has no overload for StringComparison
        /// </remarks>
        [PublicAPI]
        [SuppressMessage("Globalization", "CA1307:Specify StringComparison", Justification = "<Pending>")]
        public AssetBuilder<TParent> AddItem(string name, ContentItem value)
        {
            if (name.IsNotSet() || new FileInfo(name).Extension.IsNotSet() || name.Contains("/"))
            {
                throw new ArgumentException("All keys in the asset dictionary must be relative file names with extensions");
            }

            this.Request.Assets.Add(name, value);

            return this;
        }

        [PublicAPI]
        public AssetBuilder<TParent> AddItem(string name, string value) => AddItem(name, new ContentItem(value));

        [PublicAPI]
        public AssetBuilder<TParent> AddItem(string name, byte[] value) => AddItem(name, new ContentItem(value));

        [PublicAPI]
        public AssetBuilder<TParent> AddItem(string name, Stream value) => AddItem(name, new ContentItem(value));

        #endregion

        #region 'n' assets

        #region from dictionaries

        [PublicAPI]
        public AssetBuilder<TParent> AddItems(Dictionary<string, ContentItem> items)
        {
            foreach (var item in items.IfNullEmpty())
            {
                this.AddItem(item.Key, item.Value);
            }

            return this;
        }

        [PublicAPI]
        public AssetBuilder<TParent> AddItems(Dictionary<string, string> assets) =>
                AddItems(assets?.ToDictionary(a => a.Key, a => new ContentItem(a.Value)) ?? throw new ArgumentException("Assets can not be null"));

        [PublicAPI]
        public AssetBuilder<TParent> AddItems(Dictionary<string, byte[]> assets) =>
                AddItems(assets?.ToDictionary(a => a.Key, a => new ContentItem(a.Value)) ?? throw new ArgumentException("Assets can not be null"));

        [PublicAPI]
        public AssetBuilder<TParent> AddItems(Dictionary<string, Stream> assets) =>
                AddItems(assets?.ToDictionary(a => a.Key, a => new ContentItem(a.Value)) ?? throw new ArgumentException("Assets can not be null"));

        #endregion

        #region from KVP enumerables

        [PublicAPI]
        public AssetBuilder<TParent> AddItems(IEnumerable<KeyValuePair<string, ContentItem>> assets) =>
                AddItems(new Dictionary<string, ContentItem>(assets?.ToDictionary(a => a.Key, a => a.Value) ?? throw new ArgumentException("Assets can not be null")));

        [PublicAPI]
        public AssetBuilder<TParent> AddItems(IEnumerable<KeyValuePair<string, string>> assets) =>
                AddItems(new Dictionary<string, ContentItem>(assets?.ToDictionary(a => a.Key, a => new ContentItem(a.Value)) ?? throw new ArgumentException("Assets can not be null")));

        [PublicAPI]
        public AssetBuilder<TParent> AddItems(IEnumerable<KeyValuePair<string, byte[]>> assets) =>
                AddItems(new Dictionary<string, ContentItem>(assets?.ToDictionary(a => a.Key, a => new ContentItem(a.Value)) ?? throw new ArgumentException("Assets can not be null")));

        [PublicAPI]
        public AssetBuilder<TParent> AddItems(IEnumerable<KeyValuePair<string, Stream>> assets) =>
                AddItems(new Dictionary<string, ContentItem>(assets?.ToDictionary(s => s.Key, a => new ContentItem(a.Value)) ?? throw new ArgumentException("Assets can not be null")));

        #endregion

        #endregion
    }
}