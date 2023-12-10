using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entities
{
    public class Appointment
    {
        public int Id { get; set; }

        [Precision(18, 2)]
        public decimal Price { get; set; }

        public string? DoctorId { get; set; }
        [ForeignKey(nameof(DoctorId))]
        public ApplicationUser? Doctor { get; set; }

        public int? DayId { get; set; }
        [ForeignKey(nameof(DayId))]
        public Day? Day { get; set; }   

        public int? TimeId { get; set; }
        [ForeignKey(nameof(TimeId))]
        public Time? Time { get; set; }

    }
}
