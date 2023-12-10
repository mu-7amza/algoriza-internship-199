using Core.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Dtos
{
    public class AppointmentDto
    {
        [Precision(18, 2)]
        [Required]
        public decimal Price { get; set; }

        public DayOfWeek Day { get; set; }

        public string Time {  get; set; }


    }
}
