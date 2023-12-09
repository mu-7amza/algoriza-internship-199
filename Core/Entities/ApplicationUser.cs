﻿using Microsoft.AspNetCore.Identity;
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
        [MaxLength(20)]
        public string FirstName { get; set; }

        [Required]
        [MaxLength(20)]
        public string LastName { get; set; }

        [Required]
        public string Gender { get; set; }

        [Required]
        public string DateOfBirth { get; set; }

        public string? ImageUrl { get; set; }

        [NotMapped]
        public ICollection<Appointment>? Appointments { get; set; }

        public int? SpecializationId { get; set; }
        [ForeignKey(nameof(SpecializationId))]
        public Specialization? Specialization { get; set; }

        [NotMapped]
        public ICollection<Discount>? Discounts { get; set; }


    }
}
