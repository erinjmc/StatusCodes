namespace StatusCodes.API.DbContext
{
    public class InitStatusCode
    {
        public int PlatformCode { get; set; }
        public int Code { get; set; }
        public string Desc { get; set; } = string.Empty;
        public string? Platform { get; set; }
    }
}
