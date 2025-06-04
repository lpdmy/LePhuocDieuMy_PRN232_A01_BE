using FUNewsApp.Data;
using FUNewsApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess
{
    public class TagRepository : Repository<Tag>, ITagRepository
    {
        private readonly FUNewsContext _context;

        public TagRepository(FUNewsContext context) : base(context)
        {
            _context = context;
        }

        public Tag? GetByName(string tagName)
        {
            return _context.Tags.FirstOrDefault(t => t.TagName == tagName);
        }
    }
}