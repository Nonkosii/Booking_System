using Booking_System.Models;

namespace Booking_System.Services
{
    public interface IBookingService
    {
        bool IsBookingConflicting(Booking newBooking, IEnumerable<Booking> existingBookings);
    }
}
