using Core.Abstractions;
using Core.Entities;

namespace Service.Abstractions
{
    public interface ISpecializationService : IRepository<Specialization>
    {
        void Update(Specialization obj);
    }
}
