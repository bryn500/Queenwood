using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Queenwood.Core
{
    public static class Utilities
    {
        private static Regex ExtraSpaces = new Regex(@"\s+", RegexOptions.Compiled);
        
        public static string ReduceMultipleSpaces(string text)
        {
            return ExtraSpaces.Replace(text, " ");
        }

        public static string GetInlineFile(string path)
        {
            return File.ReadAllText($"wwwroot/{path.TrimStart('/')}");
        }
    }
}
