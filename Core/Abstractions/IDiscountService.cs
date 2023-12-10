using Core.Abstractions;
using Core.Entities;

namespace Service.Abstractions
{
    public interface IDiscountService : IRepository<Discount>
    {
        Task UpdateAsync(Discount obj);
    }
}
