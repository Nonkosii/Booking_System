namespace Booking_System.Services
{
    public interface IResourceService
    {
        Task<bool> CanDeleteResourceAsync(int resourceId);
    }

}
