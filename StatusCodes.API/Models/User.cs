using System.ComponentModel.DataAnnotations.Schema;

namespace StatusCodes.API.Models
{
    public class User
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Salt { get; set; }
        public string HashedPassword { get; set; }
        public bool IsAdmin { get; set; }
        public ICollection<Token> Tokens;

        public User() 
        {
            Tokens = new List<Token>();
        }
    }
}
