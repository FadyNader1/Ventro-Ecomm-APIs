using Ecomm.core.Specification;

namespace Ecomm.Responses
{
    
    public class ApiResponse<T>
    {
        
        public bool Success { get; set; } = true; 
        public string? Message { get; set; }  
        public Meta? Meta { get; set; }
        public T? Data { get; set; }             
    }
}
