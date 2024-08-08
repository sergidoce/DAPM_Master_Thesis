namespace ApiGetWay.Controllers
{
    /// <summary>
    /// Simple encapsulation of middleware invocation
    /// </summary>
    public static class MiddlewareExtendsions
    {
        /// <summary>
        /// Enable custom authorization middleware
        /// </summary>
        /// <param name="app"></param>
        /// <returns></returns>
        public static IApplicationBuilder UseAuthorizationWithCust(this IApplicationBuilder app)
        {
            return app.UseMiddleware<AuthorizationWithCustMiddleware>();
        }
    }
}
