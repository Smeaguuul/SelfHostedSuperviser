using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SelfHostedSuperViser.Model.APIGetter.APIGetter;

namespace SelfHostedSuperViser.Model.APIGetter
{
    public interface WebsiteAPIModel
    {
        public abstract List<APIValue> CallAPI();
    }
}
