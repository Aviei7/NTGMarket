namespace Application.Interfaces.Users
{
    public interface IAuthValidationService
    {
        string ValidateEmail(string email);

        string ValidatePhone(string phoneNumber);

        (string Email, string PhoneNumber) ValidateContact(
            string email,
            string phoneNumber);

        Task<(string Email, string PhoneNumber)> ValidateRegisterAsync(
            string email,
            string phoneNumber);
    }
}
