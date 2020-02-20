using Business.Core;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Security;

namespace Business.Model
{
    public class AuthenticationContext : DbContext, IAuthenticationContext
    {
        //NOTE: Connection strings should be stored in a config file, this is demo code only
        protected override void OnConfiguring(DbContextOptionsBuilder options) => options.UseSqlite(@"Data Source=Users.db");

        public DbSet<UserModel> Users { get; set; }

        public UserModel AuthenticateUser(string username, SecureString password)
        {
            UserModel matchedUser = Users.Where(u => u.Username == username).SingleOrDefault();

            if (PasswordHelper.PasswordCompare(password, matchedUser.PasswordHash))
            {
                matchedUser.IsAuthenticated = true;
                return matchedUser;// matchedUser is returned if a valid password has been submitted, or matchedUser is a default UserModel
            }
            else
            {
                return new UserModel();// if a matching user is found, but the password is invalid, return a default UserModel
            }
        }
    }
}