using Core.Entities;
using Infrastructure.Data;
using Infrastructure.Repositeries;
using Microsoft.EntityFrameworkCore;
using Service.Abstractions;

namespace Application.Services
{
    public class DiscountService : Repository<Discount>, IDiscountService
    {

        private readonly ApplicationDbContext _db;

        public DiscountService(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }


        public async Task UpdateAsync(Discount obj)
        {
            await _db.Discounts.ExecuteUpdateAsync(s => s.SetProperty(p => p.Name , obj.Name)
                .SetProperty(p => p.DiscountType , obj.DiscountType)
                .SetProperty(p => p.Value , obj.Value));
        }

    }
}
