/**
 *  Uses WorkflowCore DefinitionStorage and Json definitions to store flexible workflows
 *  https://workflow-core.readthedocs.io/en/latest/json-yaml/
 */
using Newtonsoft.Json;
using WorkflowCore;
using WorkflowCore.Services;
using WorkflowCore.Services.DefinitionStorage;
using WorkflowCore.Interface;

namespace Workflow.Client
{
    public class WorkflowDefinitionClient : IWorkflowDefinitionClient
    {
        private readonly IDefinitionLoader _definitionLoader;
        public WorkflowDefinitionClient(IDefinitionLoader definitionLoader) {
            _definitionLoader = definitionLoader;
        }

        public IWorkflowDefinitionClient AddWorkflowJson(string definition) {
            _definitionLoader.LoadDefinition(definition, Deserializers.Json);

            return this;
        }
    }
}
