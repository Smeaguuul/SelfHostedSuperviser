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
                var wrapPanel = new WrapPanel() { Height = 360 };
                for (int i = 0; i < _viewModel.WebsiteAPIData.Count; i++)
                {
                    wrapPanel.Children.Add(GetAPIDataElement(_viewModel.WebsiteAPIData[i]));
                    //myTextBlock.SetBinding(TextBox.TextProperty, new Binding("FirstValue"));
                }
                Websites.Children.Add(wrapPanel);
            }
        }

        private Grid GetAPIDataElement (List<APIValue> aPIValues)
        {
            var websiteGrid = new Grid()
            {
                Width = 160,
                Height = 60,
                Background = new SolidColorBrush(Color.FromRgb(126,126,126)),
                Opacity = 0.4
            };
            var row1 = new RowDefinition() { Height = new GridLength(20) };
            var row2 = new RowDefinition() { Height = new GridLength(1, GridUnitType.Star) };  

            websiteGrid.RowDefinitions.Add(row1);
            websiteGrid.RowDefinitions.Add(row2);
            
            // -1, fordi vi ikke tæller website navn med
            for (int i = 0;i < aPIValues.Count - 1;i++)
            {
                var column1 = new ColumnDefinition();
                column1.Width = new GridLength(1, GridUnitType.Star);
                websiteGrid.ColumnDefinitions.Add(column1);
            }

            var websiteTitle = new TextBlock()
            {
                Foreground = new SolidColorBrush(Color.FromRgb(255,255,255)),
                HorizontalAlignment = HorizontalAlignment.Center,
                Text = aPIValues.Find((value) => value.Name == "Website").Value,
            };
            Grid.SetColumnSpan(websiteTitle, 3);
            Grid.SetRow(websiteTitle, 0);
            websiteGrid.Children.Add(websiteTitle);

            for (int i = 0; i < aPIValues.Count - 1; i++)
            {
                var myTextBlock = new TextBlock();
                myTextBlock.Foreground = new SolidColorBrush(Colors.White);
                myTextBlock.FontSize = 9;
                myTextBlock.Background = new SolidColorBrush(Color.FromRgb(126, 126, 126));
                myTextBlock.Text = aPIValues[i].Name + Environment.NewLine + aPIValues[i].Value;
                Grid.SetColumn(myTextBlock, i);
                Grid.SetRow(myTextBlock, 1);
                websiteGrid.Children.Add(myTextBlock);
            }

            return websiteGrid;
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
