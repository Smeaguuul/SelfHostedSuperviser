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
    /// Interaction logic for AddService.xaml
    /// </summary>
    public partial class AddService : Window
    {
        private AddServiceViewModel _viewModel;

        public AddService()
        {
            InitializeComponent();
            _viewModel = new AddServiceViewModel();

            this.DataContext = _viewModel;
        }
    }
}
