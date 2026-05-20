using Microsoft.EntityFrameworkCore;
using storage_tracker.Data;
using storage_tracker.Models;

using System.Collections.Generic;
using System.Threading.Tasks;

namespace storage_tracker.Services
{
 
    public class ItemRepository : BaseRepository<Item>
    {
        public ItemRepository(ApplicationDbContext context) : base(context)
        {
        }

        public async Task<Item?> GetItemWithDetailsAsync(Guid id)
        {
            return await _dbSet
                .Include(i => i.Box)
                .Include(i => i.Category)
                .FirstOrDefaultAsync(i => i.Id == id);
        }

        public async Task<IEnumerable<Item>> GetItemsByBoxAsync(Guid boxId)
        {
            return await _dbSet
                .Include(i => i.Category)
                .Where(i => i.BoxId == boxId)
                .ToListAsync();
        }

        public async Task<IEnumerable<Item>> GetItemsByCategoryAsync(Guid categoryId)
        {
            return await _dbSet
                .Include(i => i.Box)
                .Where(i => i.CategoryId == categoryId)
                .ToListAsync();
        }

        public override async Task<IEnumerable<Item>> GetAllAsync()
        {
            return await _dbSet
                .Include(i => i.Category)
                .Include(i => i.Box)
                .ToListAsync();
        }
    }
}
