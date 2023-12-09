using Core.Abstractions;
using Core.Entities;

namespace Service.Abstractions
{
    public interface IApplicationUserService : IRepository<ApplicationUser>
    {
        void Update(ApplicationUser obj);
    }
}
