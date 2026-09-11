using MESDemo.Models;

namespace RadialGraderMES.Web.Services;

public interface IPlcService
{
    Task<PlcMachineData?> ReadDataAsync(
        CancellationToken cancellationToken = default);
}