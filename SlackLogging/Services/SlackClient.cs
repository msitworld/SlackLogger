using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using SlackLogging.Models;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace SlackLogging.Services
{
    public class SlackClient : ISlackClient
    {
        #region Constants
        private const string postMesssageUrl = "https://slack.com/api/chat.postMessage";
        #endregion

        #region Fields
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly JsonSerializerSettings _jsonSettings;
        private readonly Configs _config;

        #endregion

        #region Constructor
        public SlackClient(IHttpClientFactory httpClientFactory, Configs config)
        {
            _httpClientFactory = httpClientFactory;
            _jsonSettings = new JsonSerializerSettings() { ContractResolver = new CamelCasePropertyNamesContractResolver() };
            _config = config;
        }

        #endregion

        #region Methods
        public virtual async Task SendMessageAsync(string message, string webhookUrl)
        {
            var client = _httpClientFactory.CreateClient();

            var webhook = new WebhookMessage()
            {
                Text = message
            };

            var json = JsonConvert.SerializeObject(webhook, _jsonSettings);

            var content = new StringContent(json, Encoding.UTF8, "application/json");

            await client.PostAsync(webhookUrl, content);
        }

        public virtual async Task SendMessageAsync(string message)
        {
            if (!string.IsNullOrEmpty(_config.WebhookUrl))
                await SendMessageAsync(message, _config.WebhookUrl);
            else if (!string.IsNullOrEmpty(_config.AuthorizationToken) && !string.IsNullOrEmpty(_config.Channel))
            {
                await SendMessageAsync(message, _config.Channel, _config.AuthorizationToken);
            }
        }

        public virtual async Task SendMessageAsync(string message, string channelId, string authorizationToken)
        {
            var client = _httpClientFactory.CreateClient();

            client.DefaultRequestHeaders.Add("Authorization", $"Bearer {authorizationToken}");

            var webhook = new BotMessage()
            {
                Channel = channelId,
                Text = message
            };

            var json = JsonConvert.SerializeObject(webhook, _jsonSettings);

            var content = new StringContent(json, Encoding.UTF8, "application/json");

            await client.PostAsync(postMesssageUrl, content);
        }

        #endregion
    }
}
