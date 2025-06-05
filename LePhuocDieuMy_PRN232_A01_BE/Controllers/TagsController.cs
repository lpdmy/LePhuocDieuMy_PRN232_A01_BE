using DataAccess;
using FUNewsApp.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace LePhuocDieuMy_PRN232_A01_BE.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TagController : ControllerBase
    {
        private readonly ITagRepository _repository;

        public TagController(ITagRepository repository)
        {
            _repository = repository;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Tag>>> GetAll()
        {
            var tags = _repository.GetAll();
            return Ok(tags);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Tag>> Get(int id)
        {
            var tag = _repository.GetById(id);
            if (tag == null)
                return NotFound();
            return Ok(tag);
        }

        [HttpPost]
        public async Task<ActionResult<Tag>> Create(Tag tag)
        {
            _repository.Add(tag);
            _repository.Save();
            return CreatedAtAction(nameof(Get), new { id = tag.TagId }, tag);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, Tag tag)
        {
            if (id != tag.TagId)
                return BadRequest();

             _repository.Update(tag);
            _repository.Save();

            return NoContent();
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var category = _repository.GetById(id);
            if (category == null) return NotFound();
            _repository.Delete(category);
            _repository.Save();

            return NoContent();
        }
    }
}
