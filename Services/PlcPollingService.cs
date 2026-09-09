using MESDemo.Hubs;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using RadialGraderMES.Data;
using RadialGraderMES.Models;
using RadialGraderMES.Web.Services;

namespace MESDemo.Services
{
    public class PlcPollingService : BackgroundService
    {
        private readonly IServiceScopeFactory _scopeFactory;
        private readonly IPlcService _plcService;
        private readonly ILogger<PlcPollingService> _logger;
        private readonly IHubContext<MesHub> _hubContext;
        

        public PlcPollingService(
            IServiceScopeFactory scopeFactory,
            IPlcService plcService,
            ILogger<PlcPollingService> logger,
            IHubContext<MesHub> hubContext)
        {
            _scopeFactory = scopeFactory;
            _plcService = plcService;
            _logger = logger;
            _hubContext = hubContext;
        }

        protected override async Task ExecuteAsync(
            CancellationToken stoppingToken)
        {
            _logger.LogInformation(
                "PLC Polling Service Started");

            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {    
                    var plcData = await _plcService
                        .ReadDataAsync(stoppingToken);

                    if (plcData == null)
                    {
                        _logger.LogWarning(
                            "PLC data is null. Could not read data from PLC.");

                        await Task.Delay(
                            TimeSpan.FromSeconds(1),
                            stoppingToken);

                        continue;
                    }

                    _logger.LogInformation(
                        "PLC DATA RECEIVED: Machine={Machine}, Status={Status}, Output={Output}, G1={G1}, G2={G2}, G3={G3}, G4={G4}",
                        plcData.MachineCode,
                        plcData.MachineStatus,
                        plcData.OutputWeight,
                        plcData.Grade1Weight,
                        plcData.Grade2Weight,
                        plcData.Grade3Weight,
                        plcData.Grade4Weight);


                    using var scope =
                        _scopeFactory.CreateScope();

                    var db = scope.ServiceProvider
                        .GetRequiredService<MESDbContext>();      

                    var machine = await db.Machines
                        .FirstOrDefaultAsync(
                            x => x.MachineCode ==
                                 plcData.MachineCode,
                            stoppingToken);

                    if (machine == null)
                    {
                        _logger.LogWarning(
                            "Machine {MachineCode} not found in database.",
                            plcData.MachineCode);

                        await Task.Delay(
                            TimeSpan.FromSeconds(1),
                            stoppingToken);

                        continue;
                    }

                    machine.Status =
                        plcData.MachineStatus;

                    var machineData = new MachineData
                    {
                        MachineId = machine.Id,

                        MachineStatus =
                            plcData.MachineStatus,

                        MachineSpeed =
                            plcData.MachineSpeed,

                        InputWeight =
                            plcData.InputWeight,

                        OutputWeight =
                            plcData.OutputWeight,

                        Grade1Weight =
                            plcData.Grade1Weight,

                        Grade2Weight =
                            plcData.Grade2Weight,

                        Grade3Weight =
                            plcData.Grade3Weight,

                        Grade4Weight =
                            plcData.Grade4Weight,

                        RecordedAt =
                            plcData.RecordedAt
                    };

                    db.MachineData.Add(machineData);

                    var runningBatch =
                        await db.ProductionBatch
                            .Include(x => x.ProductionOrder)
                            .FirstOrDefaultAsync(
                                x =>
                                    x.Status == "Running" &&
                                    x.ProductionOrder != null &&
                                    x.ProductionOrder.MachineId ==
                                        machine.Id,
                                stoppingToken);


                    if (runningBatch == null)
                    {
                        _logger.LogWarning(
                            "NO RUNNING BATCH FOUND for MachineId={MachineId}, MachineCode={MachineCode}",
                            machine.Id,
                            machine.MachineCode);
                    }
                    else
                    {
                        _logger.LogInformation(
                            "RUNNING BATCH FOUND: Batch={BatchNumber}, OrderId={OrderId}",
                            runningBatch.BatchNumber,
                            runningBatch.ProductionOrderId);
                    }

                    if (runningBatch != null &&
                        plcData.MachineStatus.Equals(
                            "Running",
                            StringComparison.OrdinalIgnoreCase))
                    {
                        decimal goodQuantity =
                            plcData.Grade1Weight +
                            plcData.Grade2Weight +
                            plcData.Grade3Weight;

                        decimal rejectQuantity =
                            plcData.Grade4Weight;

                        decimal producedQuantity =
                            plcData.OutputWeight;


                        runningBatch.ProducedQuantity +=
                            producedQuantity;

                        runningBatch.GoodQuantity +=
                            goodQuantity;

                        runningBatch.RejectQuantity +=
                            rejectQuantity;


                        _logger.LogInformation(
                            "BATCH UPDATED: {BatchNumber} | Produced={Produced} KG | Good={Good} KG | Reject={Reject} KG",
                            runningBatch.BatchNumber,
                            runningBatch.ProducedQuantity,
                            runningBatch.GoodQuantity,
                            runningBatch.RejectQuantity);
                    }

                    await db.SaveChangesAsync(
                        stoppingToken);

                    await _hubContext.Clients.All
                        .SendAsync(
                            "MachineDataUpdated",
                            plcData,
                            stoppingToken);


                    _logger.LogInformation(
                        "PLC cycle completed successfully for {MachineCode}",
                        plcData.MachineCode);

                    await Task.Delay(
                        TimeSpan.FromSeconds(1),
                        stoppingToken);
                }
                catch (OperationCanceledException)
                    when (stoppingToken.IsCancellationRequested)
                {
                    break;
                }
                catch (Exception ex)
                {
                    _logger.LogError(
                        ex,
                        "Error While Reading PLC Data");

                    await Task.Delay(
                        TimeSpan.FromSeconds(1),
                        stoppingToken);
                }
            }

            _logger.LogInformation(
                "PLC Polling Service Stopped");
        }
    }
}

