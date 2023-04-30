using System.Security.Claims;

namespace Core.Extensions
{
    // Bu Class vasitəsilə JWT-dən gələn bir kişinin Claim-lərin oxumağ üçün istifadə edilir. 
    // Bu prosesi ClaimsPrincipal class-ı həyata keçirir. Biz işimizi asanlaşdırmaq üçün bu class-ı Extension(genişlədirik) edirik.
    // Aşağıdakı kodda ?(sual) işarəsi null ola bilməni göstərir.
    public static class ClaimsPrincipalExtensions
    {
        public static List<string> Claims(this ClaimsPrincipal claimsPrincipal, string claimType)
        {
            var result = claimsPrincipal?.FindAll(claimType)?.Select(x => x.Value).ToList();
            return result;
        }

        public static List<string> ClaimRoles(this ClaimsPrincipal claimsPrincipal)
        {
            return claimsPrincipal?.Claims(ClaimTypes.Role);
        }
    }
}
