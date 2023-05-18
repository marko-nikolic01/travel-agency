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

namespace TravelAgency.WPF.ViewModels
{
    /// <summary>
    /// Interaction logic for AcceptTourRequestDialogue.xaml
    /// </summary>
    public partial class AcceptTourRequestDialogue : Page
    {
        public AcceptTourRequestDialogue(Domain.Models.TourRequest selectedRequest, int id, System.Windows.Navigation.NavigationService navigationService)
        {
            InitializeComponent();
            this.DataContext = new AcceptTourRequestViewModel(selectedRequest, id, navigationService);
        }
    }
}
