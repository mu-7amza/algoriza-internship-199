using Core.Abstractions;
using Infrastructure.Data;
using Service.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Repositeries
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ApplicationDbContext _db;
        public IApplicationUserService ApplicationUser { get; private set; }
        public ISpecializationService Specialization { get; private set; }
        public IAppointmentService Appointment { get; private set; }

        public UnitOfWork(ApplicationDbContext db, IApplicationUserService ApplicationUser, ISpecializationService specialization, IAppointmentService appointment)
        {
            _db = db;
            this.ApplicationUser = ApplicationUser;
            Specialization = specialization;
            Appointment = appointment;
        }

        public async Task SaveChangesAsync()
        {
            await _db.SaveChangesAsync();
        }
    }
}
