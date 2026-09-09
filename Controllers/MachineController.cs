using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RadialGraderMES.Data;
using RadialGraderMES.Models;

namespace MESDemo.Controllers
{
    public class MachineController : Controller
    {
        private readonly MESDbContext _context;

        public MachineController(MESDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var machines = await _context.Machines
                .OrderBy(x => x.MachineCode)
                .ToListAsync();

            return View(machines);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Machine machine)
        {
            if (string.IsNullOrWhiteSpace(machine.MachineCode))
            {
                ModelState.AddModelError(
                    "MachineCode",
                    "Machine Code is required.");
            }

            if (string.IsNullOrWhiteSpace(machine.MachineName))
            {
                ModelState.AddModelError(
                    "MachineName",
                    "Machine Name is required.");
            }

            if (!ModelState.IsValid)
                return View(machine);

            var exists = await _context.Machines
                .AnyAsync(x => x.MachineCode == machine.MachineCode);

            if (exists)
            {
                ModelState.AddModelError(
                    "MachineCode",
                    $"Machine Code '{machine.MachineCode}' already exists.");

                return View(machine);
            }

            machine.Status = "Stopped";
            machine.IsActive = true;
            machine.CreatedAt = DateTime.UtcNow;

            try
            {
                _context.Machines.Add(machine);
                await _context.SaveChangesAsync();

                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                Console.WriteLine("========== MACHINE SAVE ERROR ==========");
                Console.WriteLine(ex.ToString());
                Console.WriteLine("========================================");

                ModelState.AddModelError(
                    "",
                    "Machine save failed: " + ex.Message);

                return View(machine);
            }
        }
    }
}