using Booking_System.Data;
using Booking_System.Models;
using Booking_System.Services;
using Humanizer.Localisation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

[Authorize]
public class BookingsController(ApplicationDbContext context, IBookingService bookingService) : Controller
{
    private readonly ApplicationDbContext _context = context;

    private readonly IBookingService _bookingService = bookingService;

    // GET: Bookings
    public async Task<IActionResult> Index(DateTime? dateFilter)
    {
        var bookings = _context.Bookings
                               .Include(b => b.Resource)
                               .AsQueryable();


        if (dateFilter.HasValue)
        {
            bookings = bookings.Where(b => b.StartTime.Date == dateFilter.Value.Date);
        }

        return View(await bookings
            .OrderBy(b => b.StartTime)
            .ToListAsync());
    }

    // GET: Bookings/Create
    public IActionResult Create()
    {
        var availableResources = _context.Resources
            .Where(r => r.IsAvailable)
            .ToList();

        var resourceList = new[] { new { Id = (int?)null, Name = "-- Select --" } }
            .Concat(availableResources.Select(r => new { Id = (int?)r.Id, r.Name }));

        ViewData["ResourceId"] = new SelectList(resourceList, "Id", "Name");

        return View();
    }


    // POST: Bookings/Create
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create([Bind("Id,ResourceId,StartTime,EndTime,BookedBy,Purpose")] Booking booking)
    {
        if (booking.EndTime <= booking.StartTime)
        {
            ModelState.AddModelError("", "End time must be after start time.");
        }

        // Conflict check
        var existingBookings = await _context.Bookings
                .Where(b => b.ResourceId == booking.ResourceId)
                .ToListAsync();

        if (_bookingService.IsBookingConflicting(booking, existingBookings))
        {
            ModelState.AddModelError("", "This resource is already booked during the requested time.");
        }

        if (ModelState.IsValid)
        {
            _context.Add(booking);
            await _context.SaveChangesAsync();
            TempData["Success"] = "Booking added successfully.";
            return RedirectToAction(nameof(Index));
        }

        ViewData["ResourceId"] = new SelectList(_context.Resources.Where(r => r.IsAvailable), "Id", "Name", booking.ResourceId);
        return View(booking);
    }
    // GET: Bookings/Edit/5
    public async Task<IActionResult> Edit(int? id)
    {
        if (id == null) return NotFound();

        var booking = await _context.Bookings.FindAsync(id);
        if (booking == null) return NotFound();

        ViewData["ResourceId"] = new SelectList(_context.Resources.Where(r => r.IsAvailable), "Id", "Name", booking.ResourceId);
        return View(booking);
    }

    // POST: Bookings/Edit/5
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, Booking booking)
    {
        if (id != booking.Id) return NotFound();

        if (booking.EndTime <= booking.StartTime)
            ModelState.AddModelError("", "End time must be after start time.");

        var existingBookings = await _context.Bookings
                .Where(b => b.ResourceId == booking.ResourceId)
                .ToListAsync();

        if (_bookingService.IsBookingConflicting(booking, existingBookings))
        {
            ModelState.AddModelError("", "This resource is already booked during the requested time.");
        }

        if (ModelState.IsValid)
        {
            try
            {
                _context.Update(booking);
                await _context.SaveChangesAsync();
                TempData["Success"] = "Booking updated successfully.";
                return RedirectToAction(nameof(Index));
            }
            catch (DbUpdateException)
            {
                ModelState.AddModelError("", "Error updating the booking.");
            }
        }

        ViewData["ResourceId"] = new SelectList(_context.Resources, "Id", "Name", booking.ResourceId);
        return View(booking);
    }

    // GET: Bookings/Details/5
    public async Task<IActionResult> Details(int? id)
    {
        if (id == null) return NotFound();

        var booking = await _context.Bookings
                                    .Include(b => b.Resource)
                                    .FirstOrDefaultAsync(m => m.Id == id);

        return booking == null ? NotFound() : View(booking);
    }
    // GET: Bookings/Delete/5  → confirmation page
    public async Task<IActionResult> Delete(int? id)
    {
        if (id == null) return NotFound();

        var booking = await _context.Bookings
                                    .Include(b => b.Resource)
                                    .FirstOrDefaultAsync(m => m.Id == id);

        return booking == null ? NotFound() : View(booking);
    }

    // POST: Bookings/Delete/5  → actually removes row
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        var booking = await _context.Bookings.FindAsync(id);
        if (booking != null)
        {
            _context.Bookings.Remove(booking);
            await _context.SaveChangesAsync();
            TempData["Success"] = "Booking deleted successfully.";
        }
        return RedirectToAction(nameof(Index));
    }


}
