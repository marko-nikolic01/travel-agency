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
using TravelAgency.ViewModel;

namespace TravelAgency.View
{
    /// <summary>
    /// Interaction logic for TourRequestForm.xaml
    /// </summary>
    public partial class TourRequestFormView : Window
    {
        public TourRequestFormViewModel TourRequestFormViewModel { get; set; }
        private int guestId;
        public TourRequestFormView(int id)
        {
            InitializeComponent();
            guestId = id;
            TourRequestFormViewModel = new TourRequestFormViewModel(guestId);
            DataContext = TourRequestFormViewModel;
        }

        private void Country_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            TourRequestFormViewModel.SetCitiesComboBox();         
        }

        private void SubmitButton_Click(object sender, RoutedEventArgs e)
        {
            if (TourRequestFormViewModel.SubmitRequest())
                Close();
        }
    }
}
