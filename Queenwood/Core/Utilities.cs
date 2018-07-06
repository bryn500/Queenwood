using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Queenwood.Core
{
    public static class Utilities
    {
        public static string GetInlineFile(string path)
        {
            return File.ReadAllText($"wwwroot/{path.TrimStart('/')}");
        }
    }
}
