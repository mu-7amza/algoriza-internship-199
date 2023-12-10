using Core.Abstractions;
using Core.Entities;
using Infrastructure.Data;
using Infrastructure.Repositeries;


namespace Application.Services
{
    public class BookingService : Repository<Booking>, IBookingService
    {
        private readonly ApplicationDbContext _db;

        public BookingService(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }



        public void Update(Booking obj)
        {
            _db.Bookings.Update(obj);
        }



        // Implement IAppointmentService methods

    }
}
