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
            DataContext = _viewModel;
            //AddWebsiteInfo();
        }

        private void AddWebsiteInfo()
        {
            var myTextBlock = new TextBlock { Text = _viewModel.WebsiteAPIData[0][0].Name };
            Websites.Children.Add(myTextBlock);
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
