namespace Hangfire.Options
{
    public class HangfireOptions
    {
        public const string OptionsName = "HangfireOptions";
        public string Prefix { get; set; }
        public string JobsServer { get; set; }
        public int HeartBeatInterval { get; set; }
    }
}
