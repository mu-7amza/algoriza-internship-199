using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entities
{
    public class DoctorTime
    {
        public int Id { get; set; }

        public string Time { get; set; }

        public int DoctorDayId { get; set; }

        [ForeignKey(nameof(DoctorDayId))]
        public DoctorDay DoctorDay { get; set; }
    }
}
