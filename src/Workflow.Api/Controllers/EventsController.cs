using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Workflow.Api.Workflows;
using WorkflowCore.Interface;

namespace Workflow.Api.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    public class EventsController : Controller
    {
        private readonly IWorkflowController _workflowService;

        public EventsController(IWorkflowController workflowService)
        {
            _workflowService = workflowService;
        }

        [HttpPost("{eventName}/{eventKey}")]
        public async Task<IActionResult> Post(string eventName, string eventKey, [FromBody] MyDataClass eventData)
        {
            await _workflowService.PublishEvent(eventName, eventKey, eventData.Value1);
            return Ok();
        }

    }
}
