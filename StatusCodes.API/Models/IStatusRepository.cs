namespace StatusCodes.API.Models
{
    public interface IStatusRepository
    {
        Task<IEnumerable<StatusCode>> GetCodes();
        Task<IEnumerable<User>> GetUsers();
        Task<User> GetUser(int id);
    }
}
