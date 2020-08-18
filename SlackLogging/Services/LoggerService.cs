using Prometheus;
using SlackLogging.Models;
using System.Threading.Tasks;

namespace SlackLogging.Services
{
    public class LoggerService : ILoggerService
    {
        private readonly ISlackClient _client;
        private static readonly Counter ProcessedJobCount = Metrics
            .CreateCounter("slacklogger_event_logs_total", "Number of log released",
                new CounterConfiguration(){LabelNames = new []{"type"}});

        private static readonly Histogram LogDuration = Metrics
            .CreateHistogram("slacklogger_sending_logs_duration_seconds", "Histogram of log call processing durations.");

        private static readonly Gauge LogSendingInProgress = Metrics
            .CreateGauge("slacklogger_sending_logs_in_progress", "Number of sending log operations ongoing.");

        public LoggerService(ISlackClient client)
        {
            _client = client;
        }

        public async Task Log(LogType logType, string message)
        {
            await _client.SendMessageAsync(message);
        }

        public async Task Log(LogType logType, string message, string webhookUrl)
        {
            var logLevel = new SlackLogLevel{Type = logType};

            ProcessedJobCount.WithLabels(logType.ToString().ToLower()).Inc();

            using (LogSendingInProgress.TrackInProgress())
            {
                using (LogDuration.NewTimer())
                {
                    await _client.SendMessageAsync($"{logLevel.Icon}[{logType.ToString()}]: {message}", webhookUrl);
                }
            }

            ProcessedJobCount.Publish();
            LogSendingInProgress.Publish();
            LogDuration.Publish();
        }

        public async Task Log(LogType logType, string message, string channel, string authorizationToken)
        {
            var logLevel = new SlackLogLevel { Type = logType };

            await _client
                .SendMessageAsync($"{logLevel.Icon}[{logType.ToString()}]: {message}", channel, authorizationToken);
        }
    }
}
