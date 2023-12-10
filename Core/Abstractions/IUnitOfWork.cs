using Service.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Abstractions
{
    public interface IUnitOfWork
    {
        IApplicationUserService ApplicationUser { get; }
        ISpecializationService Specialization { get; }
        IAppointmentService Appointment { get; }
        ICouponService Coupon { get; }
        IDiscountService Discount { get; }
        Task SaveChangesAsync();
    }
}
