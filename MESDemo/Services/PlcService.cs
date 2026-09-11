using MESDemo.Models;
using RadialGraderMES.Web.Services;

namespace MESDemo.Services
{
    public class PlcService : IPlcService
    {
        private readonly PlcSimulationService _simulationService;

        public PlcService(PlcSimulationService simulationService)
        {
            _simulationService = simulationService;
        }

        public Task<PlcMachineData?> ReadDataAsync(
            CancellationToken cancellationToken = default)
        {
            var data = _simulationService.GenerateData();

            return Task.FromResult<PlcMachineData?>(data);
        }
    }
}