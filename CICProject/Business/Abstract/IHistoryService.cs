using Core.Utilities.Results;
using Entities.Dto;

namespace Business.Abstract
{
    public interface IHistoryService
    {
        IDataResult<List<RequestHistoryDto>> GetHistoryOfRequestById(int requestId);
    }
}
