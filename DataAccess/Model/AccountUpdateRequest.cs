using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.Model
{
    public class AccountUpdateRequest
    {
        public AccountUpdateRequest(int accountId, string accountName, string accountEmail, string accountPassword, int accountRole)
        {
            AccountId = accountId;
            AccountName = accountName;
            AccountEmail = accountEmail;
            AccountPassword = accountPassword;
            AccountRole = accountRole;
        }

        public int AccountId { get; set; }
        public string AccountName { get; set; }
        public string AccountEmail { get; set; }
        public string AccountPassword { get; set; }
        public int AccountRole { get; set; }
    }
}
