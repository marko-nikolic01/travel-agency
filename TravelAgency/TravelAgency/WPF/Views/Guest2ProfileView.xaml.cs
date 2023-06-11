using System.Windows.Controls;
using TravelAgency.WPF.ViewModels;
using PdfSharp.Drawing;
using PdfSharp.Drawing.Layout;
using PdfSharp.Pdf;
using System;
using System.Text;
using System.Windows.Forms;
using TravelAgency.Domain.DTOs;
using System.Windows;
using System.Windows.Navigation;

namespace TravelAgency.WPF.Views
{
    /// <summary>
    /// Interaction logic for Guest2ProfileView.xaml
    /// </summary>
    public partial class Guest2ProfileView : Page
    {
        Guest2ProfileViewModel viewModel;
        public Guest2ProfileView(int id, NavigationService navService)
        {
            InitializeComponent();
            viewModel = new Guest2ProfileViewModel(id, navService);
            DataContext = viewModel;
        }
        
        private void ConfirmPassword(object sender, System.Windows.RoutedEventArgs e)
        {
            viewModel.ConfirmNewPassword(oldPwdBox.Password, newPwdBox.Password);
        }
    }
}
