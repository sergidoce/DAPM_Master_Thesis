using ApiGetWay.Common;
using ApiGetWay.Models;
using DBDapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using Ocelot.Authorization;
using Ocelot.Errors;
using Ocelot.Logging;
using Ocelot.Middleware;
using Ocelot.Responses;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

/// <summary>
/// Custom authorization middleware class: Uses the identity information stored by the authentication middleware for authorization verification
/// Be sure to enable the authentication middleware first (app.UseAuthentication()), which will verify the identity information in the request and store it in the HttpContext.User property.
/// If the authentication middleware is not enabled, the authorization middleware will not be able to obtain identity information and therefore cannot perform authorization verification.
/// </summary>
public class AuthorizationWithCustMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<AuthorizationWithCustMiddleware> _logger;
    private readonly RedisHelper _redis;
    /// <summary>
    /// 1. Must have a public constructor, and the constructor must contain at least one parameter of type RequestDelegate
    /// </summary>
    /// <param name="next"></param>
    /// <param name="logger"></param>
    public AuthorizationWithCustMiddleware(RequestDelegate next, ILogger<AuthorizationWithCustMiddleware> logger, RedisHelper redis)
    {
        _next = next;
        _logger = logger;
        _redis = redis;
    }

    /// <summary>
    /// 2. Must have a public method named Invoke or InvokeAsync, which must contain a parameter of type HttpContext as the first parameter, and the return type must be Task
    /// </summary>
    /// <param name="context"></param>
    /// <returns></returns>
    public async Task Invoke(HttpContext context)
    {
        // Get the request path
        string path = context.Request.Path;

        // 1. Validate the token; if the path is a login request, do not intercept
        if (!path.Contains("/UserInfo/Login") && !path.Contains("/UserInfo/UserRegistration"))
        {
            object? tokenObj = context.Request.Headers["token"]; // Generally, the token is placed in the request header
            if (tokenObj == null || string.IsNullOrEmpty(tokenObj.ToString()))
            {
                await ResetErrorContent(context, 500, "Token cannot be empty!");
                return;
            }
            else
            {
                RedisValue redisValue = _redis.GetDatabase().StringGet(tokenObj.ToString());
                if (string.IsNullOrEmpty(redisValue.ToString()))
                {
                    await ResetErrorContent(context, 300, "Token has expired, please log in again!");
                    return;
                }
                else
                { // If not expired, extend the expiration time for each operation
                    _redis.GetDatabase().KeyExpire(tokenObj.ToString(), TimeSpan.FromMinutes(30)); // Update expiration time
                }
            };
            // 2. Verify access permissions; does the user have permission to access the interface
            // To be continued---
            // Read token, verify user permission information
            string token = context.Request.Headers["token"].ToString();
            UserInfo? user = JsonConvert.DeserializeObject<UserInfo>(_redis.GetDatabase().StringGet(token).ToString());
            if (user == null)
            {
                await ResetErrorContent(context, 900, "User login information not found!");
                return;
            }
            string sql = "select \"Id\",\"InterfaceName\",\"InterfaceType\" from sys_interface_auth where POSITION(\"InterfaceName\" IN '" + path + "')>0 and \"Status\"=1";
            List<InterfaceAuth> UserFiles = SqlDapperHelper.QueryList<InterfaceAuth>("1", sql, null);
            if (UserFiles.Count > 0)  // If the interface requires permission verification
            {
                InterfaceAuth UserFile = UserFiles.FirstOrDefault();
                // Administrators can operate without authorization
                if (UserFile.InterfaceType == "create repository" || UserFile.InterfaceType == "administrator")
                {
                    if (user.RoleName != "administrator")
                    {
                        await ResetErrorContent(context, 900, "No permission to access this content!");
                        return;
                    }
                }
                else
                {
                    // Check if there is permission to create resource
                    if (UserFile.InterfaceType == "create resource")
                    {
                        List<string> permission_repositoryIDs = new List<string>();
                        if (!string.IsNullOrEmpty(user.permission_repositoryID)) {
                            permission_repositoryIDs = user.permission_repositoryID.Split(",").ToList();
                        }
                        List<string> permission_repository_createIDs = new List<string>();
                        if (!string.IsNullOrEmpty(user.permission_repository_createID))
                        {
                            permission_repository_createIDs = user.permission_repository_createID.Split(",").ToList();
                        }
                        if (permission_repositoryIDs.Where(o => path.Contains(o)).Count() <= 0 && permission_repository_createIDs.Where(o => path.Contains(o)).Count() <= 0)
                        {
                            await ResetErrorContent(context, 900, "No permission to access this content!");
                            return;
                        }
                    }
                    else if (UserFile.InterfaceType == "read resource")
                    {
                        List<string> permission_repositoryIDs = new List<string>();
                        List<string> permission_repository_readIDs = new List<string>();
                        List<string> permission_resource_readIDs = new List<string>();
                        if (!string.IsNullOrEmpty(user.permission_repositoryID))
                        {
                            permission_repositoryIDs = user.permission_repositoryID.Split(",").ToList();
                        }
                        if (!string.IsNullOrEmpty(user.permission_repository_readID))
                        {
                            permission_repository_readIDs = user.permission_repository_readID.Split(",").ToList();
                        }
                        if (!string.IsNullOrEmpty(user.permission_resource_readID))
                        {
                            permission_resource_readIDs = user.permission_resource_readID.Split(",").ToList();
                        }
                        if (permission_repositoryIDs.Where(o => path.Contains(o)).Count() <= 0 && permission_repository_readIDs.Where(o => path.Contains(o)).Count() <= 0 && permission_resource_readIDs.Where(o => path.Contains(o)).Count() <= 0)
                        {
                            await ResetErrorContent(context, 900, "No permission to access this content!");
                            return;
                        }
                    }
                    else if (UserFile.InterfaceType == "download resource")
                    {
                        List<string> permission_repositoryIDs = new List<string>();
                        List<string> permission_resource_downloadIDs = new List<string>();
                        if (!string.IsNullOrEmpty(user.permission_repositoryID))
                        {
                            permission_repositoryIDs = user.permission_repositoryID.Split(",").ToList();
                        }
                        if (!string.IsNullOrEmpty(user.permission_resource_downloadID))
                        {
                            permission_resource_downloadIDs = user.permission_resource_downloadID.Split(",").ToList();
                        }
                        if (permission_repositoryIDs.Where(o => path.Contains(o)).Count() <= 0 && permission_resource_downloadIDs.Where(o => path.Contains(o)).Count() <= 0)
                        {
                            await ResetErrorContent(context, 900, "No permission to access this content!");
                            return;
                        }
                    }
                    else if (UserFile.InterfaceType == "execute/deploy pipeline")
                    {
                        List<string> permission_resource_readIDs = new List<string>();
                        if (!string.IsNullOrEmpty(user.permission_resource_readID))
                        {
                            permission_resource_readIDs = user.permission_resource_readID.Split(",").ToList();
                        }
                        if (permission_resource_readIDs.Where(o => path.Contains(o)).Count() <= 0)
                        {
                            await ResetErrorContent(context, 900, "No permission to access this content!");
                            return;
                        }
                    }
                }
            }
        }
        // Go through the gateway
        await _next(context);
        var code = context.Response.StatusCode;
        if (code != StatusCodes.Status200OK)
        {
            // Unauthorized
            var value = context.Response.Headers["WWW-Authenticate"];
            await ResetErrorContent(context, code, value);
            return;
        }
    }
    /// <summary>
    /// Reset error content
    /// </summary>
    /// <param name="context"></param>
    /// <param name="statusCode"></param>
    /// <param name="message"></param>
    private async Task ResetErrorContent(HttpContext context, int statusCode, string message)
    {
        context.Response.StatusCode = StatusCodes.Status200OK;
        context.Response.ContentType = "application/json; charset=utf-8";
        await context.Response.WriteAsync("{\"code\": " + statusCode + ", \"message\": \"" + (message ?? "Unauthorized") + "\",\"data\":\"\"}");
        await context.Response.CompleteAsync();
    }
}
