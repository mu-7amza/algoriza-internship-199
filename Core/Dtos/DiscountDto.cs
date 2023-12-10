using Core.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Core.Enums;

namespace Core.Dtos
{
    public class DiscountDto
    {
        [Required]
        public string Code { get; set; }

        public int? DiscountId { get; set; }

        public DiscountType? DiscountType { get; set; }

        public string? DiscountName { get; set; } 

        [Precision(18, 2)]
        public decimal? Value { get; set; }


    }
}
