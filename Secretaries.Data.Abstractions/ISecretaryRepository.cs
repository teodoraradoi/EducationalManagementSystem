using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ExtCore.Data.Abstractions;
using Secretaries.Data.Entities;

namespace Secretaries.Data.Abstractions
{
    public interface ISecretaryRepository : IRepository
    {
        IEnumerable<Secretary> All();
    }
}
