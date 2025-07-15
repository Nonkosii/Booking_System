using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace Booking_System.Models
{
    public class Resource
    {
        public int Id { get; set; }

        [Required, StringLength(100)]
        public string Name { get; set; } = string.Empty;

        [Required, StringLength(500)]
        public string? Description { get; set; }

        [Required, StringLength(200)]
        public string? Location { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = "Capacity must be positive")]
        public int Capacity { get; set; }

        public bool IsAvailable { get; set; } = true;

        // Navigation property (one-to-many)
        public ICollection<Booking>? Bookings { get; set; }
    }
}
