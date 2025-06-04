using FUNewsApp.Data;
using FUNewsApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess
{
    public class CategoryRepository : Repository<Category>, ICategoryRepository
    {
        private readonly FUNewsContext _context;

        public CategoryRepository(FUNewsContext context) : base(context)
        {
            _context = context;
        }

        public IEnumerable<Category> GetAllActive()
        {
            return _context.Categories.Where(c => c.IsActive).ToList();
        }
    }
}
