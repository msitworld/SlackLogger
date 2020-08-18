using SlackLogging.Models;
using System.Threading.Tasks;

namespace SlackLogging.Services
{
    public interface ILoggerService
    {
        Task Log(LogType logType, string message);

        Task Log(LogType logType, string message, string webhookUrl);

        Task Log(LogType logType, string message, string channel, string authorizationToken);
    }
}
