using Dapper;
using DBDapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.FileSystemGlobbing.Internal;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using StackExchange.Redis;
using System;
using System.Reflection;
using System.Text;
using System.Text.Json.Serialization;
using System.Text.RegularExpressions;
using System.Xml.Linq;
using UserAuthorization.Common;
using UserAuthorization.Models;

namespace UserAuthorization.Controllers
{

    [Route("UserAuthorization/[controller]/[action]")]
    [ApiController]
    public class UserInfoController : CommonController
    {

        private readonly RedisHelper _redis;
        /// <summary>
        /// Initialization
        /// </summary>
        /// <param name="redis"></param>
        public UserInfoController(RedisHelper redis)
        {
            _redis = redis;
        }

        /// <summary>
        /// User Login
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public dynamic Login(UserInfo user)
        {
            try
            {
                user.Password = MD5Encrypt.Encrypt(user.Password);
                if (string.IsNullOrEmpty(user.UserName) || string.IsNullOrEmpty(user.Password))
                {
                    return GenActionResultGenericEx("", ApiResultCode.Failed, "Please enter your username or password!");
                }
                UserInfo model = SqlDapperHelper.Query("1", $"select * from sys_UserInfo where \"UserName\" =@UserName and \"Password\" =@Password and \"Status\"=1", user);
                if (model == null)
                {
                    return GenActionResultGenericEx("", ApiResultCode.Failed, "Incorrect username or password!");
                }
                string json = JsonConvert.SerializeObject(model);
                string token = Guid.NewGuid().ToString();
                _redis.GetDatabase().StringSet(token, json, TimeSpan.FromMinutes(30)); // Store token data in Redis and set expiration time

                // Update login time
                try
                {
                    SqlDapperHelper.Update("1", $"update sys_UserInfo set \"LastLoginTime\"='{DateTime.Now}' where \"UserName\" ='{user.UserName}'");
                }
                catch (Exception ex)
                {

                }
                return new
                {
                    Code = ApiResultCode.Success,
                    Message = "Login successful!",
                    Data = token,
                    UserId = model.Id
                };
                //   return GenActionResultGenericEx(token, ApiResultCode.Success, "Login successful!");
            }
            catch (Exception ex)
            {
                return GenActionResultGenericEx("", ApiResultCode.Failed, ex.Message);
            }
        }


        /// <summary>
        /// User registration
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        [HttpPost]
        public ApiResult<string> UserRegistration(UserInfo user)
        {
            try
            {
                if (string.IsNullOrEmpty(user.UserName) || string.IsNullOrEmpty(user.Password))
                {
                    return GenActionResultGenericEx("", ApiResultCode.Failed, "Username or password cannot be empty!");
                }
                if (!Regex.IsMatch(user.UserName, @"^[a-zA-Z0-9_]+$"))
                {
                    return GenActionResultGenericEx("", ApiResultCode.Failed, "Username cannot contain special characters!");
                }
                if (user.Password.Length < 8)
                {
                    return GenActionResultGenericEx("", ApiResultCode.Failed, "Password cannot be less than eight characters!");
                }
                user.Id = Guid.NewGuid().ToString(); // Generate Guid
                user.Password = MD5Encrypt.Encrypt(user.Password); // Encrypt password
                user.RoleName = "visitor"; // Default user role is visitor
                UserInfo model = SqlDapperHelper.Query("1", $"select * from sys_UserInfo where \"UserName\" =@UserName and \"Status\"=1 ", user);
                if (model != null)
                {
                    return GenActionResultGenericEx("", ApiResultCode.Failed, "This username already exists!");
                }
                string sql = "insert into sys_userinfo(\"Id\",\"UserName\",\"Password\",\"CreateTime\",\"Status\",\"RoleName\") VALUES(@Id,@UserName,@Password,@CreateTime,@Status,@RoleName)";
                int isadd = SqlDapperHelper.Add("1", sql, user);
                if (isadd > 0)
                {
                    return GenActionResultGenericEx("", ApiResultCode.Success, "Registration successful!");
                }
                else
                {
                    return GenActionResultGenericEx("", ApiResultCode.Success, "Registration failed!");
                }
            }
            catch (Exception ex)
            {
                return GenActionResultGenericEx("", ApiResultCode.Success, "User registration error, error message: " + ex.Message);
            }
        }

        /// <summary>
        /// Query all user information, can be used as a dropdown list
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public ApiResultList GetUserList([FromBody] UserInfo? user)
        {
            try
            {
                string sql = "select * from sys_UserInfo  where \"Status\"=1";

                if (user != null && !string.IsNullOrEmpty(user.UserName))
                {
                    user.UserName = string.Format("%{0}%", user.UserName);
                    sql += " and \"UserName\" like  @UserName ";
                }
                if (user != null && !string.IsNullOrEmpty(user.RoleName))
                {
                    sql += " and \"RoleName\" = @RoleName";
                }
                List<object> UserInfo = SqlDapperHelper.QueryList<object>("1", sql, user);
                return GenActionResultGenericEx(UserInfo, ApiResultCode.Success, "Query successful!");
            }
            catch (Exception ex)
            {
                return GenActionResultGenericEx(null, ApiResultCode.Failed, "Error querying user information: " + ex.Message);
            }

        }

