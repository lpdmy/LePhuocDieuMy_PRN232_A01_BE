using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.Model
{
    public class AccountCreateRequest
    {
 public AccountCreateRequest(string accountName, string accountEmail, string accountPassword, int accountRole)
        {
            AccountName = accountName;
            AccountEmail = accountEmail;
            AccountPassword = accountPassword;
            AccountRole = accountRole;
        }

        public string AccountName { get; set; }
        public string AccountEmail { get; set; }
        public string AccountPassword { get; set; }
        public int AccountRole { get; set; }
    }
}
