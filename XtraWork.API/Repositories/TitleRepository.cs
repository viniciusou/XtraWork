using Microsoft.EntityFrameworkCore;
using XtraWork.API.Entities;

namespace XtraWork.API.Repositories
{
    public class TitleRepository
    {
        private readonly XtraWorkContext _context;

        public TitleRepository(XtraWorkContext context)
        {
            _context = context;
        }

        public async Task<List<Title>> GetAll()
        {
            return await _context.Titles.ToListAsync();
        }

        public async Task<Title> Get(Guid id)
        {
            return await _context.Titles.FindAsync(id);
        }

        public async Task<Title> Create(Title title)
        {
            title.Id = Guid.NewGuid();
            _context.Add(title);
            await _context.SaveChangesAsync();
            return title;
        }

        public async Task<Title> Update (Title title)
        {
            _context.Update(title);
            await _context.SaveChangesAsync();
            return title;
        }

        public async Task Delete(Guid id)
        {
            var title = await _context.Titles.FindAsync(id);
            _context.Remove(title);
            await _context.SaveChangesAsync();
        }
    }
}