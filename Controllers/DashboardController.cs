using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RadialGraderMES.Data;

namespace MESDemo.Controllers
{
    public class DashboardController : Controller
    {
        private readonly MESDbContext _context;

        public DashboardController(MESDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> GetCurrentProduction()
        {
            var batch = await _context.ProductionBatch
                .Include(x => x.ProductionOrder)
                .Where(x =>
                    x.Status == "Running" &&
                    x.ProductionOrder != null)
                .OrderByDescending(x => x.Id)
                .FirstOrDefaultAsync();

            if (batch == null)
            {
                return Ok(null);
            }

            var order = batch.ProductionOrder!;

            decimal progress = 0;

            if (order.TargetQuantity > 0)
            {
                progress =
                    (batch.ProducedQuantity /
                     order.TargetQuantity) * 100;
            }

            if (progress > 100)
            {
                progress = 100;
            }

            return Ok(new
            {
                orderNumber = order.OrderNumber,
                productName = order.ProductName,
                targetQuantity = order.TargetQuantity,
                unit = order.Unit,

                batchNumber = batch.BatchNumber,
                batchStatus = batch.Status,

                producedQuantity = batch.ProducedQuantity,
                goodQuantity = batch.GoodQuantity,
                rejectQuantity = batch.RejectQuantity,

                progress = Math.Round(progress, 2)
            });
        }
    }
}

