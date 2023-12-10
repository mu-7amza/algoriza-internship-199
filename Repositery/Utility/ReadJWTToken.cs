using System.IdentityModel.Tokens.Jwt;

namespace Infrastructure.Utility
{
    public class ReadJWTToken
    {
        public static string ExtractPayload(string jwtToken)
        {

            var jwtHandler = new JwtSecurityTokenHandler();
            var jwt = jwtHandler.ReadJwtToken(jwtToken);
            return jwt.Payload.SerializeToJson();
        }
    }
}
