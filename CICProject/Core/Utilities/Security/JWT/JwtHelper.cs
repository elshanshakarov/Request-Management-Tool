using Core.Extensions;
using Core.Utilities.Security.Encryption;
using Entities.Concrete;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Core.Utilities.Security.JWT
{
    public class JwtHelper : ITokenHelper
    {
        public IConfiguration Configuration { get; }
        private static TokenOptions _tokenOptions;
        private DateTime _accessTokenExpiration;


        public JwtHelper(IConfiguration configuration)
        {
            Configuration = configuration;
            _tokenOptions = Configuration.GetSection("TokenOptions").Get<TokenOptions>();
        }

        public AccessToken CreateAccessToken(User user, List<OperationClaim> operationClaims)
        {
            _accessTokenExpiration = DateTime.Now.AddMinutes(_tokenOptions.AccessTokenExpiration);

            SecurityKey securityKey = SecurityKeyHelper.CreateSecurityKey(_tokenOptions.AccessSecurityKey);
            SigningCredentials signingCredentials = SigningCredentialsHelper.CreateSigningCredentials(securityKey);
            JwtSecurityToken jwt = CreateJwtSecurityToken(_tokenOptions, user, signingCredentials, operationClaims);
            String token = new JwtSecurityTokenHandler().WriteToken(jwt);

            return new AccessToken { Token = token, Expiration = _accessTokenExpiration };
        }

        public AccessToken CreateForgetToken(User user, List<OperationClaim> operationClaims)
        {
            _accessTokenExpiration = DateTime.Now.AddMinutes(_tokenOptions.ForgetTokenExpiration);

            SecurityKey securityKey = SecurityKeyHelper.CreateSecurityKey(_tokenOptions.ForgetSecurityKey);
            SigningCredentials signingCredentials = SigningCredentialsHelper.CreateSigningCredentials(securityKey);
            JwtSecurityToken jwt = CreateJwtSecurityToken(_tokenOptions, user, signingCredentials, operationClaims);
            String token = new JwtSecurityTokenHandler().WriteToken(jwt);

            return new AccessToken { Token = token, Expiration = _accessTokenExpiration };
        }

        private JwtSecurityToken CreateJwtSecurityToken(TokenOptions tokenOptions, User user, SigningCredentials signingCredentials, List<OperationClaim> operationClaims)
        {
            JwtSecurityToken jwt = new JwtSecurityToken(
                issuer: tokenOptions.Issuer,
                audience: tokenOptions.Audience,
                expires: _accessTokenExpiration,
                notBefore: DateTime.Now, // Token-in istifadə müddəti cari zamandan əvvəldirsə keçərli deyil
                signingCredentials: signingCredentials,
                claims: SetClaims(user, operationClaims)
            );
            return jwt;
        }

        private IEnumerable<Claim> SetClaims(User user, List<OperationClaim> operationClaims)
        {
            List<Claim> claims = new List<Claim>();
            claims.AddNameIdentifier(user.Id.ToString());
            claims.AddEmail(user.Mail);
            claims.AddName($"{user.Name} {user.Surname}");
            claims.AddRoles(operationClaims.Select(c => c.Name).ToArray());

            return claims;
        }


        public static bool VerifyResetToken(string token, out string email)
        {
            var tokenHandler = new JwtSecurityTokenHandler();

            try
            {
                var validationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_tokenOptions.ForgetSecurityKey)),
                    ValidateIssuer = true,
                    ValidIssuer = "elsen@elsen.com",
                    ValidateAudience = true,
                    ValidAudience = "elsen@elsen.com",
                };

                // Validate and parse the token
                ClaimsPrincipal? claimsPrincipal = tokenHandler.ValidateToken(token, validationParameters, out SecurityToken validatedToken);
                email = claimsPrincipal.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Email).Value;
                return true;
            }
            catch (Exception)
            {
                email = null;
                return false;
            }



        }
        //public static IEnumerable<Claim> GetClaimsFromToken(string token)
        //{
        //    var tokenHandler = new JwtSecurityTokenHandler();
        //    var jwtToken = tokenHandler.ReadJwtToken(token);

        //    return jwtToken.Claims;
        //}
    }
}
