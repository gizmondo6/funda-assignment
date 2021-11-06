using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using FundaAssignment.Web.Models;
using FundaAssignment.Web.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace FundaAssignment.Web.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class AgentsController : ControllerBase
    {
        private readonly IAgentService _agentService;
        private readonly ILogger<AgentsController> _logger;

        public AgentsController(IAgentService agentService, ILogger<AgentsController> logger)
        {
            _agentService = agentService;
            _logger = logger;
        }

        [HttpGet("top-sellers")]
        [ResponseCache(Duration = 60)]
        public async Task<ActionResult<IEnumerable<TopSellingAgent>>> GetTopSellers([Required] string searchQuery, int take = 10)
        {
            try
            {
                return Ok(await _agentService.GetTopSellers(searchQuery, take, HttpContext.RequestAborted));
            }
            catch (OperationCanceledException e)
            {
                _logger.LogDebug(e, "Getting top agents has been aborted");
                return Ok(Enumerable.Empty<TopSellingAgent>());
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Failed to get top agents");
                return Problem(e.Message);
            }
        }
    }
}