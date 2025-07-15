using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace Booking_System.Models
{
    public class Booking
    {
        public class DateGreaterThanAttribute(string comparison) : ValidationAttribute
        {
            private readonly string _comparison = comparison;

            protected override ValidationResult? IsValid(object? value, ValidationContext ctx)
            {
                var comparisonProp = ctx.ObjectType.GetProperty(_comparison);
                if (comparisonProp == null) return ValidationResult.Success;

                var comparisonValue = comparisonProp.GetValue(ctx.ObjectInstance);
                if (value is DateTime end && comparisonValue is DateTime start && end <= start)
                    return new ValidationResult(ErrorMessage);
                return ValidationResult.Success;
            }
        }

        public int Id { get; set; }
        [Required(ErrorMessage = "Please select a resource.")]
        [Display(Name = "Resource")]
        public int ResourceId { get; set; }

        public Resource? Resource { get; set; }

        [Required]
        public DateTime StartTime { get; set; }

        [Required]
        [DateGreaterThan(nameof(StartTime),
            ErrorMessage = "End time must be after start time")]
        public DateTime EndTime { get; set; }

        [Required, StringLength(100)]
        public string BookedBy { get; set; } = string.Empty;

        [StringLength(300)]
        public string? Purpose { get; set; }
    }
}
