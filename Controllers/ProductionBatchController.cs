using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RadialGraderMES.Data;
using RadialGraderMES.Models;

namespace MESDemo.Controllers
{
    public class ProductionBatchController : Controller
    {
        private readonly MESDbContext _context;

        public ProductionBatchController(MESDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
           var batches = await _context.ProductionBatch
                .Include(x => x.ProductionOrder)
                .OrderByDescending(x => x.Id)
                .ToListAsync();

            return View(batches);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Start(int orderId)
        {
            var order = await _context.ProductionOrders
                .FirstOrDefaultAsync(x => x.Id == orderId);

            if (order == null)
            {
                return NotFound();
            }

            if(order.Status == "Running")
            {
                TempData["Error"] = "Product Order is already running";
                return RedirectToAction("Index", "ProductionOrder");
            }

            var batchNumber = $"Batch-{DateTime.Now:yyyyMMdd-HHmmss}";

            var batch = new ProductionBatch
            {
                BatchNumber = batchNumber,
                ProductionOrderId = order.Id,
                StartTime = DateTime.UtcNow,
                Status = "Running",
                ProducedQuantity = 0,
                GoodQuantity = 0,
                RejectQuantity = 0
            };

            order.Status = "Running";
            order.StartTime = DateTime.UtcNow;

            _context.ProductionBatch.Add(batch);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> stop(int id)
        {
            var batch = await _context.ProductionBatch
                .Include(x => x.ProductionOrder)
                .FirstOrDefaultAsync(x => x.Id == id);

            if(batch == null)
            {
                return NotFound();
            }

            batch.Status = "Completed";
            batch.EndTime = DateTime.UtcNow;

            if(batch.ProductionOrder != null)
            {
                batch.ProductionOrder.Status = "Completed";
                batch.ProductionOrder.EndTime = DateTime.UtcNow;
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
    }
}
