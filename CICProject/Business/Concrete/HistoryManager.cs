using AutoMapper;
using Business.Abstract;
using Core.Utilities.Results;
using DataAccess.Abstract;
using Entities.Concrete;
using Entities.Dto;
using Microsoft.EntityFrameworkCore;

namespace Business.Concrete
{
    public class HistoryManager : IHistoryService
    {
        private readonly IHistoryDal _historyDal;
        private readonly IMapper _mapper;

        public HistoryManager(IHistoryDal historyDal, IMapper mapper)
        {
            _historyDal = historyDal;
            _mapper = mapper;
        }
        public IDataResult<List<RequestHistoryDto>> GetHistoryOfRequestById(int requestId)
        {
            List<History> historyList = _historyDal.GetAll(p => p.RequestId == requestId).Include(p => p.User).ToList();
            List<RequestHistoryDto> requestHistoryDtos = _mapper.Map<List<RequestHistoryDto>>(historyList);

            return new SuccessDataResult<List<RequestHistoryDto>>(requestHistoryDtos);
        }
    }
}
