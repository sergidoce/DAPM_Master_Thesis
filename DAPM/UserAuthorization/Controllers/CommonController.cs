using Microsoft.AspNetCore.Mvc;
using UserAuthorization.Common;

namespace UserAuthorization.Controllers
{

    public class CommonController : ControllerBase
    {
        protected ApiResult<T> GenActionResultGenericEx<T>(T datas, ApiResultCode code = ApiResultCode.Success, string message = "")
        {

            var result = new ApiResult<T>
            {
                Code = code,
                Message = message,
                Data = datas
            };
            return result;
        }


        protected ApiResultList GenActionResultGenericEx(List<object> datas, ApiResultCode code = ApiResultCode.Success, string message = "")
        {
            var result = new ApiResultList();
            if (datas != null && datas.Count > 0)
            {
                result = new ApiResultList
                {
                    Code = code,
                    Message = message,
                    Data = datas
                };
            }
            else
            {
                result = new ApiResultList
                {
                    Code = code,
                    Message = message,
                    Data = ""
                };
            }
            return result;
        }
    }
}
