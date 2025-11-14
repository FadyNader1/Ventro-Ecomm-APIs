namespace Ecomm.Errors
{
    public class ApiServerError:ApiHandleError
    {
        public string? Details { get; set; }
        public ApiServerError(int StatusCode, string? Message= null,string? details=null):base(StatusCode,Message)
        {
            
        }
    }
}
