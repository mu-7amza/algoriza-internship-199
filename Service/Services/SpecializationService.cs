using Core.Entities;
using Infrastructure.Data;
using Infrastructure.Repositeries;
using Microsoft.EntityFrameworkCore;
using Service.Abstractions;

namespace Application.Services
{
    public class SpecializationService : Repository<Specialization>, ISpecializationService
    {

        private readonly ApplicationDbContext _db;

        public SpecializationService(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }


        public async Task Update(Specialization obj)
        {
            await _db.Specializations.ExecuteUpdateAsync(s => s.SetProperty(p => p.Name , obj.Name));
        }

    }
}
