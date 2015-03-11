namespace XAuth
{
    public class Settings
    {
        public AuthSettings Auth { get; private set; }

        public EnvironmentSettings Environment { get; private set; }

        private Settings(AuthSettings auth, EnvironmentSettings environment)
        {
            Auth = auth;
            Environment = environment;
        }
        public static Settings SharePointPrd
        {
            get { return new Settings(AuthSettings.Prd, EnvironmentSettings.SharePointPrd); }
        }

        public static Settings ExchangePrd
        {
            get { return new Settings(AuthSettings.Prd, EnvironmentSettings.ExchangePrd); }
        }
    }
}
