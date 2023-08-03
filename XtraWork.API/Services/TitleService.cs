using XtraWork.API.Entities;
using XtraWork.API.Repositories;
using XtraWork.API.Requests;
using XtraWork.API.Responses;

namespace XtraWork.API.Services
{
    public class TitleService : ITitleService
    {
        private readonly ITitleRepository _titleRepository;

        public TitleService(ITitleRepository titleRepository)
        {
            _titleRepository = titleRepository;
        }

        public async Task<TitleResponse> Get(Guid id, CancellationToken cancellationToken)
        {
            var title = await _titleRepository.Get(id, cancellationToken);

            if (title == null)
                return null;

            var response = new TitleResponse
            {
                Id = title.Id,
                Description = title.Description
            };

            return response;
        }

        public async Task<List<TitleResponse>> GetAll(CancellationToken cancellationToken)
        {
            var titles = await _titleRepository.GetAll(cancellationToken);

            var response = titles.Select(title => new TitleResponse
            {
                Id = title.Id,
                Description = title.Description
            }).ToList();

            return response;
        }

        public async Task<TitleResponse> Create(TitleRequest request)
        {
            var title = new Title
            {
                Description = request.Description
            };

            await _titleRepository.Create(title);

            return new TitleResponse
            {
                Id = title.Id,
                Description = title.Description
            };
        }

        public async Task<TitleResponse> Update(Guid id, TitleRequest request, CancellationToken cancellationToken)
        {
            var title = await _titleRepository.Get(id, cancellationToken);

            title.Description = request.Description;

            await _titleRepository.Update(title);

            return new TitleResponse
            {
                Id = title.Id,
                Description = title.Description
            };
        }

        public async Task Delete(Guid id)
        {
            await _titleRepository.Delete(id);
        }
    }
}