using Microsoft.AspNetCore.Mvc;
using PLC_Simulator.Services;

namespace PLC_Simulator.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PlcController : Controller
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
