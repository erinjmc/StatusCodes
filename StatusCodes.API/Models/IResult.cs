namespace StatusCodes.API.Models
{
    public interface IResult
    {   bool IsSuccess { get; set; }
        int ErrorCode { get; set; }
        string Message { get; set; }
        Object Body { get; set; }
    }
}
