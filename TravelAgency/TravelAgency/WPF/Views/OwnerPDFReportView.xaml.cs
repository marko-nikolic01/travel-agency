using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using TravelAgency.WPF.Commands;
using TravelAgency.WPF.ViewModels;

namespace TravelAgency.WPF.Views
{
    /// <summary>
    /// Interaction logic for OwnerPDFReportView.xaml
    /// </summary>
    public partial class OwnerPDFReportView : Page
    {
        public MyICommand NavigateBackCommand { get; set; }

        public OwnerPDFReportViewModel ViewModel { get; set; }

        public OwnerPDFReportView()
        {
            NavigateBackCommand = new MyICommand(Execute_NavigateBackCommand);
            InitializeComponent();
            ViewModel = new OwnerPDFReportViewModel();
            DataContext = ViewModel;

            this.Loaded += (s, e) => Keyboard.Focus(this);
        }

        private void Execute_NavigateBackCommand()
        {
            this.NavigationService.Navigate(new Uri("WPF/Views/OwnerRenovationsView.xaml", UriKind.Relative));
        }

        private void NavigateBack_Click(object sender, RoutedEventArgs e)
        {
            Execute_NavigateBackCommand();
        }

        private void GenerateReport_Click(object sender, RoutedEventArgs e)
        {
            ViewModel.GenerateReportCommand.Execute();
        }
    }
}
