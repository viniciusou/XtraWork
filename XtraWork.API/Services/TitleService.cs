using AutoMapper;
using XtraWork.API.Entities;
using XtraWork.API.Repositories;
using XtraWork.API.Requests;
using XtraWork.API.Responses;

namespace XtraWork.API.Services
{
    public class TitleService : ITitleService
    {
        private readonly ITitleRepository _titleRepository;
        private readonly IMapper _mapper;

        public TitleService(ITitleRepository titleRepository, IMapper mapper)
        {
            _titleRepository = titleRepository;
            _mapper = mapper;
        }

        public async Task<TitleResponse> Get(Guid id, CancellationToken cancellationToken)
        {
            var title = await _titleRepository.Get(id, cancellationToken);

            if (title == null)
                return null;

            var response = _mapper.Map<TitleResponse>(title);

            return response;
        }

        public async Task<List<TitleResponse>> GetAll(CancellationToken cancellationToken)
        {
            var titles = await _titleRepository.GetAll(cancellationToken);

            var response = _mapper.Map<List<TitleResponse>>(titles);
            
            return response;
        }

        public async Task<TitleResponse> Create(TitleRequest request)
        {
            var title = _mapper.Map<Title>(request);

            var createdTitle = await _titleRepository.Create(title);

            var response = _mapper.Map<TitleResponse>(createdTitle);
            
            return response;
        }

        public async Task<TitleResponse> Update(Guid id, TitleRequest request, CancellationToken cancellationToken)
        {
            var title = await _titleRepository.Get(id, cancellationToken);

            title.Description = request.Description;

            var updatedTitle = await _titleRepository.Update(title);

            var response = _mapper.Map<TitleResponse>(updatedTitle);

            return response;
        }

        public async Task Delete(Guid id)
        {
            await _titleRepository.Delete(id);
        }
    }
}