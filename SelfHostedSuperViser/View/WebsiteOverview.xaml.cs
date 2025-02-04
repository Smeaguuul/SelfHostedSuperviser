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

        private Border GetAPIDataElement(List<APIValue> aPIValues)
        {

            var border = new Border()
            {
                Background = new SolidColorBrush(Color.FromRgb(50, 50, 50)),
                CornerRadius = new CornerRadius(10),
                Padding = new Thickness(10),
                Margin = new Thickness(5)
            };

            // Create the Grid
            var websiteGrid = new Grid()
            {
                Width = 200,
                Height = 80,
                Opacity = 0.9
            };

            var row1 = new RowDefinition() { Height = new GridLength(30) };
            var row2 = new RowDefinition() { Height = new GridLength(1, GridUnitType.Star) };

            websiteGrid.RowDefinitions.Add(row1);
            websiteGrid.RowDefinitions.Add(row2);

            // Creates columns based on the number of API values
            for (int i = 0; i < aPIValues.Count - 1; i++)
            {
                var column1 = new ColumnDefinition();
                column1.Width = new GridLength(1, GridUnitType.Star);
                websiteGrid.ColumnDefinitions.Add(column1);
            }

            // Title TextBlock
            var websiteTitle = new TextBlock()
            {
                Foreground = new SolidColorBrush(Colors.White),
                HorizontalAlignment = HorizontalAlignment.Center,
                VerticalAlignment = VerticalAlignment.Center, // Center vertically
                FontSize = 14, // Increased font size for better readability
                FontWeight = FontWeights.Bold, // Bold title
                Text = aPIValues.Find((value) => value.Name == "Website").Value,
            };

            Grid.SetColumnSpan(websiteTitle, aPIValues.Count - 1);
            Grid.SetRow(websiteTitle, 0);
            websiteGrid.Children.Add(websiteTitle);

            // Content TextBlocks
            for (int i = 0; i < aPIValues.Count - 1; i++)
            {
                var myTextBlock = new TextBlock()
                {
                    Foreground = new SolidColorBrush(Colors.White),
                    FontSize = 10,
                    Background = new SolidColorBrush(Color.FromRgb(70, 70, 70)),
                    Padding = new Thickness(5),
                    Text = $"{aPIValues[i].Name}: " + Environment.NewLine + aPIValues[i].Value,
                    TextWrapping = TextWrapping.Wrap,
                    VerticalAlignment = VerticalAlignment.Center
                };

                Grid.SetColumn(myTextBlock, i);
                Grid.SetRow(myTextBlock, 1);
                websiteGrid.Children.Add(myTextBlock);
            }

            border.Child = websiteGrid;
            return border;
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
