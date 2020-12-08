using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WorkflowCore.Interface;
using WorkflowCore.Models;
using Workflow.Steps;
using Workflow.Constants;

namespace Workflow.Workflows
{
    public class HelloWorld : IWorkflow
    {
        public string Id => WorkflowNames.HelloWorld;
        public int Version => 1;

        public void Build(IWorkflowBuilder<object> builder)
        {
            builder
                .StartWith<StartWorkflowStep>()
                .Then<ConsoleLogStep>()
                    .Input(step => step.Message, context => "Hello World.")
                .Then<EndWorkflowStep>();
        }
    }
}

