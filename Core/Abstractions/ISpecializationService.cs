using Core.Abstractions;
using Core.Entities;
using System.Linq.Expressions;

namespace Service.Abstractions
{
    public interface ISpecializationService : IRepository<Specialization>
    {
        Task Update(Specialization obj);
    }
}
