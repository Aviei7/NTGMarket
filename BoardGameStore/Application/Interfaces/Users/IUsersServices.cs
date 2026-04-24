using Application.DTO.Output.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IdentityModel.Tokens.Jwt;

namespace Application.Interfaces.Users
{
    public interface IUsersServices
    {
        public Task<string> Register(string userName, string userLastName, string password, string email, string phoneNumber);

        public Task<string> Login(string email, string password);

        public Task<UserInfoDTO> GetUserByIdAsync(int jwt, CancellationToken ct);

        public Task<List<UserInfoDTO>> GetUserList(CancellationToken ct);
    }
}
