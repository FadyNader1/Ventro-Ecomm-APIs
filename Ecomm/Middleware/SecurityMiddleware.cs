namespace Ecomm.Middleware
{
    public class SecurityMiddleware
    {
        private readonly RequestDelegate next;
        private const int MaxRequestSizeInBytes = 10 * 1024 * 1024; // 10 MB

        public SecurityMiddleware(RequestDelegate next)
        {
            this.next = next;
        }

        public Task Invoke(HttpContext httpContext)
        {

            //limit request size
            if (httpContext.Request.ContentLength > MaxRequestSizeInBytes)
            {
                httpContext.Response.StatusCode = StatusCodes.Status413PayloadTooLarge;
                return httpContext.Response.WriteAsync("Payload too large");
            }

            httpContext.Response.Headers["X-Content-Type-Options"] = "nosniff";
            //httpContext.Response.Headers["X-XSS-Protection"] = "1; mode=block";
            httpContext.Response.Headers["X-Frame-Options"] = "DENY";
            httpContext.Response.Headers["Strict-Transport-Security"] = "max-age=31536000; includeSubDomains";
            //csp policy
            httpContext.Response.Headers["Content-Security-Policy"] = string.Join(";", new string[]
            {
                "default-src 'self'",
                "script-src 'self' ",
                "style-src 'self' ",
                "img-src 'self' data: ",
                "font-src 'self' ",
                "connect-src 'self' ",
                "frame-ancestors 'none'",
                "form-action 'self' ",
                "base-uri 'self' ",
                "block-all-mixed-content",
                "frame-src 'none' ",
                "object-src 'none' "

            });
            httpContext.Response.Headers["Referrer-Policy"] = "origin-when-cross-origin";
            httpContext.Response.Headers["Permissions-Policy"] = "geolocation=(self), microphone=(self), camera=(self) ";
            
            
            return next(httpContext);






        }
    }
}
