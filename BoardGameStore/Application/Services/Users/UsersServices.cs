using Application.DTO.Output.Users;
using Application.Exceptions;
using Application.Interfaces.Auth;
using Application.Interfaces.DBContext;
using Application.Interfaces.Users;
using Domain.Models.UserModel;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using static System.Net.WebRequestMethods;


namespace Application.Services.Users.UsersServices
{
    public class UsersServices : IUsersServices
    {
        private readonly IAuthValidationService _authValidationService;
        private readonly IPasswordHasher _passwordHasher;
        private readonly IUsersRepository _usersRepository;
        private readonly IJWTProvider _jwtProvider;

        public UsersServices(
            IJWTProvider jwtProvider,
            IUsersRepository usersRepository,
            IPasswordHasher passwordHasher,
            IAuthValidationService authValidationService)
        {
            _authValidationService = authValidationService;
            _passwordHasher = passwordHasher;
            _usersRepository = usersRepository;
            _jwtProvider = jwtProvider;
        }

        public async Task<string> Register(string userName, string userLastName, string password, string email, string phoneNumber)
        {
            var validatedData = await _authValidationService.ValidateRegisterAsync(email, phoneNumber);
            var hashedPassword = _passwordHasher.Generate(password);
            var user = UsersModel.Create(userName, userLastName, hashedPassword, validatedData.Email, validatedData.PhoneNumber);
            var created = await _usersRepository.AddUser(user);
            if (!created)
            {
                throw new InvalidOperationException("User registration failed.");
            }

            return _jwtProvider.GenerateToken(user);
        }

        public async Task<string> Login(string email, string password)
        {
            var user = await _usersRepository.GetByEmail(email);

            var ok = _passwordHasher.Verify(password, user.PasswordHash);
            if (!ok)
            {
                throw new UnauthorizedException();
            }

            var token = _jwtProvider.GenerateToken(user);

            return token;

        }

        public async Task<UserInfoDTO> GetUserByIdAsync(int jwt, CancellationToken ct)
        {
            var userInfo = await _usersRepository.GetById(jwt);

            return new UserInfoDTO() 
            {
                Name = userInfo?.UserName,
                LastName = userInfo?.UserLastname,
                Email = userInfo?.Email

            };
        }

        public async Task<List<UserInfoDTO>> GetUserList(CancellationToken ct)
        {
            var users = await _usersRepository.GetAll(ct);

            return users.Select(user => new UserInfoDTO
            {
                Name = user.UserName,
                LastName = user.UserLastname,
                Email = user.Email
            }).ToList();
        }
    }
}
