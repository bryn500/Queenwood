using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Queenwood.Models
{
    public class Result
    {
        public string ErrorMessage { get; set; }
        public bool IsError { get; set; }
    }

    public class Result<T> : Result
    {
        public T Item { get; set; }
    }
}
