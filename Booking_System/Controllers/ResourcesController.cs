using Booking_System.Data;
using Booking_System.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

[Authorize]
public class ResourcesController(ApplicationDbContext context) : Controller
{
    private readonly ApplicationDbContext _context = context;

    // GET: Resources/Index
    public async Task<IActionResult> Index(string? search)
    {
        var resources = _context.Resources.AsQueryable();

        if (!string.IsNullOrEmpty(search))
        {
            resources = resources.Where(r => r.Name.Contains(search));
        }

        return View(await resources.ToListAsync());
    }

    // GET: Resources/Details/5
    public async Task<IActionResult> Details(int? id)
    {
        if (id == null) return NotFound();

        var resource = await _context.Resources
            .Include(r => r.Bookings!.Where(b => b.StartTime >= DateTime.Now))
            .FirstOrDefaultAsync(r => r.Id == id);

        if (resource == null) return NotFound();

        return View(resource);
    }


    // GET: Resources/Create
    public IActionResult Create()
    {
        return View();
    }

    // POST: Resources/Create
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(Resource resource)
    {
        try
        {
            if (ModelState.IsValid)
            {
                _context.Add(resource);
                await _context.SaveChangesAsync();
                TempData["Success"] = "Resource added successfully.";
                return RedirectToAction(nameof(Index));
            }
        }
        catch
        {
            ModelState.AddModelError("", "An error occurred while saving the resource.");
        }

        return View(resource);
    }

    // GET: Resources/Edit/5
    public async Task<IActionResult> Edit(int? id)
    {
        if (id == null) return NotFound();

        var resource = await _context.Resources.FindAsync(id);
        if (resource == null) return NotFound();

        return View(resource);
    }

    // POST: Resources/Edit/5
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, Resource resource)
    {
        if (id != resource.Id) return NotFound();

        if (!ModelState.IsValid) return View(resource);

        try
        {
            _context.Update(resource);
            await _context.SaveChangesAsync();
            TempData["Success"] = "Resource updated successfully.";
            return RedirectToAction(nameof(Index));
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!ResourceExists(resource.Id))
                return NotFound();

            throw;
        }
    }

    // GET: Resources/Delete/5
    public async Task<IActionResult> Delete(int? id)
    {
        if (id == null) return NotFound();

        var resource = await _context.Resources
            .FirstOrDefaultAsync(r => r.Id == id);

        if (resource == null) return NotFound();

        return View(resource);
    }

    // POST: Resources/Delete/5
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        var resource = await _context.Resources.FindAsync(id);

        if (resource != null)
        {
            _context.Resources.Remove(resource);
            await _context.SaveChangesAsync();
            TempData["Success"] = "Resource deleted successfully.";
        }

        return RedirectToAction(nameof(Index));
    }

    private bool ResourceExists(int id)
    {
        return _context.Resources.Any(e => e.Id == id);
    }
}
