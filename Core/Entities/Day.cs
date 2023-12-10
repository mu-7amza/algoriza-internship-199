using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entities
{
    public class Day
    {
        public int Id { get; set; }

        public DayOfWeek Date { get; set; }

        public string? DoctorId { get; set; }

        [ForeignKey(nameof(DoctorId))]
        public ApplicationUser? Doctor { get; set; }

        public ICollection<Time>? Times { get; set; }
    }
}
