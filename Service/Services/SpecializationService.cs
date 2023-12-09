using Core.Entities;
using Infrastructure.Data;
using Infrastructure.Repositeries;
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


        public void Update(Specialization obj)
        {
            _db.Specializations.Update(obj);
        }

    }
}
