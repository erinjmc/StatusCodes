namespace StatusCodes.API.Models
{
    public class User
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string HashedPassword { get; set; }
        public bool IsAdmin { get; set; }
        public List<Token> Tokens = new List<Token>();
    }
}
