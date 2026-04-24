using Application.DTO.Output.Users;
using Application.Interfaces.Cabinet;
using Application.Interfaces.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services.Cabinet
{
    public class CabinetServices : ICabinetServices
    {
        private readonly IUsersRepository _usersRepository;

        public CabinetServices(IUsersRepository usersRepository)
        {
            _usersRepository = usersRepository;
        }

        public async Task<UserInfoDTO> GetInfoByUser(string email)
        {
            var user = await _usersRepository.GetByEmail(email);

            return new UserInfoDTO { Email = user.Email, LastName = user.UserLastname, Name = user.UserName };
        }
    }
}
