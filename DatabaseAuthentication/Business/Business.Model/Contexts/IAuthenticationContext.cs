using System.Security;

namespace Business.Model
{
    public interface IAuthenticationContext
    {
        UserModel AuthenticateUser(string username, SecureString password);
    }
}