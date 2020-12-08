namespace Workflow
{
    public class WorkflowOptions
    {
        public const string OptionsName = "WorkflowOptions";
        public string Prefix { get; set; } = "";
        public string ElasticUrl { get; set; } = "";
        public bool DisableDirectStreaming { get; set; } = false;
    }
}
