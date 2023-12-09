using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entities
{
    public class Booking
    {
        public int Id { get; set; }

        public bool IsComplete { get; set; } = false;

        public string Status { get; set; }

        public string DoctorId { get; set; }
        [ForeignKey(nameof(DoctorId))]
        public ApplicationUser? Doctor { get; set; }

        public string PatientId { get; set; }
        [ForeignKey(nameof(PatientId))]
        public ApplicationUser? Patient { get; set; }

        public int AppointmentId { get; set; }
        [ForeignKey(nameof(AppointmentId))]
        public Appointment? Appointment { get; set; }

        public int TimeId { get; set; }
        [ForeignKey(nameof(TimeId))]
        public DoctorTime? DoctorTime { get; set; }

        public int DayId { get; set; }
        [ForeignKey(nameof(DayId))]
        public DoctorDay? DoctorDay { get; set; }


    }
}
