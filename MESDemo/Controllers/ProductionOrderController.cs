using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Razor.TagHelpers;
using Microsoft.EntityFrameworkCore;
using RadialGraderMES.Data;
using RadialGraderMES.Models;

namespace MESDemo.Controllers
{
    public class ProductionOrderController : Controller
    {
        private readonly MESDbContext _context;
        public ProductionOrderController(MESDbContext context)
        { 
            _context = context;
        }
        public async Task<IActionResult> Index()
        {
            var orders = await _context.ProductionOrders
                .Include(x => x.Machine)
                .OrderByDescending(x => x.Id)
                .ToListAsync();

            return View(orders);            
        }

        public async Task<IActionResult> Create()
        {
            await LoadMachines();
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ProductionOrder order)
        {
            if (!ModelState.IsValid)
            {
                await LoadMachines();
                return View(order);
            }

            order.Status = "Pending";
            _context.ProductionOrders.Add(order);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        private async Task LoadMachines()
        {
            var machines = await _context.Machines
                .Where(x => x.IsActive)
                .OrderBy(x => x.MachineCode)
                .ToListAsync();

            ViewBag.Machines = new SelectList(
                machines,
                "Id",
                "MachineName"
            );
        }
    }
}
