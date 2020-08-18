using System.Threading.Tasks;

namespace SlackLogging.Services
{
    public interface ISlackClient
    {
        Task SendMessageAsync(string message);
        Task SendMessageAsync(string message, string webhookUrl);
        Task SendMessageAsync(string message, string channel, string authorizationToken);
    }
}
