using System;
using System.Windows;
using System.Windows.Media.Animation;
using TravelAgency.Repositories;
using TravelAgency.WPF.ViewModels;

namespace TravelAgency.WPF.Views
{
    /// <summary>
    /// Interaction logic for Guest2MainWindow.xaml
    /// </summary>
    public partial class Guest2MainWindow : Window
    {
        public Guest2MainViewModel ViewModel { get; set; }
        public Guest2MainWindow(int guestId)
        {
            InitializeComponent();
            ViewModel = new Guest2MainViewModel(this.frame.NavigationService, guestId);
            this.DataContext = ViewModel;
            ProgramStatusRepository repository = new ProgramStatusRepository();
            if (repository.GetProgramStatus().IsFirstTimeOpening)
            {
                Guest2MainViewModel.MenuVisible = "Hidden";
                IntroductionWizardWindow introduction = new IntroductionWizardWindow(this.frame.NavigationService, guestId);
                this.frame.NavigationService.Navigate(introduction);
            }
            else
            {
                Guest2ProfileView guest2 = new Guest2ProfileView(guestId, this.frame.NavigationService);
                this.frame.NavigationService.Navigate(guest2);
            }
            
            HelpGrid.Height = 0;
        }
        private void SignOut_Click(object sender, RoutedEventArgs e)
        {
            new MainWindow().Show();
            Close();
        }

        private void HelpButton_Click(object sender, RoutedEventArgs e)
        {
            if (HelpGrid.Height == 0)
            {
                DoubleAnimation heightAnimation = new DoubleAnimation(600, new Duration(TimeSpan.FromSeconds(0.6)));
                HelpGrid.BeginAnimation(HeightProperty, heightAnimation);
            }
            else
            {
                DoubleAnimation heightAnimation = new DoubleAnimation(0, new Duration(TimeSpan.FromSeconds(0.3)));
                HelpGrid.BeginAnimation(HeightProperty, heightAnimation);
            }
        }
    }
}
