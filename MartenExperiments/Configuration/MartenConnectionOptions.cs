using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MartenExperiments.Configuration
{
    public class MartenConnectionOptions
    {
        public string Host { get; set; }
        public string Database { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }

        public string ToConnectionString() =>
            $"host={Host};database={Database};password={Password};username={Username}";
    }
}
