using System.Net;
using E_commerce.API.Helper;
using System.Text.Json;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.AspNetCore.Http;


namespace E_commerce.API.Middlewares
{
    /// will regiter this IMemoryCache >>>>>>> in program file

    //middleware as a door when i need req like get Gategory and any thing in app i need to enter this door  first 
    //can make sequritty or limitation and alot of thing by it 
    public class ExpectionMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly IHostEnvironment _hostEnvironment;
        private readonly IMemoryCache _memoryCache;
        //make limited time 30 sec
        private readonly TimeSpan _rateLimitWindow=TimeSpan.FromSeconds(30);

        public ExpectionMiddleware(RequestDelegate next, IHostEnvironment hostEnvironment, IMemoryCache memoryCache)
        {
            _next = next;
            _hostEnvironment = hostEnvironment;
            _memoryCache = memoryCache;

        }
        public async Task InvokeAsync(HttpContext context)
        {
            ApplySecurity(context);


            if (!IsRequestAllowed(context))
            {
                context.Response.StatusCode = (int)HttpStatusCode.TooManyRequests;
                context.Response.ContentType = "application/json";

                
                var response = new
                {
                    StatusCode = (int)HttpStatusCode.TooManyRequests,
                    Message = "Too many requests. Please try again after 30 seconds.",
                    Timestamp = DateTime.UtcNow
                };

              
                await context.Response.WriteAsync(JsonSerializer.Serialize(response));
                return;
            }

            
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                //when production will make  _hostEnvironment.IsProduction()
                context.Response.ContentType = "application/json";
                var errorResponse = _hostEnvironment.IsDevelopment() ?
                 new ApiException((int)HttpStatusCode.InternalServerError,ex.Message,ex.StackTrace) : new ApiException((int)HttpStatusCode.InternalServerError, ex.Message);
                await context.Response.WriteAsync(JsonSerializer.Serialize(errorResponse));
            }
        }


        //Rate Limit
        // why >> when bot make 1000 or more req in sec to server>> server will down 
        // so i need t o handle the num of req in website 
        // bot may have no reqister  so i will use IP that make this req

        private bool IsRequestAllowed(HttpContext context)
        {
            var ip = context.Connection.RemoteIpAddress.ToString();
            var cacheKey = $"Rate:{ip}";
            DateTime dateNow = DateTime.Now;

            var cacheEntry = _memoryCache.GetOrCreate(cacheKey, entry =>
            {
                entry.AbsoluteExpirationRelativeToNow = _rateLimitWindow;
                return (TimeStamp: dateNow, Count: 1); //start count of 1
            });

            if (dateNow - cacheEntry.TimeStamp < _rateLimitWindow)
            {
                if (cacheEntry.Count >= 8)
                {
                    return false;
                }
                
                // still save untill limited time
                _memoryCache.Set(cacheKey, (cacheEntry.TimeStamp, cacheEntry.Count + 1), _rateLimitWindow);
            }
            else
            {
                // Reset window and count
                _memoryCache.Set(cacheKey, (cacheEntry.TimeStamp, cacheEntry.Count ), _rateLimitWindow);
            }

            return true;
        }


        //Actions strengthen the security of your web application against several common types of attacks and are essential for securing any modern web application.
        //These settings are applied as HTTP headers in the response sent from the server to the browser.
        private void ApplySecurity(HttpContext context)
        {
            context.Response.Headers["X-Content-Type-Options"] = "nosniff";
            context.Response.Headers["X-XSS-Protection"] = "1;mode=block";
            context.Response.Headers["X-Frame-Options"] = "DENY";
        }






    }
}
