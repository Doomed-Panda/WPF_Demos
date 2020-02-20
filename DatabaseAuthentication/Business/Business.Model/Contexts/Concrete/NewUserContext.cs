using Business.Core;
using Microsoft.EntityFrameworkCore;
using System;
using System.Security;

namespace Business.Model
{
    public class NewUserContext : DbContext, INewUserContext
    {
        //NOTE: Connection strings should be stored in a config file, this is demo code only
        protected override void OnConfiguring(DbContextOptionsBuilder options) => options.UseSqlite(@"Data Source=Users.db");

        public DbSet<UserModel> Users { get; set; }

        public bool CreateNewUser(string username, SecureString securePassword)
        {
            try
            {
                UserModel newUser = new UserModel();
                newUser.Username = username;
                newUser.PasswordHash = PasswordHelper.GenerateNewHash(securePassword);
                Users.Add(newUser);
                base.SaveChanges();
            }
            catch (Exception)
            {
                return false;
            }
            return true;
        }
    }
}