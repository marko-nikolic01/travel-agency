using System.Windows;
using TravelAgency.WPF.ViewModels;

namespace TravelAgency.WPF.Views
{
    /// <summary>
    /// Interaction logic for IntroductionWizard.xaml
    /// </summary>
    public partial class IntroductionWizardWindow : Window
    {
        public IntroductionWizardWindow(int id)
        {
            InitializeComponent();
            DataContext = new IntroductionWizardViewModel(id);
        }
    }
}
