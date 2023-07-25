using XtraWork.API.Entities;
using XtraWork.API.Repositories;
using XtraWork.API.Requests;
using XtraWork.API.Responses;

namespace XtraWork.API.Services
{
    public class TitleService
    {
        private readonly TitleRepository _titleRepository;

        public TitleService(TitleRepository titleRepository)
        {
            _titleRepository = titleRepository;
        }

        public async Task<TitleResponse> Get(Guid id)
        {
            var title = await _titleRepository.Get(id);

            var response = new TitleResponse
            {
                Id = title.Id,
                Description = title.Description
            };

            return response;
        }

        public async Task<List<TitleResponse>> GetAll()
        {
            var titles = await _titleRepository.GetAll();

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

        public async Task<TitleResponse> Update(Guid id, TitleRequest request)
        {
            var title = await _titleRepository.Get(id);

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