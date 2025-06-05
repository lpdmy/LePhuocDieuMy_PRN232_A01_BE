using Entities.Model;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Validator
{
    public class AccountUpdateRequestValidator : AbstractValidator<AccountUpdateRequest>
    {
        public AccountUpdateRequestValidator(ISystemAccountRepository accountRepo)
        {
            RuleFor(x => x.AccountName)
                .NotEmpty().WithMessage("Account name is required.");

            RuleFor(x => x.AccountEmail)
                .NotEmpty().WithMessage("Email is required.")
                .EmailAddress().WithMessage("Invalid email format.")
                .Must((model, email) =>
                {
                    var existingAccount = accountRepo.GetAll()
                        .FirstOrDefault(a => a.AccountEmail == email);
                    return existingAccount == null || existingAccount.AccountId == model.AccountId;
                }).WithMessage("Email is already used by another account.");

            RuleFor(x => x.AccountPassword)
                .MinimumLength(6).When(x => !string.IsNullOrEmpty(x.AccountPassword))
                .WithMessage("Password must be at least 6 characters.");


            RuleFor(x => x.AccountRole)
                .Must(role => role == 1 || role == 2)
                .WithMessage("Account role must be 1 (Staff) or 2 (Lecturer).");
        }
    }
}
