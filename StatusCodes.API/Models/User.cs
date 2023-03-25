namespace StatusCodes.API.Models
{
    public class User
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public bool IsAdmin { get; set; }
        public List<Token> Tokens { get; set; }

        public User(int id, string firstName, string lastName, string email, bool isAdmin)
        {
            Id = id;
            FirstName = firstName;
            LastName = lastName;
            Email = email;
            IsAdmin = isAdmin;
        }
    }
}
