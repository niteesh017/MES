using Microsoft.AspNetCore.Mvc;
using MESDemo.Services;

namespace MESDemo.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PlcController : ControllerBase
    {
        private readonly PlcSimulationService _simulationService;

        public PlcController(PlcSimulationService simulationService)
        {
            _simulationService = simulationService;
        }

        [HttpGet("data")]
        public IActionResult GetData()
        {
            var data = _simulationService.GenerateData();
            return Ok(data);
        }
    }
}