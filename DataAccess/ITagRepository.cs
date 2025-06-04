using FUNewsApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess
{
    public interface ITagRepository : IRepository<Tag>
    {
        Tag? GetByName(string tagName);
    }
}
