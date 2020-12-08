using System;
using Microsoft.Extensions.Logging;
using WorkflowCore.Interface;
using WorkflowCore.Models;

namespace Workflow.Steps
{
    public class EndWorkflowStep : StepBody
    {
        private readonly ILogger<EndWorkflowStep> _logger;

        public EndWorkflowStep(ILogger<EndWorkflowStep> logger)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public override ExecutionResult Run(IStepExecutionContext context)
        {
            _logger.LogInformation("Ending Workflow...");

            return ExecutionResult.Next();
        }
    }
}

