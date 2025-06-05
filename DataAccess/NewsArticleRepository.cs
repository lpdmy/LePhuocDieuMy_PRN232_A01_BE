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
    public class NewsArticleRepository : Repository<NewsArticle>, INewsArticleRepository
    {
        private readonly FUNewsContext _context;

        public NewsArticleRepository(FUNewsContext context) : base(context)
        {
            _context = context;
        }

        public IEnumerable<NewsArticle> GetAll()
        {
            return _context.NewsArticles.ToList(); 
        }

        public IEnumerable<NewsArticle> GetAllWithDetails()
        {
            return _context.NewsArticles
                .Include(n => n.Category)
                .Include(n => n.CreatedBy)
                .Include(n => n.UpdatedBy)
                .Include(n => n.NewsTags)
                    .ThenInclude(nt => nt.Tag)
                .ToList();
        }

        public NewsArticle? GetByIdWithDetails(int id)
        {
            var article = _context.NewsArticles
                .Include(n => n.Category)
                .Include(n => n.CreatedBy)
                .Include(n => n.UpdatedBy)
                .Include(n => n.NewsTags)
                    .ThenInclude(nt => nt.Tag)
                .FirstOrDefault(n => n.NewsArticleId == id);
            var tagList = _context.NewsTags.Where(t => t.NewsArticleId == article.NewsArticleId);
            article.NewsTags = tagList.ToList();
            return article;
        }
    }
}