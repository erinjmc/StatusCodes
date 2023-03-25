namespace StatusCodes.API.Models
{
    public interface IStatusRepository
    {
        Task<IEnumerable<StatusCode>> GetCodesAsync();
    }
}
