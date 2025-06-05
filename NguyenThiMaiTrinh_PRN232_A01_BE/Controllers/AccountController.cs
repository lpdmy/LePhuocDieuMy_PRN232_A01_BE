using Azure.Core;
using DataAccess;
using Entities.Model;
using Entities.Validator;
using FUNewsApp.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Formatter;
using Microsoft.AspNetCore.OData.Query;
using Microsoft.AspNetCore.OData.Routing.Controllers;

namespace NguyenThiMaiTrinh_PRN232_A01_BE.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "Admin")]
    public class AccountController : ODataController
    {

        private readonly ISystemAccountRepository _accountRepo;
        private readonly INewsArticleRepository _newsRepo;

        public AccountController(ISystemAccountRepository accountRepo, INewsArticleRepository newsRepo)
        {
            _accountRepo = accountRepo;
            _newsRepo = newsRepo;
        }

        [EnableQuery]
        [HttpGet]
        public IActionResult Get()
        {
            var accounts = _accountRepo.GetAll().AsQueryable();
            return Ok(accounts);
        }

        [HttpGet("{id}")]
        public IActionResult GetById(int id)
        {
            var account = _accountRepo.GetById(id);
            if (account == null)
                return NotFound();
            return Ok(account);
        }
        [HttpPost]
        public IActionResult Create([FromBody] AccountCreateRequest request)
        {
            var validator = new AccountCreateRequestValidator(_accountRepo);
            var result = validator.Validate(request);
            if (!result.IsValid)
                return BadRequest(result.Errors);


            var newAccount = new SystemAccount
            {
                AccountName = request.AccountName,
                AccountEmail = request.AccountEmail,
                AccountPassword = request.AccountPassword,
                AccountRole = request.AccountRole
            };

            _accountRepo.Add(newAccount);
            _accountRepo.Save();
            return CreatedAtAction(nameof(GetById), new { id = newAccount.AccountId }, newAccount);
        }

        [HttpPut]
        public IActionResult Update([FromBody] AccountUpdateRequest account)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var existing = _accountRepo.GetById(account.AccountId);
            if (existing == null)
                return NotFound();

            // Update properties except ID
            existing.AccountName = account.AccountName;
            existing.AccountEmail = account.AccountEmail;
            existing.AccountPassword = account.AccountPassword;
            existing.AccountRole = account.AccountRole;

            _accountRepo.Update(existing);
            _accountRepo.Save();

            return NoContent();
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var account = _accountRepo.GetById(id);
            if (account == null)
                return NotFound();

            // Check if account created any news articles
            var hasArticles = _newsRepo.GetAllWithDetails()
                 .Any(n => n.CreatedById == id || n.UpdatedById == id);

            if (hasArticles)
            {
                return BadRequest("Cannot delete account because it has created news articles.");
            }

            _accountRepo.Delete(account);
            _accountRepo.Save();

            return NoContent();
        }

    }
}
