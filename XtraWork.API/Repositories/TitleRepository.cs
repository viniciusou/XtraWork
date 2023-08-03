using XtraWork.API.Entities;

namespace XtraWork.API.Repositories
{
    public class TitleRepository : BaseRepository<Title>, ITitleRepository
    {
        public TitleRepository(XtraWorkContext context) : base(context) { }
    }
}