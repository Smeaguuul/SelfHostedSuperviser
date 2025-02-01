using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Input;
using SelfHostedSuperViser.Commands;
using SelfHostedSuperViser.Model.APIGetter;
using SelfHostedSuperViser.Model.APIGetter.AdguardHome;
using SelfHostedSuperViser.Model.APIGetter.APIGetter;

namespace SelfHostedSuperViser.ViewModel
{
    public class WebsiteInfoGetter : INotifyPropertyChanged
    {
        private ObservableCollection<List<APIValue>> _websiteAPIData;
        public ObservableCollection<List<APIValue>> WebsiteAPIData
        {
            get => _websiteAPIData;
            set
            {
                if (_websiteAPIData != value)
                {
                    _websiteAPIData = value;
                    OnPropertyChanged(nameof(WebsiteAPIData)); // Notify the UI that the property has changed
                }
            }
        }
        private ObservableCollection<WebsiteAPIModel> websiteAPIModels;

        public WebsiteInfoGetter()
        {
            WebsiteAPIData = [];
            websiteAPIModels = [];
            RefreshWebsiteData = new UpdateWebsiteInfoCommand { WebsiteInfoGetter = this };

            websiteAPIModels.Add(new AdguardHome());

            UpdateData();
        }

        public void UpdateData()
        {
            ObservableCollection<List<APIValue>> TempData = [];
            foreach (var website in websiteAPIModels)
            {
                TempData.Add(website.CallAPI());
            }
            WebsiteAPIData = TempData;
        }


        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public ICommand RefreshWebsiteData { get; }
    }
}
