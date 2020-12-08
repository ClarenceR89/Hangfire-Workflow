using WorkflowCore;
using WorkflowCore.Services.DefinitionStorage;

namespace Workflow.Client
{
    public interface IWorkflowDefinitionClient
    {
        IWorkflowDefinitionClient AddWorkflowJson(string definition);
    }
}
