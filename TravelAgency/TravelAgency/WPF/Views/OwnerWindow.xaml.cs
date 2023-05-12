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
using TravelAgency.Domain.Models;
using TravelAgency.WPF.Commands;
using TravelAgency.WPF.Controls.CustomControls;
using TravelAgency.WPF.ViewModels;

namespace TravelAgency.WPF.Views
{
    /// <summary>
    /// Interaction logic for OwnerWindow.xaml
    /// </summary>
    public partial class OwnerWindow : Window
    {
        public OwnerMainViewModel MainViewModel { get; set; }
        public MyICommand<string> CheckRadioButtonCommand { get; private set; }

        public OwnerWindow()
        {
            CheckRadioButtonCommand = new MyICommand<string>(OnCheckRadioButton);

            InitializeComponent();
            MainViewModel = new OwnerMainViewModel();
            DataContext = MainViewModel;
        }

        private void OnCheckRadioButton(string button)
        {
            switch (button)
            {
                case "profile":
                    myProfileRadioButton.IsChecked = true;
                    MainViewModel.OnNav(button);
                    break;
                case "accommodations":
                    accommodationsRadioButton.IsChecked = true;
                    MainViewModel.OnNav(button);
                    break;
                case "reservations":
                    reservationsRadioButton.IsChecked = true;
                    MainViewModel.OnNav(button);
                    break;
                case "ratings":
                    ratingsRadioButton.IsChecked = true;
                    MainViewModel.OnNav(button);
                    break;
                case "forum":
                    forumRadioButton.IsChecked = true;
                    MainViewModel.OnNav(button);
                    break;
            }
        }
    }
}
