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
using TravelAgency.Domain.Models;
using TravelAgency.WPF.ViewModels;

namespace TravelAgency.WPF.Views
{
    /// <summary>
    /// Interaction logic for Guest1AccommodationReservationView.xaml
    /// </summary>
    public partial class Guest1AccommodationReservationView : UserControl
    {
        public Guest1AccommodationReservationView()
        {
            InitializeComponent();
            Loaded += (s, e) => Keyboard.Focus(this);
        }

        private void ButtonMakeReservation_Click(object sender, RoutedEventArgs e)
        {
            listViewAvailableDateSpans.SelectedItem = ((FrameworkElement)sender).DataContext;
        }
    }
}
