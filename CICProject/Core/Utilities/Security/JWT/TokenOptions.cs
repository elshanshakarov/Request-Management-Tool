namespace Core.Utilities.Security.JWT
{
    public class TokenOptions
    {
        public string Audience { get; set; }
        public string Issuer { get; set; }
        public int AccessTokenExpiration { get; set; }
        public string AccessSecurityKey { get; set; }
        public int ForgetTokenExpiration { get; set; }
        public string ForgetSecurityKey { get; set; }
    }
}
