using Booking_System.Data;
using Booking_System.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;

namespace Booking_System.Controllers
{
    public class HomeController(ApplicationDbContext context) : Controller
    {
        private readonly ApplicationDbContext _context = context;

        public IActionResult Index()
        {
            return View();
        }

        [Authorize]
        public async Task<IActionResult> Dashboard(int? resourceId, DateTime? date, string? search)
        {
            var bookingsQuery = _context.Bookings
                .Include(b => b.Resource)
                .Where(b => b.StartTime >= DateTime.Today)
                .AsQueryable();

            if (resourceId.HasValue)
                bookingsQuery = bookingsQuery.Where(b => b.ResourceId == resourceId.Value);

            if (date.HasValue)
                bookingsQuery = bookingsQuery.Where(b => b.StartTime.Date == date.Value.Date);

            if (!string.IsNullOrEmpty(search))
                bookingsQuery = bookingsQuery.Where(b =>
                    b.BookedBy.Contains(search) || b.Purpose!.Contains(search));

            var bookings = await bookingsQuery
                .OrderBy(b => b.StartTime)
                .ToListAsync();

            ViewData["Resources"] = new SelectList(await _context.Resources.ToListAsync(), "Id", "Name");

            return View(bookings);
        }




        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
