

using DAPM.ClientApi.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Threading.Tasks;

public class ActivityLogMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<ActivityLogMiddleware> _logger;

    public ActivityLogMiddleware(RequestDelegate next, ILogger<ActivityLogMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task Invoke(HttpContext context)
    {
        string username = "Anonymous"; // Default username
        string action = $"{context.Request.Method} {context.Request.Path}";
        string ticketId = "None"; // Default TicketId

        // Retrieve Client IP Address
        string clientIp = GetClientIpAddress(context);

        // Log Authorization Header and Client IP
        _logger.LogInformation("Authorization Header: {Authorization}", context.Request.Headers["Authorization"]);
        _logger.LogInformation("Client IP: {ClientIP}", clientIp);

        try
        {
            // Extract Authorization header
            var authHeader = context.Request.Headers["Authorization"].FirstOrDefault();
            if (!string.IsNullOrEmpty(authHeader) && authHeader.StartsWith("Bearer "))
            {
                var token = authHeader.Substring("Bearer ".Length).Trim();
                var jwtHandler = new JwtSecurityTokenHandler();

                if (jwtHandler.CanReadToken(token))
                {
                    var jwtToken = jwtHandler.ReadJwtToken(token);

                    // Extract 'name' claim as username
                    username = jwtToken.Claims.FirstOrDefault(c => c.Type == "name")?.Value ?? "Unknown";
                    _logger.LogInformation($"Extracted Username from Token: {username}");
                }
            }

            // Extract TicketId from query or headers
            ticketId = context.Request.Query["ticketId"].FirstOrDefault() ??
                       context.Request.Headers["ticketId"].FirstOrDefault() ??
                       "None";

            _logger.LogInformation($"Raw TicketId from Request: {ticketId}");

            // Fallback to User.Identity.Name if still "Anonymous"
            if (username == "Anonymous" && context.User.Identity != null && context.User.Identity.IsAuthenticated)
            {
                username = context.User.Identity.Name ?? "Unknown";
                _logger.LogInformation($"Extracted Username from Identity: {username}");
            }

            // Skip logging for the 'status' endpoint
            if (context.Request.Path.StartsWithSegments("/status", System.StringComparison.OrdinalIgnoreCase))
            {
                await _next(context); // Skip further processing for this middleware
                return;
            }

            // Proceed to the next middleware
            await _next(context);

            // Determine the result based on the HTTP response status code
            var result = context.Response.StatusCode switch
            {
                200 => "Success",
                401 => "Unauthorized",
                403 => "Forbidden",
                404 => "Not Found",
                _ => "Failed"
            };

            // Log the activity with Client IP 
            var activityLogService = context.RequestServices.GetService<IActivityLogService>();
            if (activityLogService != null)
            {
                string detailedAction = $"{action}";
                await activityLogService.LogUserActivity(username, detailedAction, result, DateTime.UtcNow, clientIp);
            }
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error in ActivityLogMiddleware: {ex.Message}");
            throw; // Re-throw the exception to avoid swallowing it
        }
    }


    private string GetClientIpAddress(HttpContext context)
    {
        var remoteIpAddress = context.Connection.RemoteIpAddress;

        if (remoteIpAddress == null)
            return "Unknown";

        // Check if the IP is IPv4-mapped IPv6 and extract the IPv4 address
        if (remoteIpAddress.IsIPv4MappedToIPv6)
        {
            var ipv4 = remoteIpAddress.MapToIPv4().ToString();
            _logger.LogInformation($"Converted IPv6-mapped IPv4 to IPv4: {ipv4}");
            return ipv4;
        }

        return remoteIpAddress.ToString();
    }
}
