using FUNewsApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess
{
    public interface INewsArticleRepository : IRepository<NewsArticle>
    {
        IEnumerable<NewsArticle> GetAllWithDetails();
        NewsArticle? GetByIdWithDetails(int id);
    }
}
