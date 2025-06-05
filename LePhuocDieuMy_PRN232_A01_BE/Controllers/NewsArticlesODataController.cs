using DataAccess;
using FUNewsApp.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Query;
using Microsoft.AspNetCore.OData.Routing.Controllers;

namespace LePhuocDieuMy_PRN232_A01_BE.Controllers
{
    [Route("odata/NewsArticles")]
    [ApiController]
    public class NewsArticlesODataController : ODataController
    {
        private readonly INewsArticleRepository _repo;

        public NewsArticlesODataController(INewsArticleRepository repo)
        {
            _repo = repo;
        }

        [EnableQuery]
        [HttpGet]
        public IQueryable<NewsArticle> GetAll()
        {
            return _repo.GetAll().AsQueryable();
        }

    }
}
