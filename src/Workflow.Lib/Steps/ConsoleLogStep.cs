using System;
using Microsoft.Extensions.Logging;
using WorkflowCore.Interface;
using WorkflowCore.Models;

namespace Workflow.Steps
{
    public class ConsoleLogStep : StepBody
    {
        private readonly ILogger<ConsoleLogStep> _logger;

        public ConsoleLogStep(ILogger<ConsoleLogStep> logger)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }
        public string Message { get; set; }

        public override ExecutionResult Run(IStepExecutionContext context)
        {
            _logger.LogInformation(Message);

            return ExecutionResult.Next();
        }
    }
}
