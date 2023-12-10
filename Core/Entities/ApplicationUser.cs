using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entities
{
    public class ApplicationUser : IdentityUser
    {
        [Required]
        [MaxLength(50)]
        public string FirstName { get; set; }

        [Required]
        [MaxLength(50)]
        public string LastName { get; set; }

        [Required]
        public string Gender { get; set; }

        [Required]
        public string DateOfBirth { get; set; }

        public string? ImageUrl { get; set; }

        public int? AppointmentId {  get; set; }
        [ForeignKey(nameof(AppointmentId))]
        public Appointment? Appointment { get; set; }

        public int? SpecializId { get; set; }
        [ForeignKey(nameof(SpecializId))]
        public Specialization? Specialization { get; set; }

        public int? DiscountId { get; set; }
        [ForeignKey(nameof(DiscountId))]
        public Discount? Discount { get; set; }

        public int? BookingId { get; set; }
        [ForeignKey(nameof(BookingId))]
        public Booking? Booking { get; set; }


    }
}
