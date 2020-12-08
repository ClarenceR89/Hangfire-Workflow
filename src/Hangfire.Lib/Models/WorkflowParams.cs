using Newtonsoft.Json.Linq;

namespace Hangfire.Models
{
    public class WorkflowParams
    {
        public string WorkflowId { get; set; }
        public int? Version { get; set; }
        public string Reference { get; set; }
        public JObject Data { get; set; }
    }
}
