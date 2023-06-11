using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using TravelAgency.WPF.ViewModels;

namespace TravelAgency.WPF.Views
{
    /// <summary>
    /// Interaction logic for IntroductionWizard.xaml
    /// </summary>
    public partial class IntroductionWizardWindow : Page
    {
        public IntroductionWizardWindow(NavigationService navService, int guestId)
        {
            InitializeComponent();
            DataContext = new IntroductionWizardViewModel(navService, guestId);
        }
    }
}
