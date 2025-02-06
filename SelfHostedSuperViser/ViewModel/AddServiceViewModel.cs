using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using SelfHostedSuperViser.Commands;
using SelfHostedSuperViser.Model;

namespace SelfHostedSuperViser.ViewModel
{
    public class AddServiceViewModel
    {
        public ICommand AddServiceCommand { get; set; }
        public string? WebsiteName { get; set; }
        public string? BaseUrl { get; set; }
        public string? Endpoint { get; set; }
        public string? Authorization { get; set; }
        public string? Protocol { get; set; }        
        public string? Password { get; set; }
        public string? User { get; set; }
        public string? APIKey { get; set; }
        public List<APIValue>? APIValues { get; set; }

        public AddServiceViewModel()
        {
            AddServiceCommand = new RelayCommand(AddService, CanAddUser);
        }

        private void AddService(object obj)
        {
            Authorization = "basic";
            ServiceManager.AddService(WebsiteName);
            ServiceManager.AddServiceJson(WebsiteName, BaseUrl, Endpoint, Authorization, Protocol, Password, User, APIKey, APIValues);
        }

        private bool CanAddUser(object obj)
        {
            return true;
        }
    }
}
