﻿using System;
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
using TravelAgency.Domain.Models;
using TravelAgency.Services;
using TravelAgency.WPF.Commands;
using TravelAgency.WPF.ViewModels;

namespace TravelAgency.WPF.Pages
{
    /// <summary>
    /// Interaction logic for OwnerManageAccommodationsPage.xaml
    /// </summary>
    public partial class OwnerManageAccommodationsView : Page
    {
        public MyICommand NavigateBackCommand { get; set; }
        public MyICommand AddAccommodationCommand { get; set; }
        public OwnerManageAccommodationsViewModel ViewModel { get; set; }

        public OwnerManageAccommodationsView()
        {
            NavigateBackCommand = new MyICommand(Execute_NavigateBack);
            AddAccommodationCommand = new MyICommand(Execute_AddAccommodation);
            InitializeComponent();
            ViewModel = new OwnerManageAccommodationsViewModel();
            DataContext = ViewModel;

            Loaded += (s, e) => Keyboard.Focus(this);
        }

        private void Execute_AddAccommodation()
        {
            OwnerAddAccommodationViewModel vm = new OwnerAddAccommodationViewModel(this.NavigationService);
            OwnerAddAccommodationView ownerAddAccommodationPage = new OwnerAddAccommodationView(vm);
            this.NavigationService.Navigate(ownerAddAccommodationPage);
        }

        private void Execute_NavigateBack()
        {
            NavigationService.Navigate(new Uri("WPF/Views/OwnerAccommodationsView.xaml", UriKind.Relative));
        }

        private void NavigateBack_Click(object sender, RoutedEventArgs e)
        {
            Execute_NavigateBack();
        }

        private void AddAccommodation_Click(object sender, RoutedEventArgs e)
        {
            Execute_AddAccommodation();
        }
    }
}