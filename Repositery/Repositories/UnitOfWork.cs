using Core.Abstractions;
using Infrastructure.Data;
using Service.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Repositeries
{
    public class UnitOfWork(ApplicationDbContext db, IApplicationUserService ApplicationUser, ISpecializationService specialization, IAppointmentService appointment , ICouponService coupon , IDiscountService discount , IBookingService booking) : IUnitOfWork
    {
        private readonly ApplicationDbContext _db = db;
        public IApplicationUserService ApplicationUser { get; private set; } = ApplicationUser;
        public ISpecializationService Specialization { get; private set; } = specialization;
        public IAppointmentService Appointment { get; private set; } = appointment;
        public ICouponService Coupon { get; private set; } = coupon;
        public IDiscountService Discount { get; private set; } = discount;
        public IBookingService Booking { get; private set; } = booking;

        public async Task SaveChangesAsync()
        {
            await _db.SaveChangesAsync();
        }
    }
}
