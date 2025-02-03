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
                _websiteAPIData = value;
                OnPropertyChanged(nameof(WebsiteAPIData)); // Notify the UI that the property has changed
            }
        }
        private ObservableCollection<Expression<Func<Task<List<APIValue>>>>> websiteAPIModels;

        public WebsiteInfoGetter()
        {
            WebsiteAPIData = [];
            websiteAPIModels = [];
            RefreshWebsiteData = new UpdateWebsiteInfoCommand { WebsiteInfoGetter = this };

            websiteAPIModels.Add(() => new AdguardHome().CallAPIAsync());
            websiteAPIModels.Add(() => new Immich().CallAPIAsync());
        }

        public async Task UpdateData()
        {
            ObservableCollection<List<APIValue>> TempData = [];
            foreach (var apiCall in websiteAPIModels)
            {
                var func = apiCall.Compile();
                // Execute the delegate
                List<APIValue> result = await func();
                TempData.Add(result);
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
