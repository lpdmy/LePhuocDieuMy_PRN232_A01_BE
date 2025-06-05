using DataAccess;
using FUNewsApp.Models;
using LePhuocDieuMy_PRN232_A01_BE.DTOs;
using Microsoft.AspNetCore.Authorization;
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
        private readonly INewsTagsRepository _repoNewsTags;

        public NewsArticlesController(INewsArticleRepository repo, INewsTagsRepository repoNewsTags)
        {
            _repo = repo;
            _repoNewsTags = repoNewsTags;
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var news = _repo.GetByIdWithDetails(id);
            var dto = new NewsArticleDTO
            {
                AccountId = news.CreatedById,
                NewsTitle = news.NewsTitle,
                Headline = news.Headline,
                NewsContent = news.NewsContent,
                CategoryId = news.CategoryId,
                NewsStatus = news.NewsStatus,
                CreatedDate = DateTime.UtcNow,
                TagIds = news.NewsTags.Select(t => t.TagId).ToList()
            };
            return Ok(dto);
        }

        [HttpGet]
        public async Task<IActionResult> GetAll(int id)
        {
            return Ok(_repo.GetAll());
        }

        [HttpPost]
        public async Task<IActionResult> Create(NewsArticleDTO dto)
        {
            var article = new NewsArticle
            {
                NewsTitle = dto.NewsTitle,
                Headline = dto.Headline,
                NewsContent = dto.NewsContent,
                CategoryId = dto.CategoryId,
                NewsStatus = dto.NewsStatus,
                CreatedDate = DateTime.UtcNow,
                CreatedById = dto.AccountId
            };

            _repo.Add(article);
            _repo.Save(); // Sau dòng này EF sẽ cập nhật article.NewsArticleId nếu bạn đang dùng DbContext

            // Kiểm tra xem ID đã được gán chưa
            if (article.NewsArticleId == 0)
            {
                return StatusCode(500, "Failed to generate NewsArticleId.");
            }

            // Tạo list NewsTags với ID vừa được tạo
            var newsTags = dto.TagIds.Select(tagId => new NewsTag
            {
                TagId = tagId,
                NewsArticleId = article.NewsArticleId
            }).ToList();

            foreach (var newsTag in newsTags)
            {
                _repoNewsTags.Add(newsTag);
            }

            _repoNewsTags.Save();

            return Ok(article);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, NewsArticleDTO dto)
        {
            var article = _repo.GetById(id);

            if (article == null) return NotFound();

            article.NewsTitle = dto.NewsTitle;
            article.Headline = dto.Headline;
            article.NewsContent = dto.NewsContent;
            article.CategoryId = dto.CategoryId;
            article.NewsStatus = dto.NewsStatus;
            article.ModifiedDate = DateTime.UtcNow;
            article.CreatedById = dto.AccountId;

            // Cập nhật tags
            article.NewsTags.Clear();
            foreach (var tagId in dto.TagIds)
            {
                var newTag = new NewsTag { TagId = tagId, NewsArticleId = id };
                _repoNewsTags.Add(newTag);
                _repoNewsTags.Save();
                article.NewsTags.Add(newTag);

            }
            _repo.Update(article);
            _repo.Save();
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
            _repo.Save();
            return NoContent();
        }
    }
}
