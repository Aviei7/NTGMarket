using Application.DTO.Output.Users;
using Application.Interfaces.DBContext;
using Application.Interfaces.Users;
using Domain.Models.FiltersModel;
using Domain.Models.UserModel;
using Infrastucture.Context;
using Microsoft.EntityFrameworkCore;
using Application.DTO.Output.Checkout;
using Application.DTO.Input.Checkout;
using Infrastructure.Migrations;

namespace Infrastructure.Repositories
{
    public class UsersRepository : IUsersRepository
    {
        private readonly BoardStoreContext _context;
        public UsersRepository(BoardStoreContext context)
        {
            _context = context;
        }

        public async Task<bool> AddUser(UsersModel user)
        {

            await _context.Users.AddAsync(user);
            var result = await _context.SaveChangesAsync();

            return result > 0;
        }

        public async Task<UsersModel?> GetByEmail(string Email)
        {
            return await _context.Users.AsNoTracking().FirstOrDefaultAsync(x => x.Email == Email);
        }

        public async Task<UsersModel?> GetById(int id)
        {
            return await _context.Users.AsNoTracking().FirstOrDefaultAsync(x => x.UserID == id);
        }

        public async Task<List<UsersModel>> GetAll(CancellationToken ct)
        {
            return await _context.Users
                .AsNoTracking()
                .ToListAsync(ct);
        }

        public async Task<int> GetGuestUserID()
        {
            var email = "guestorder@guest.com";

            var user = await _context.Users.AsNoTracking().FirstOrDefaultAsync(x => x.Email == email);
            return user?.UserID
                ?? throw new InvalidOperationException("Guest system user was not found in the Users table.");
        }
    }
}
