using Reddit.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Reddit.Core.Interfaces
{
    public interface INewsService
    {
        public Task<int> Get(string token);

    }
}
