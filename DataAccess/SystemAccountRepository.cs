using FUNewsApp.Data;
using FUNewsApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess
{
    public class SystemAccountRepository : Repository<SystemAccount>, ISystemAccountRepository
    {
        private readonly FUNewsContext _context;

        public SystemAccountRepository(FUNewsContext context) : base(context)
        {
            _context = context;
        }

        public SystemAccount? GetByEmail(string email)
        {
            return _context.SystemAccounts.FirstOrDefault(a => a.AccountEmail == email);
        }
    }
}
