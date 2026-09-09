using MESDemo.Models;
using RadialGraderMES.Models;

namespace RadialGraderMES.Web.Services;

public interface IPlcService
{
    Task<PlcDataDto?> ReadDataAsync(
        CancellationToken cancellationToken = default);
}