﻿// Gotenberg.Sharp.Api.Client - Copyright (c) 2020 CaptiveAire

using System.Collections.Generic;
using System.IO;
using System.Linq;
using Gotenberg.Sharp.API.Client.Domain.Requests.Content;

namespace Gotenberg.Sharp.API.Client.Domain.Requests
{
    public class MergeOfficeRequest: MergeRequest, IMergeOfficeRequest  
    {
       
        static readonly string[] _allowedExtensions = {".txt",".rtf",".fodt",".doc",".docx",".odt",".xls",".xlsx",".ods",".ppt",".pptx",".odp"};

        internal MergeOfficeRequest(Dictionary<string, ContentItem> items) 
            => this.Items = items;

        public IMergeOfficeRequest FilterByExtension()
        {
            var allowedItems = this.Items
                                   .Where(item=>_allowedExtensions.Contains(new FileInfo(item.Key).Extension.ToLowerInvariant())).ToList()
                                   .ToDictionary(item => item.Key, item => item.Value);
 
            var filteredRequest = new MergeOfficeRequest(allowedItems) { Config = this.Config };
            
            foreach (var item in allowedItems)
            {   
                filteredRequest.Items.Add(item.Key, item.Value);
            }

            return filteredRequest;
        }
    }
}