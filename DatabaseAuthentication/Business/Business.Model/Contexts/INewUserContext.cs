using System.Security;

namespace Business.Model
{
    public interface INewUserContext
    {
        bool CreateNewUser(string username, SecureString securePassword);
    }
}