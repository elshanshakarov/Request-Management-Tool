using Business.Abstract;
using Business.Constants;
using Business.ValidationRules;
using Core.Utilities.Results;
using DataAccess.Abstract;
using Entities.Concrete;
using Entities.Enums;

namespace Business.Concrete
{
    public class StatusManager : IStatusService
    {
        private readonly IRequestDal _requestDal;
        private readonly IHistoryDal _historyDal;

        public StatusManager(IRequestDal requestDal, IHistoryDal historyDal)
        {
            _requestDal = requestDal;
            _historyDal = historyDal;
        }

        public IResult UpdateStatus(int requestId, int userId, short? statusId)
        {
            Request? request = _requestDal.GetRequestById(userId, requestId).SingleOrDefault();
            if (request == null)
            {
                return new ErrorResult(Messages.RequestNotFound);
            }

            bool checkStatus = StatusValidator.Validate(request, userId, statusId);

            if (checkStatus)
            {
                if (statusId == ((short)AvailableStatus.Lock) || statusId == ((short)AvailableStatus.Reject))
                {
                    History history = new History() { UserId = userId, RequestId = requestId, Date = DateTime.Now }; ;
                    if (statusId == ((short)AvailableStatus.Lock) && request.StatusId == ((short)AvailableStatus.Wait))
                    {
                        request.StatusId = (short)statusId;
                        _requestDal.Update(request);
                        history.Message = "Sorğunu gözləmədən çıxardı";
                    }
                    else
                    {
                        request.StatusId = (short)statusId;
                        request.ExecutorId = userId;
                        _requestDal.Update(request);
                        history.Message = statusId == ((short)AvailableStatus.Lock) ? Messages.LockMessage : statusId == ((short)AvailableStatus.Reject) ? Messages.RejectMessage : "";
                    }
                    _historyDal.Add(history);
                    return new SuccessResult(Messages.StatusSuccess);
                }
                else if (statusId == ((short)AvailableStatus.Close) || statusId == ((short)AvailableStatus.Wait) || statusId == ((short)AvailableStatus.Confirm))
                {
                    request.StatusId = (short)statusId;
                    _requestDal.Update(request);
                    History history = new History() { UserId = userId, RequestId = requestId, Message = statusId == ((short)AvailableStatus.Close) ? Messages.CloseMessage : statusId == ((short)AvailableStatus.Wait) ? Messages.WaitMessage : statusId == ((short)AvailableStatus.Confirm) ? Messages.ConfirmMessage : "", Date = DateTime.Now };
                    _historyDal.Add(history);
                    return new SuccessResult(Messages.StatusSuccess);
                }
            }
            return new ErrorResult(Messages.StatusError);
        }
    }
}
