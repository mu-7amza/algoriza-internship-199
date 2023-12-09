using Core.Enums;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entities
{
    public class Discount
    {
        public int Id { get; set; }
        [Required]
        public string Name { get; set; } // A name or description for the discount
        [Required]
        [Precision(18, 2)]
        public decimal Value { get; set; }
        [Required]
        public string DiscountType { get; set; }
    }
}
