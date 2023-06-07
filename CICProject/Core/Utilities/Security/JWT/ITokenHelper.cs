using Entities.Concrete;

namespace Core.Utilities.Security.JWT
{
    public interface ITokenHelper
    {
        AccessToken CreateAccessToken(User user, List<OperationClaim> operationClaims);
        AccessToken CreateForgetToken(User user, List<OperationClaim> operationClaims);
    }
}
