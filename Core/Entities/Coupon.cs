using Core.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entities
{
    public class Coupon
    {
        public int Id { get; set; }

        [Required]
        public string Code { get; set; }

        public int DiscountId { get; set; }

        [ForeignKey(nameof(DiscountId))]
        public Discount Discount { get; set; }

        public bool IsValid { get; set; } = true;

    }
}
