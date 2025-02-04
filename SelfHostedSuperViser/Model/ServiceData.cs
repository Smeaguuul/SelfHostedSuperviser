using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SelfHostedSuperViser.Model
{
    public class ServiceData
    {
        public required List<APIValue> APIValues { get; set; }
        public required string WebsiteName { get; set; }
    }
}
