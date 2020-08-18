namespace SlackLogging
{
    public sealed class ConfigBuilder
    {
        private readonly Configs _config;

        public ConfigBuilder(Configs config)
        {
            _config = config;
        }

        public ConfigBuilder WithWebhook(string webhookUrl)
        {
            _config.WebhookUrl = webhookUrl;
            return this;
        }

        public ConfigBuilder WithBot(string authorizationToken, string channel)
        {
            _config.AuthorizationToken = authorizationToken;
            _config.Channel = channel;
            return this;
        }
    }

}
