using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using WorkflowCore.Interface;
using WorkflowCore.Models;

namespace Workflow.Steps
{
    public class StartWorkflowStep : StepBody
    {
        private readonly ILogger<StartWorkflowStep> _logger;

        public StartWorkflowStep(ILogger<StartWorkflowStep> logger)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public override ExecutionResult Run(IStepExecutionContext context)
        {
            _logger.LogInformation("Starting Workflow...");

            return ExecutionResult.Next();
        }
    }
}

