using System;
using System.Collections.Generic;
using System.Text;

namespace SmartAppointment.Core.Entities
{
    public class Claims
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }

        public bool IsNewUser { get; set; }
    }
}
