namespace Selenium_CSharp_Practice.Config
{
    public class AppConfig
    {
        //default values
        public string BaseUrl { get; set; } = string.Empty;
        public string Browser { get; set; } = "chrome";
        public List<string> Options { get; set; } = [];
        public bool Headless { get; set; } = false;
        public int ImplicitWaitSeconds { get; set; } = 5;
        public int ExplicitWaitSeconds { get; set; } = 10;
    }
}