        /// <summary>
        /// Update user information, can update username, user password
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        [HttpPost]
        public ApiResult<string> UpdateUserInfo(UserInfo user)
        {
            try
            {
                string sql = "update sys_UserInfo set ";

                if (!string.IsNullOrEmpty(user.UserName))
                {
                    sql += " \"UserName\"=@UserName,";
                }
                if (!string.IsNullOrEmpty(user.Password))
                {
                    user.Password = MD5Encrypt.Encrypt(user.Password); // Encrypt password
                    sql += " \"Password\"=@Password,";
                }
                sql += " \"Status\"=@Status where  \"Id\"= @Id ";
                int isUpdate = SqlDapperHelper.Delete("1", sql, user);
                if (isUpdate > 0)
                {
                    return GenActionResultGenericEx("", ApiResultCode.Success, "Operation successful!");
                }
                return GenActionResultGenericEx("", ApiResultCode.Failed, "Operation failed!");
            }
            catch (Exception ex)
            {
                return GenActionResultGenericEx("", ApiResultCode.Success, "Error updating user information, error message: " + ex.Message);
            }
        }

        /// <summary>
        /// Update user role, can update username, user role
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        [HttpPost]
        public ApiResult<string> UpdateUserRole(UserInfo user)
        {
            try
            {
                string sql = "update sys_UserInfo set ";

                if (!string.IsNullOrEmpty(user.UserName))
                {
                    sql += " \"RoleName\"=@RoleName,";
                }
                sql += " \"Status\"=@Status where  \"Id\"= @Id ";
                int isUpdate = SqlDapperHelper.Delete("1", sql, user);
                if (isUpdate > 0)
                {
                    return GenActionResultGenericEx("", ApiResultCode.Success, "Operation successful!");
                }
                return GenActionResultGenericEx("", ApiResultCode.Failed, "Operation failed!");
            }
            catch (Exception ex)
            {
                return GenActionResultGenericEx("", ApiResultCode.Success, "Error updating user information, error message: " + ex.Message);
            }
        }

        /// <summary>
        /// Add or update user file permissions
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        [HttpPost]
        public ApiResult<string> AddUserFilePermission(UserInfo user)
        {
            try
            {
                if (string.IsNullOrEmpty(user.Id) || string.IsNullOrEmpty(user.Id))
                {
                    return GenActionResultGenericEx("", ApiResultCode.Failed, "Please select the user to be authorized!");
                }
                string sql = "update sys_UserInfo set ";
                if (!string.IsNullOrEmpty(user.permission_repositoryID))
                {
                    sql += " \"permission_repositoryID\"=@permission_repositoryID,";
                }
                if (!string.IsNullOrEmpty(user.permission_repository_createID))
                {
                    sql += " \"permission_repository_createID\"=@permission_repository_createID,";
                }
                if (!string.IsNullOrEmpty(user.permission_repository_readID))
                {
                    sql += " \"permission_repository_readID\"=@permission_repository_readID,";
                }
                if (!string.IsNullOrEmpty(user.permission_resource_readID))
                {
                    sql += " \"permission_resource_readID\"=@permission_resource_readID,";
                }
                if (!string.IsNullOrEmpty(user.permission_resource_downloadID))
                {
                    sql += " \"permission_resource_downloadID\"=@permission_resource_downloadID,";
                }
                sql += " \"Status\"=1 where  \"Id\"= @Id ";
                // Clear old file permissions
                int isUpdate = SqlDapperHelper.Delete("1", sql, user);
                if (isUpdate > 0)
                {
                    return GenActionResultGenericEx("", ApiResultCode.Success, "Operation successful!");
                }
                return GenActionResultGenericEx("", ApiResultCode.Failed, "Operation failed!");
            }
            catch (Exception ex)
            {
                return GenActionResultGenericEx("", ApiResultCode.Success, "Error configuring file permissions, error message: " + ex.Message);
            }
        }

        /// <summary>
        /// Query user's existing file permissions
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("{UserId}")]
        public ApiResultList GetUserFile(string UserId)
        {
            try
            {
                List<object> UserFile = SqlDapperHelper.QueryList<object>
                    ("1", $"select \"Id\",\"permission_repositoryID\",\"permission_repository_createID\",\"permission_repository_readID\",\"permission_resource_readID\",\"permission_resource_downloadID\" from sys_UserInfo " +
                    $" where  \"Id\"='" + UserId + "' and \"Status\"=1 ", null);
                return GenActionResultGenericEx(UserFile, ApiResultCode.Success, "Query user file permissions successful!");
            }
            catch (Exception ex)
            {
                return GenActionResultGenericEx(null, ApiResultCode.Failed, "Error querying user file permissions: " + ex.Message);
            }

        }

    }
}
