﻿using System;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace TravelAgency.WPF.Pages
{
    /// <summary>
    /// Interaction logic for OwnerAccommodations.xaml
    /// </summary>
    public partial class OwnerAccommodationsPage : Page
    {
        public OwnerAccommodationsPage()
        {
            InitializeComponent();
        }

        private void AccommodationsNavigationButton_Click(object sender, RoutedEventArgs e)
        {
            this.NavigationService.Navigate(new OwnerManageAccommodationsPage());
        }
    }
}