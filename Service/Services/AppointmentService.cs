using Core.Abstractions;
using Core.Entities;
using Infrastructure.Data;
using Infrastructure.Repositeries;


namespace Application.Services
{
    public class AppointmentService : Repository<Appointment>, IAppointmentService
    {
        private readonly ApplicationDbContext _db;

        public AppointmentService(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }



        public void Update(Appointment obj)
        {
            _db.Appointments.Update(obj);
        }



        // Implement IAppointmentService methods

    }
}
