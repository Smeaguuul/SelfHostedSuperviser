using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Input;
using SelfHostedSuperViser.Commands;
using SelfHostedSuperViser.Model;
using SelfHostedSuperViser.View;

namespace SelfHostedSuperViser.ViewModel
{
    public class WebsiteOverviewViewModel : INotifyPropertyChanged
    {
        public ObservableCollection<ServiceData> _websiteAPIData;
        public ObservableCollection<ServiceData> WebsiteAPIData
        {
            get => _websiteAPIData;
            set
            {
                _websiteAPIData = value;
                OnPropertyChanged(nameof(WebsiteAPIData)); // Notify the UI that the property has changed
            }
        }
        public ObservableCollection<string> Websites { get; set; }
        public ICommand RefreshWebsiteData { get; }
        public ICommand ShowWindowCommand { get; }

        private readonly SynchronizationContext _syncContext;
        public WebsiteOverviewViewModel()
        {
            WebsiteAPIData = [];
            Websites = ["NginxProxyManager", "AdguardHome", "Immich", "Traefik"];
            RefreshWebsiteData = new RelayCommand((obj) => UpdateData(), (obj) => true);
            ShowWindowCommand = new RelayCommand(ShowWindow, CanShowWindow);
            _syncContext = SynchronizationContext.Current;

            ServiceManager.AddService("NginxProxyManager");
            ServiceManager.AddService("AdguardHome");
            ServiceManager.AddService("Immich");
            ServiceManager.AddService("Traefik");
        }

        private void ShowWindow(object obj)
        {
            AddService ada = new AddService();
            ada.Show();
        }

        private bool CanShowWindow(object obj)
        {
            return true;
        }

        public async Task UpdateData()
        {
            ObservableCollection<ServiceData> TempData = [];
            foreach (var service in ServiceManager.GetServices())
            {
                List<APIValue> result = await service.CallAPIAsync();

                TempData.Add(new ServiceData(){ APIValues = result, WebsiteName = service.WebsiteName });
            }
            _syncContext.Post(_ =>
            {
                WebsiteAPIData = TempData;
            }, null);
        }


        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

    }
}
