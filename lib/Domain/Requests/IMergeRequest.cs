namespace Gotenberg.Sharp.API.Client.Domain.Requests
{
    public interface IMergeRequest: IConvertToHttpContent
    {
        int Count { get; }
    }
   
}