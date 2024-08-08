namespace UserAuthorization.Common
{
    public enum ApiResultCode
    {
        // Failure
        Failed = 500,
        // Success
        Success = 200,
        // Token expired
        Expired = 300,
        // No authority
        Authority = 900
    }
}
