using Core.Abstractions;
using Core.Entities;

namespace Service.Abstractions
{
    public interface ICouponService : IRepository<Coupon>
    {
        void Update(Coupon obj);
    }
}
