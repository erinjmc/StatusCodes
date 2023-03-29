namespace StatusCodes.API.Models
{
    public class ResultDto
    {
        public bool IsSuccess { get; set; }
        public int ErrorCode { get; set; }
        public string Message { get; set; }
        public Object Body { get; set; }
    }
}
