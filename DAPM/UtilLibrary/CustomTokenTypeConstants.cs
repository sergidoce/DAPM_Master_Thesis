using System.IdentityModel.Tokens.Jwt;
namespace UtilLibrary
{
    //to make sure the token generator and token users can reference the same claims
    public class CustomTokenTypeConstants
    {
        public const string OrganisationName = "Organisation";
        public const string OrganisationId = "OrganisationId";
        public const string UserName = JwtRegisteredClaimNames.Name;
        public const string Id = JwtRegisteredClaimNames.NameId;
        //Add more as needed
    }
}
