using DataAccess;
using FUNewsApp.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Routing.Controllers;
using System.Security.Claims;

namespace LePhuocDieuMy_PRN232_A01_BE.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "Staff")]
    public class NewsArticlesController : ODataController
    {
        private readonly INewsArticleRepository _repo;

        public NewsArticlesController(INewsArticleRepository repo)
        {
            _repo = repo;
        }

        [HttpGet]
        public IActionResult GetMyArticles()
        {
            var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
            var articles = _repo.GetAll().Where(a => a.CreatedById == userId || a.UpdatedById == userId);
            return Ok(articles);
        }

        [HttpPost]
        public IActionResult Create(NewsArticle article)
        {
            article.CreatedDate = DateTime.UtcNow;
            article.CreatedById = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
            _repo.Add(article);
            return CreatedAtAction(nameof(GetMyArticles), new { id = article.NewsArticleId }, article);
        }

        [HttpPut("{id}")]
        public IActionResult Update(int id, NewsArticle updated)
        {
            var article = _repo.GetById(id);
            if (article == null) return NotFound();

            if (article.CreatedById != int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)))
                return Forbid();

            article.NewsTitle = updated.NewsTitle;
            article.NewsContent = updated.NewsContent;
            article.UpdatedById = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
            article.ModifiedDate = DateTime.UtcNow;
            _repo.Update(article);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var article = _repo.GetById(id);
            if (article == null) return NotFound();

            if (article.CreatedById != int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)))
                return Forbid();

            _repo.Delete(article);
            return NoContent();
        }
    }
}
