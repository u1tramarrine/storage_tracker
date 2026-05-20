using Microsoft.EntityFrameworkCore;
using storage_tracker.Data;
using storage_tracker.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace storage_tracker.Services
{

    public class CategoryRepository : BaseRepository<Category>
    {
        public CategoryRepository(ApplicationDbContext context) : base(context)
        {
        }

        public async Task<bool> IsNameUniqueAsync(string name, Guid? excludeId = null)
        {
            if (excludeId.HasValue)
                return !await _dbSet.AnyAsync(c => c.Name == name && c.Id != excludeId.Value);
            return !await _dbSet.AnyAsync(c => c.Name == name);
        }

        public async Task<Category?> GetCategoryWithBoxesAsync(Guid id)
        {
            return await _dbSet
                .Include(c => c.Boxes)
                .FirstOrDefaultAsync(c => c.Id == id);
        }
    }
}
