using System.Security.AccessControl;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using WorkflowCore.Models;
using WorkflowCore.Models.Search;

namespace Workflow.Client
{
    public interface IWorkflowClient
    {
        Task<WorkflowInstance> GetWorkflowById(string id);
        Task<Page<WorkflowSearchResult>> GetWorkflows(string terms, WorkflowStatus? status, string type, DateTime? createdFrom, DateTime? createdTo, int skip, int take = 10);
        Task<bool> Resume(string id);
        Task SendWorkflowEvent(string eventName, string eventKey, JObject eventData);
        Task<string> Start(string id, int? version, string reference, JObject data);
        Task<bool> Suspend(string id);
        Task<bool> Terminate(string id);
        List<string> GetWorkflowNames();
        List<string> GetEventNames();
    }
}
