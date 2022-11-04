namespace Application.Services
{
    public interface IPasswordIssuer
    {
        bool Verify(string password, string hashToMatch);
        string Hash(string password);
    }

    public class PasswordIssuer : IPasswordIssuer
    {
        public bool Verify(string password, string hashToMatch)
        {
            return BCrypt.Net.BCrypt.Verify(password, hashToMatch);
        }

        public string Hash(string password)
        {
            return BCrypt.Net.BCrypt.HashPassword(password);
        }
    }
}