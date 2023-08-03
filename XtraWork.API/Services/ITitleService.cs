using XtraWork.API.Requests;
using XtraWork.API.Responses;

namespace XtraWork.API.Services
{
    public interface ITitleService
    {
         Task<TitleResponse> Create(TitleRequest request);
        Task Delete(Guid id);
        Task<TitleResponse> Get(Guid id, CancellationToken cancellationToken);
         Task<List<TitleResponse>> GetAll(CancellationToken cancellationToken);
         Task<TitleResponse> Update(Guid id, TitleRequest request, CancellationToken cancellationToken);
    }
}