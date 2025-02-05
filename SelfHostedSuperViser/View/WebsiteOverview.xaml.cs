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
using SelfHostedSuperViser.Model;
using SelfHostedSuperViser.ViewModel;

namespace SelfHostedSuperViser.View
{
    /// <summary>
    /// Interaction logic for WebsiteOverview.xaml
    /// </summary>
    public partial class WebsiteOverview : Window
    {
        private WebsiteOverviewViewModel _viewModel;
        public WebsiteOverview()
        {
            InitializeComponent();
            _viewModel = new WebsiteOverviewViewModel();
            this.DataContext = _viewModel;

            //Fire and forget, no await necessary
            LoadDataAsync();
        }

        private async void LoadDataAsync()
        {
            await _viewModel.UpdateData();
        }

        private void MinimizeButton_Click(object sender, RoutedEventArgs e)
        {
            this.WindowState = WindowState.Minimized;
        }

        private void ExitButton_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

        private void Outermost_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
                this.DragMove();
        }
    }
}
