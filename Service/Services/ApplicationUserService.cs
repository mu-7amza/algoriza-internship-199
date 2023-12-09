using Core.Entities;
using Infrastructure.Data;
using Infrastructure.Repositeries;
using Service.Abstractions;

namespace Application.Services
{
    public class ApplicationUserService : Repository<ApplicationUser>, IApplicationUserService
    {
        private readonly ApplicationDbContext _db;

        public ApplicationUserService(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }


        public void Update(ApplicationUser obj)
        {
            _db.ApplicationUsers.Update(obj);
        }
    }
}
