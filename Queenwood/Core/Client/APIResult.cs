using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Queenwood.Core.Client
{
    public class APIResult
    {
        public string Json { get; set; }
    }

    public class APIResult<T> : APIResult
    {
        public T Data { get; set; }
    }
}
