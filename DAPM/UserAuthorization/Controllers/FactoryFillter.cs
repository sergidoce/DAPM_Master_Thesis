using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using StackExchange.Redis;
using UserAuthorization.Common;

namespace UserAuthorization.Controllers
{
    public class FactoryFillter : ActionFilterAttribute
    {
        private readonly RedisHelper _redis;

        public FactoryFillter(RedisHelper redis)
        {
            _redis = redis;
        }
        /// <summary>
        /// API interceptor (verify before each interface call)
        /// </summary>
        /// <param name="context"></param>
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            Debug.WriteLine("Before interception");
            // Get the request path
            string path = context.HttpContext.Request.Path;
            Debug.WriteLine("Request path: " + path);

            // When the path is a login request, do not intercept
            if (!path.Contains("/UserInfo/Login") && !path.Contains("/UserInfo/UserRegistration"))
            {
                object? tokenObj = context.HttpContext.Request.Headers["token"]; // Generally, the token is placed in the request header
                if (tokenObj == null || string.IsNullOrEmpty(tokenObj.ToString()))
                {
                    context.Result = new JsonResult(new { code = "500", message = "Token cannot be empty!", data = "" });
                    return;
                }
                else
                {
                    RedisValue redisValue = _redis.GetDatabase().StringGet(tokenObj.ToString());
                    if (string.IsNullOrEmpty(redisValue.ToString()))
                    {
                        context.Result = new JsonResult(new { code = "300", message = "Token has expired, please log in again!", data = "" });
                        return;
                    }
                    else
                    { // If not expired, extend the expiration time for each operation
                        _redis.GetDatabase().KeyExpire(tokenObj.ToString(), TimeSpan.FromMinutes(30)); // Update expiration time
                    }
                }
            }
        }
    }
}
