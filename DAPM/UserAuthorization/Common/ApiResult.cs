namespace UserAuthorization.Common
{
    public class ApiResult<T>
    {
        /// <summary>
        /// Error code
        /// </summary>
        public ApiResultCode Code { get; set; }
        /// <summary>
        /// Error message
        /// </summary>
        public string Message { get; set; }
        /// <summary>
        /// Specific data
        /// </summary>
        public T Data { get; set; }

    }
}
