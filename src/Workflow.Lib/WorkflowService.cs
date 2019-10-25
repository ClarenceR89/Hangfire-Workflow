/**
 *  Uses WorkflowCore DefinitionStorage and Json definitions to store flexible workflows
 *  https://workflow-core.readthedocs.io/en/latest/json-yaml/
 */

namespace Workflow.Lib
{
    public class WorkflowService : IWorkflowService
    {
     
        public IWorkflowService AddWorkflowJson(string definition) {
            return this;
        }
    }
}
