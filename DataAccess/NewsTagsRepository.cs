using FUNewsApp.Data;
using FUNewsApp.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess
{
    public class NewsTagsRepository : Repository<NewsTag>, INewsTagsRepository
    {
        public NewsTagsRepository(FUNewsContext context) : base(context)
        {
        }

        public void DeleteTags(int newsId)
        {
           _dbSet.RemoveRange(_dbSet.Where(t => t.NewsArticleId == newsId));
        }
    }
}
