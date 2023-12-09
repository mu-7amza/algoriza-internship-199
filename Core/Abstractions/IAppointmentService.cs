﻿using Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Abstractions
{
    public interface IAppointmentService : IRepository<Appointment>
    {
        void Update(Appointment obj);
    }
}
