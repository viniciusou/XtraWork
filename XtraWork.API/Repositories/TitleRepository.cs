using Microsoft.EntityFrameworkCore;
using XtraWork.API.Entities;

namespace XtraWork.API.Repositories
{
    public class TitleRepository : BaseRepository<Title>
    {
        public TitleRepository(XtraWorkContext context) : base(context) { }
    }
}