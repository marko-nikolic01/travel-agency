using System.Collections.Generic;
using System.Windows;
using System.Windows.Documents;
using TravelAgency.Domain.Models;
using TravelAgency.WPF.ViewModels;

namespace TravelAgency.WPF.Views
{
    public partial class TourPhotosView : Window
    {
        public TourPhotosView(List<string> links)
        {
            InitializeComponent();
            DataContext = new TourPhotosViewModel(links);
        }
    }
}
