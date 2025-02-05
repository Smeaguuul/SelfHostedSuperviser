using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SelfHostedSuperViser.Model
{
    public class APIValue
    {
        public required List<string> Names { get; set; }
        public string Value { get; set; }
        public required string DisplayName { get; set; }
    }
}
