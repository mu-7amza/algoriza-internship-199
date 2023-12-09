using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Utility
{
    public static class SD
    {
        // Request Status
        public const string REQUEST_PENDING = "Pending";
        public const string REQUEST_COMPLETED = "Completed";
        public const string REQUEST_Canceled = "Canceled";

        // Roles
        public const string ROLE_ADMIN = "Admin";
        public const string ROLE_DOCTOR = "Doctor";
        public const string ROLE_PATIENT = "Patient";

    }
}
