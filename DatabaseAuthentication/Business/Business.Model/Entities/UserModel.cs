using System.ComponentModel.DataAnnotations.Schema;

namespace Business.Model
{
    public class UserModel
    {
        public int Id { get; set; }

        [NotMapped] public bool IsAuthenticated { get; set; }

        public string Username { get; set; }
        public string PasswordHash { get; set; }
    }
}