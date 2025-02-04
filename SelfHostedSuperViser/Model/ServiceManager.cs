using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SelfHostedSuperViser.Model
{
    public class ServiceManager
    {
        private static ObservableCollection<Service> _Services  = new ObservableCollection<Service>();

        public static ObservableCollection<Service> GetServices()
        {
            return _Services;
        }

        public static void AddService(string serviceName)
        {
            _Services.Add(new Service() { WebsiteName = serviceName });
        }
    }
}
