using Application.Interfaces.Auth;

namespace Infrastructure.Auth.Users
{
    public class PasswordHasher : IPasswordHasher
    {
        /*Generate password*/
        public string Generate(string password) => BCrypt.Net.BCrypt.EnhancedHashPassword(password);


        /*Equals hashes password*/
        public bool Verify(string password, string hashedPassword) => BCrypt.Net.BCrypt.EnhancedVerify(password, hashedPassword);
    }
}
