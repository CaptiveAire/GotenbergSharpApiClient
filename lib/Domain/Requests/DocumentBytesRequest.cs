using System.Net.Http;
using JetBrains.Annotations;

namespace Gotenberg.Sharp.API.Client.Domain.Requests
{
    [UsedImplicitly]
    public sealed class DocumentBytesRequest : DocumentRequest<byte[]>
    {
        public DocumentBytesRequest(byte[] bodyHtml) : base(value => new ByteArrayContent(value), bodyHtml)
        {
        }
    }
}