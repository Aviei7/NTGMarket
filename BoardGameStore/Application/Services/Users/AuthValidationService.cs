using Application.Exceptions;
using Application.Interfaces.Users;
using System.Text.RegularExpressions;

namespace Application.Services.Users.AuthValidationService
{
    public class AuthValidationService : IAuthValidationService
    {
        private static readonly Regex ValidEmailRegex =
            new(@"^[^@\s]+@[^@\s]+\.[^@\s]+$", RegexOptions.Compiled | RegexOptions.IgnoreCase);

        private static readonly Regex ValidPhoneRegex =
            new(@"^\+380\d{9}$", RegexOptions.Compiled);

        private readonly IUsersRepository _usersRepository;

        public AuthValidationService(IUsersRepository usersRepository)
        {
            _usersRepository = usersRepository;
        }

        public (string Email, string PhoneNumber) ValidateContact(
            string email,
            string phoneNumber)
        {
            return (ValidateEmail(email), ValidatePhone(phoneNumber));
        }

        public string ValidateEmail(string email)
        {
            return NormalizeEmail(email);
        }

        public string ValidatePhone(string phoneNumber)
        {
            return NormalizePhone(phoneNumber);
        }

        public async Task<(string Email, string PhoneNumber)> ValidateRegisterAsync(
            string email,
            string phoneNumber)
        {
            var normalizedEmail = NormalizeEmail(email);
            var normalizedPhone = NormalizePhone(phoneNumber);

            var existingUser = await _usersRepository.GetByEmail(normalizedEmail);
            if (existingUser is not null)
                throw new ValidationException("Користувач з таким email вже існує.");

            return (normalizedEmail, normalizedPhone);
        }

        private static string NormalizeEmail(string email)
        {
            var normalizedEmail = (email ?? string.Empty).Trim().ToLowerInvariant();

            if (string.IsNullOrWhiteSpace(normalizedEmail) || !ValidEmailRegex.IsMatch(normalizedEmail))
                throw new ValidationException("Введіть коректний email.");

            return normalizedEmail;
        }

        private static string NormalizePhone(string phoneNumber)
        {
            var digits = new string((phoneNumber ?? string.Empty).Where(char.IsDigit).ToArray());

            if (digits.StartsWith("380"))
                digits = "+" + digits;
            else if (digits.Length == 10 && digits.StartsWith("0"))
                digits = "+38" + digits;
            else if (digits.Length == 9)
                digits = "+380" + digits;

            if (!ValidPhoneRegex.IsMatch(digits))
                throw new ValidationException("Номер телефону має бути у форматі +380XXXXXXXXX.");

            return digits;
        }
    }
}
