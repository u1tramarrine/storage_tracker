using Microsoft.EntityFrameworkCore;
using storage_tracker.Data;
using storage_tracker.Models;
namespace storage_tracker.Services
{
    public class BoxRepository : BaseRepository<Box>
    {
        public BoxRepository(ApplicationDbContext context) : base(context)
        {
        }

        public async Task<Box?> GetBoxWithItemsAsync(Guid id)
        {
            return await _dbSet
                .Include(b => b.Items)
                .Include(b => b.Category)
                .FirstOrDefaultAsync(b => b.Id == id);
        }

        public async Task<IEnumerable<Box>> GetBoxesByCategoryAsync(Guid categoryId)
        {
            return await _dbSet
                .Where(b => b.CategoryId == categoryId)
                .ToListAsync();
        }
    }
}
