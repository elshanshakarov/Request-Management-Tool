using Core.Utilities.Results;

namespace Business.Abstract
{
    public interface IStatusService
    {
        IResult UpdateStatus(int requestId, int userId, short? statusId);
    }
}
