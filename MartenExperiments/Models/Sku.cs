using System.Text.RegularExpressions;
using StringlyTyped;

namespace MartenExperiments.Models
{
    public class Sku : Stringly
    {
        protected override Regex Regex => new Regex(@"[a-zA-Z_][a-zA-Z0-9\-_]*", RegexOptions.Compiled);

        public static Sku Parse(string sku)=> new Stringly<Sku>(sku);
    }
}