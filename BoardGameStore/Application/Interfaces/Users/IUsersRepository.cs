using Application.DTO.Output.Users;
using Domain.Models.UserModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces.Users
{
    public interface IUsersRepository
    {
        public Task<bool> AddUser(UsersModel user);

        public Task<UsersModel?> GetByEmail(string Email);

        public Task<UsersModel?> GetById(int id);

        public Task<List<UsersModel>> GetAll(CancellationToken ct);
        public Task<int> GetGuestUserID();
    }
}
