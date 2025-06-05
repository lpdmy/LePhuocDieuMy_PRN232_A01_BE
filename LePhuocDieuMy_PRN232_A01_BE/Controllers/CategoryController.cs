using DataAccess;
using DataAccess.Model;
using FUNewsApp.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Routing.Controllers;

namespace LePhuocDieuMy_PRN232_A01_BE.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "Staff")]
    public class CategoryController : ODataController
    {
        private readonly ICategoryRepository _categoryRepo;
        private readonly INewsArticleRepository _newsRepo;

        public CategoryController(ICategoryRepository categoryRepo, INewsArticleRepository newsRepo)
        {
            _categoryRepo = categoryRepo;
            _newsRepo = newsRepo;
        }

        [HttpGet]
        public IActionResult GetAll()
        {
            /* var categories = _categoryRepo.GetAll()
                 .Select(c => new CategoryDto
                 {
                     CategoryId = c.CategoryId,
                     CategoryName = c.CategoryName
                 });*/
            var categories = _categoryRepo.GetAll();

            return Ok(categories);
        }

        [HttpGet("{id}")]
        public IActionResult GetById(int id)
        {
            /* var categories = _categoryRepo.GetAll()
                 .Select(c => new CategoryDto
                 {
                     CategoryId = c.CategoryId,
                     CategoryName = c.CategoryName
                 });*/
            var category = _categoryRepo.GetById(id);

            return Ok(category);
        }


        [HttpPost]
        public IActionResult Create(Category category)
        {
            _categoryRepo.Add(category);
            _categoryRepo.Save();

            return CreatedAtAction(nameof(GetAll), new { id = category.CategoryId }, category);
        }

        [HttpPut("{id}")]
        public IActionResult Update(int id, Category updated)
        {
            var category = _categoryRepo.GetById(id);
            if (category == null) return NotFound();
            category.CategoryName = updated.CategoryName;
            category.CategoryDescription = updated.CategoryDescription;
            category.IsActive = updated.IsActive;
            category.ParentCategoryId = updated.ParentCategoryId;
            _categoryRepo.Update(category);
            _categoryRepo.Save();

            return NoContent();
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var hasArticles = _newsRepo.GetAll().Any(n => n.CategoryId == id);
            if (hasArticles)
                return BadRequest("Cannot delete category. It is used in news articles.");

            var category = _categoryRepo.GetById(id);
            if (category == null) return NotFound();
            _categoryRepo.Delete(category);
            _categoryRepo.Save();

            return NoContent();
        }
    }
}

