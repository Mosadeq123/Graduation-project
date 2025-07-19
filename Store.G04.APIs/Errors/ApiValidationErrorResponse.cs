namespace Store.G04.APIs.Errors
{
    public class ApiValidationErrorResponse : ApiErrorResponse
    {
        public IEnumerable<String> Errors { get; set; } = new List<string>();
        public ApiValidationErrorResponse() : base(400)
        {
            
        }
    }
}
