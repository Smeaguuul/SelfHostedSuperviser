using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SelfHostedSuperViser.ViewModel;

namespace SelfHostedSuperViser.Commands
{
    internal class UpdateWebsiteInfoCommand : CommandBase
    {
        public required WebsiteInfoGetter WebsiteInfoGetter { get; set; }
        public override void Execute(object? parameter)
        {
            WebsiteInfoGetter.UpdateData();
        }

    }
}
