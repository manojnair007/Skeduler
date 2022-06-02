using Appointment.Core.Entities;
using SmartAppointment.Core.Entities;
using SmartAppointment.Core.Interfaces.Models;
using SmartAppointment.Core.Interfaces.Repositories;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SmartAppointment.Models
{
    public class LoginModel : ILoginModel
    {
        private IUserRepository _userRepository;

        public LoginModel(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<bool> AddUser(Claims claims)
        {
            var user = await _userRepository.InsertItemAsync(new UserInfo()
            {
                FirstName = claims.FirstName,
                EmailId = claims.Email,
                LastName = claims.LastName,
            });

            return !string.IsNullOrEmpty(user.Id);
        }
    }
}
