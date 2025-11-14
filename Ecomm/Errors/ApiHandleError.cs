namespace Ecomm.Errors
{
    public class ApiHandleError
    {
        public string? Message { get; set; }
        public int StatusCode { get; set; }
        public ApiHandleError(int statusCode, string? message=null)
        {
            this.StatusCode= statusCode;
            this.Message = message ?? HandleErrorMessage(); ;
        }

        private string HandleErrorMessage()
        {
            return StatusCode switch
            {
                200 => "Success",
                400 => "Bad Request",
                401 => "Unauthorized Access",
                404 => "Resource Not Found",
                500 => "Internal Server Error",
                _ => "An unexpected error occurred"
            };
        }
    }
}
