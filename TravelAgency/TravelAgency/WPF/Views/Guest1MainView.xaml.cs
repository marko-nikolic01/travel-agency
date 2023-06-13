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
using System.Windows.Threading;
using TravelAgency.Domain.Models;
using TravelAgency.WPF.Commands;
using TravelAgency.WPF.ViewModels;

namespace TravelAgency.WPF.Views
{
    /// <summary>
    /// Interaction logic for Guest1NavigationView.xaml
    /// </summary>
    public partial class Guest1MainView : Window
    {
        public MyICommand LogOutCommand { get; private set; }
        public Guest1MainView(User guest)
        {
            LogOutCommand = new MyICommand(OnLogOut);
            InitializeComponent();
            this.DataContext = new Guest1MainViewModel(guest);
        }

        private void ButtonLogOut_Click(object sender, RoutedEventArgs e)
        {
            OnLogOut();
        }

        private void OnLogOut()
        {
            MainWindow mainWindow = new MainWindow();
            mainWindow.Show();
            Close();
        }
    }
}
