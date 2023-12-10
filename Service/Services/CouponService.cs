using Core.Entities;
using Infrastructure.Data;
using Infrastructure.Repositeries;
using Microsoft.EntityFrameworkCore;
using Service.Abstractions;

namespace Application.Services
{
    public class CouponService : Repository<Coupon>, ICouponService
    {

        private readonly ApplicationDbContext _db;

        public CouponService(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }


        public void Update(Coupon obj)
        {
             _db.Coupons.Update(obj);
        }

    }
}
