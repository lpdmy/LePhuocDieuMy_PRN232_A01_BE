using DataAccess;
using Entities.Model;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.Validator
{
    public class AccountCreateRequestValidator : AbstractValidator<AccountCreateRequest>
    {
        public AccountCreateRequestValidator(ISystemAccountRepository accountRepo)
        {
            RuleFor(x => x.AccountName)
                .NotEmpty().WithMessage("Account name is required.");

            RuleFor(x => x.AccountEmail)
                .NotEmpty().WithMessage("Email is required.")
                .EmailAddress().WithMessage("Invalid email format.")
                .Must(NotBeAdminEmail).WithMessage("This email is reserved for system administrator.")
                .Must(email => !accountRepo.GetAll().Any(a => a.AccountEmail == email))
                    .WithMessage("Email already exists.");

            RuleFor(x => x.AccountPassword)
                .NotEmpty().WithMessage("Password is required.")
                .MinimumLength(6).WithMessage("Password must be at least 6 characters.");

            RuleFor(x => x.AccountRole)
                .Must(role => role == 1 || role == 2)
                .WithMessage("Account role must be 1 (Staff) or 2 (Lecturer).");
        }

        private bool NotBeAdminEmail(string email)
        {
            return !string.Equals(email, "admin@FUNewsManagementSystem.org", StringComparison.OrdinalIgnoreCase);
        }
    }

}
