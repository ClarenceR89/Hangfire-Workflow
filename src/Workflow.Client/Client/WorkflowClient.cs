using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using WorkflowCore.Interface;
using WorkflowCore.Models;
using WorkflowCore.Models.Search;
using Workflow.Constants;


namespace Workflow.Client
{
    public class WorkflowClient : IWorkflowClient
    {
        private readonly IWorkflowController _workflowService;
        private readonly IWorkflowRegistry _registry;
        private readonly IPersistenceProvider _workflowStore;
        private readonly ISearchIndex _searchService;

        public WorkflowClient(IWorkflowController workflowService, ISearchIndex searchService, IWorkflowRegistry registry, IPersistenceProvider workflowStore)
        {
            _workflowService = workflowService;
            _workflowStore = workflowStore;
            _registry = registry;
            _searchService = searchService;
        }

        public Task<Page<WorkflowSearchResult>> GetWorkflows(string terms, WorkflowStatus? status, string type, DateTime? createdFrom, DateTime? createdTo, int skip, int take = 10)
        {
            try
            {
                var filters = new List<SearchFilter>();

                if (status.HasValue)
                    filters.Add(StatusFilter.Equals(status.Value));

                if (createdFrom.HasValue)
                    filters.Add(DateRangeFilter.After(x => x.CreateTime, createdFrom.Value));

                if (createdTo.HasValue)
                    filters.Add(DateRangeFilter.Before(x => x.CreateTime, createdTo.Value));

                if (!string.IsNullOrEmpty(type))
                    filters.Add(ScalarFilter.Equals(x => x.WorkflowDefinitionId, type));

                return _searchService.Search(terms, skip, take, filters.ToArray());
            }
            catch
            {
                throw;
            }
        }

        public Task<WorkflowInstance> GetWorkflowById(string id)
        {
            try
            {
                return _workflowStore.GetWorkflowInstance(id);
            }
            catch
            {
                throw;
            }
        }

        public async Task<string> Start(string id, int? version, string reference, JObject data)
        {
            try
            {
                string workflowId = null;
                var def = _registry.GetDefinition(id, version);
                if (def == null)
                    throw new Exception(String.Format("Workflow defintion {0} for version {1} not found", id, version));

                if ((data != null) && (def.DataType != null))
                {
                    var dataStr = JsonConvert.SerializeObject(data);
                    var dataObj = JsonConvert.DeserializeObject(dataStr, def.DataType);
                    workflowId = await _workflowService.StartWorkflow(id, version, dataObj, reference);
                }
                else
                {
                    workflowId = await _workflowService.StartWorkflow(id, version, null, reference);
                }

                return workflowId;
            }
            catch
            {
                throw;
            }
        }

        public Task<bool> Suspend(string id)
        {
            try
            {
                return _workflowService.SuspendWorkflow(id);
            }
            catch
            {
                throw;
            }
        }

        public Task<bool> Resume(string id)
        {
            try
            {
                return _workflowService.ResumeWorkflow(id);
            }
            catch
            {
                throw;
            }
        }

        public Task<bool> Terminate(string id)
        {
            try
            {
                return _workflowService.TerminateWorkflow(id);
            }
            catch
            {
                throw;
            }
        }

        public Task SendWorkflowEvent(string eventName, string eventKey, JObject eventData)
        {
            try
            {
                return _workflowService.PublishEvent(eventName, eventKey, eventData);
            }
            catch
            {
                throw;
            }
        }

        public List<string> GetWorkflowNames()
        {
            return GetConstantsNamesByReflection<WorkflowNames>();
        }

        public List<string> GetEventNames()
        {
            return GetConstantsNamesByReflection<EventNames>();
        }

        private static List<string> GetConstantsNamesByReflection<T>()
        {
            return typeof(T).GetFields(
                            BindingFlags.Public |
                            BindingFlags.Static |
                            BindingFlags.FlattenHierarchy
                        )
                        .Where(fi => fi.IsLiteral && !fi.IsInitOnly)
                        .Select(x => x.GetRawConstantValue())
                        .OfType<string>()
                        .ToList();
        }
    }
}
