using System;

using Gotenberg.Sharp.API.Client.Domain.ContentTypes;
using Gotenberg.Sharp.API.Client.Extensions;

using HeyRed.Mime;

namespace Gotenberg.Sharp.API.Client.Infrastructure.ContentTypes
{
    public class ResolveContentTypeImplementation : IResolveContentType
    {
   
        public string GetContentType(string fileName)
        {
            if (fileName.IsNotSet()) throw new ArgumentException("file name is either null or empty");
            return MimeTypesMap.GetMimeType(fileName);
        }
    }
}