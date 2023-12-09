using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entities
{
    public class DoctorDay
    {
        public int Id { get; set; }

        public string DoctorId { get; set; }

        [ForeignKey(nameof(DoctorId))]
        public ApplicationUser Doctor { get; set; }

        public string Day { get; set; }

        public List<DoctorTime>? Times { get; set; }
    }
}
