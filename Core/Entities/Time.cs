using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entities
{
    public class Time
    {
        public int Id { get; set; }

        [Required]
        public string TimeValue { get; set; }

        public int? DayId { get; set; }

        public bool IsBooked { get; set; } = false;

        [ForeignKey(nameof(DayId))]
        public Day? Day { get; set; }
    }
}
