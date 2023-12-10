using Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Abstractions
{
    public interface IBookingService : IRepository<Booking>
    {
        void Update(Booking obj);
    }
}
