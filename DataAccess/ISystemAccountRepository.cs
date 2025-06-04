using FUNewsApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess
{
    public interface ISystemAccountRepository : IRepository<SystemAccount>
    {
        SystemAccount? GetByEmail(string email);
    }
}
