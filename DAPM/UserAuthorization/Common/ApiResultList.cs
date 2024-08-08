namespace UserAuthorization.Common
{
    public class ApiResultList
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
        public dynamic Data { get; set; }

    }
}
