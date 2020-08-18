namespace SlackLogging.Models
{
    public class SlackLogLevel
    {
        public LogType Type { get; set; }

        public string Icon
        {
            get
            {
                switch (Type)
                {
                    case LogType.Information:
                        return ":information_source:";

                    case LogType.Warning:
                        return ":warning:";

                    case LogType.Error:
                        return ":exclamation:";

                    default:
                        return ":grey_exclamation:";
                }
            }
        }
    }
}
