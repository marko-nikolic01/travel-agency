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
using System.Windows.Navigation;
using System.Windows.Shapes;
using TravelAgency.WPF.ViewModels;

namespace TravelAgency.WPF.Views
{
    /// <summary>
    /// Interaction logic for Guest1ForumsMenuView.xaml
    /// </summary>
    public partial class Guest1ForumsMenuView : UserControl
    {
        public wForumsMenuViewModel ViewModel { get; set; }
        private Guest1HomeView _mainWindow;

        public Guest1ForumsMenuView()
        {
            InitializeComponent();

            //_mainWindow = guest1HomeView;
        }

        private void ButtonBack_Click(object sender, RoutedEventArgs e)
        {
            //_mainWindow.HighlightSelectedTab(_mainWindow.buttonHome);
            //this.NavigationService.Navigate(new HomeMenuView(_mainWindow, ViewModel.Guest));
        }
    }
}
