using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
using System.Windows.Navigation;
using System.Windows.Shapes;
using TravelAgency.Domain.DTOs;
using TravelAgency.Domain.Models;
using TravelAgency.Services;
using TravelAgency.WPF.Commands;
using TravelAgency.WPF.ViewModels;

namespace TravelAgency.WPF.Pages
{
    /// <summary>
    /// Interaction logic for OwnerReservationsPage.xaml
    /// </summary>
    public partial class OwnerReservationsView : Page
    {
        public MyICommand FocusOtherDataGrid { get; set; }

        public OwnerReservationsViewModel ViewModel { get; set; }

        public OwnerReservationsView()
        {
            FocusOtherDataGrid = new MyICommand(Execute_FocusOtherDataGrid);
            InitializeComponent();
            ViewModel = new OwnerReservationsViewModel();
            DataContext = ViewModel;

            activeReservationsDataGrid.Focus();
        }

        private void Execute_FocusOtherDataGrid()
        {
            if (activeReservationsDataGrid.IsFocused)
            {
                if (activeReservationsDataGrid.Items.Count > 0)
                {
                    activeReservationsDataGrid.SelectedIndex = 0;
                }               
            }
            else
            {
                if (requestsDataGrid.Items.Count > 0)
                {
                    requestsDataGrid.SelectedIndex = 0;
                }
            }
        }
    }
}
