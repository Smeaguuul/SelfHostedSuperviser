using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SelfHostedSuperViser.Model
{
    public class LoginRequest
    {
        public required string Type { get; set; }
        public required string Identity { get; set; }
        public required string Secret { get; set; }

    }
}
