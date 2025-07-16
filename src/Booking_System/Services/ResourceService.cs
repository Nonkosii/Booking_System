using Booking_System.Data;
using Microsoft.EntityFrameworkCore;

namespace Booking_System.Services
{
    public class ResourceService : IResourceService
    {
        private readonly ApplicationDbContext _context;

        public ResourceService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<bool> CanDeleteResourceAsync(int resourceId)
        {
            return !await _context.Bookings.AnyAsync(b => b.ResourceId == resourceId);
        }
    }
}
