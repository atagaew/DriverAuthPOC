namespace POC.DriverInstanceWebSocketApp
{
    public class ConfigurationSettings
    {
        public static string SectionName = "ConfigurationSettings";
        public string OAuthUrl { get; set; }
        public DriverServiceSettings DriverService { get; set; } = new DriverServiceSettings();
    }

    public class DriverServiceSettings
    {
        public string BaseUrl { get; set; }
        public string CallbackUrlEndpoint { get; set; }
        public string TokenUrlEndpoint { get; set; }
    }
}
