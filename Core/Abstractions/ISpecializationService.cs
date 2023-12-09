using Core.Abstractions;
using Core.Entities;

namespace Service.Abstractions
{
    public interface ISpecializationService : IRepository<Specialization>
    {
        Task Update(Specialization obj);
    }
}
