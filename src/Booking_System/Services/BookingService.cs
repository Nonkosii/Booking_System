using Booking_System.Models;

namespace Booking_System.Services
{
    public class BookingService : IBookingService
    {
        public bool IsBookingConflicting(Booking newBooking, IEnumerable<Booking> existingBookings)
        {
            return existingBookings.Any(b =>
                b.ResourceId == newBooking.ResourceId &&
                b.Id != newBooking.Id &&
                b.StartTime < newBooking.EndTime &&
                b.EndTime > newBooking.StartTime
            );
        }
    }
}
