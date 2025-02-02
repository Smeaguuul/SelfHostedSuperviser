using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using SelfHostedSuperViser.Model.APIGetter.APIGetter;
using SelfHostedSuperViser.ViewModel;

namespace SelfHostedSuperViser.View
{
    /// <summary>
    /// Interaction logic for WebsiteOverview.xaml
    /// </summary>
    public partial class WebsiteOverview : Window
    {
        private WebsiteInfoGetter _viewModel;
        public WebsiteOverview()
        {
            InitializeComponent();
            _viewModel = new WebsiteInfoGetter();
            this.DataContext = _viewModel;

            //Fire and forget, no await necessary
            LoadDataAsync();
        }

        private async void LoadDataAsync()
        {
            await _viewModel.UpdateData();

            AddWebsiteInfo();
        }

        private void AddWebsiteInfo()
        {
            if (_viewModel.WebsiteAPIData.Count != 0)
            {
                for (int i = 0; i < _viewModel.WebsiteAPIData.Count; i++)
                {
                    Websites.Children.Add(GetAPIDataElement(_viewModel.WebsiteAPIData[i]));
                    //myTextBlock.SetBinding(TextBox.TextProperty, new Binding("FirstValue"));
                }
            }
        }

        private TextBlock GetAPIDataElement (List<APIValue> aPIValues)
        {
            var myTextBlock = new TextBlock();
            myTextBlock.Foreground = new SolidColorBrush(Colors.White);
            myTextBlock.FontSize = 26;
            myTextBlock.Width = 60;
            myTextBlock.Height = 40;
            myTextBlock.HorizontalAlignment = HorizontalAlignment.Center;
            myTextBlock.VerticalAlignment = VerticalAlignment.Center;
            myTextBlock.Text = aPIValues[0].Value;
            return myTextBlock;
        }

        private void MinimizeButton_Click(object sender, RoutedEventArgs e)
        {
            this.WindowState = WindowState.Minimized;
        }

        private void ExitButton_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }
    }
}
