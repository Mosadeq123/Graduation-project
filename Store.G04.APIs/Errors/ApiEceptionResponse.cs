namespace Store.G04.APIs.Errors
{
    public class ApiEceptionResponse : ApiErrorResponse
    {
        public string? Details { get; set; }
        public ApiEceptionResponse(int statusCode, string? message = null, string? details = null)
            : base(statusCode, message)
        {
            Details = details;

        }
    }
}
