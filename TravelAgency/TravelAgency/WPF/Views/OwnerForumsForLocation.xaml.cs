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
using TravelAgency.WPF.Commands;
using TravelAgency.WPF.ViewModels;

namespace TravelAgency.WPF.Views
{
    /// <summary>
    /// Interaction logic for OwnerForumsForLocation.xaml
    /// </summary>
    public partial class OwnerForumsForLocation : Page
    {
        public MyICommand NavigateBackCommand { get; set; }
        public MyICommand NavigateToForumCommand { get; set; }

        public OwnerForumsForLocationViewModel ViewModel { get; set; }

        public OwnerForumsForLocation(OwnerForumsForLocationViewModel viewModel)
        {
            NavigateBackCommand = new MyICommand(Execute_NavigateBack);
            NavigateToForumCommand = new MyICommand(Execute_NavigateToForumCommand);
            InitializeComponent();
            ViewModel = viewModel;
            DataContext = ViewModel;

            Loaded += (s, e) => Keyboard.Focus(this);
            forumsListView.Loaded += PreselectFirstItem;
        }

        private void Execute_NavigateBack()
        {
            NavigationService.Navigate(new Uri("WPF/Views/OwnerForumView.xaml", UriKind.Relative));
        }

        private void NavigateBack_Click(object sender, RoutedEventArgs e)
        {
            Execute_NavigateBack();
        }

        private void Execute_NavigateToForumCommand()
        {
            if (ViewModel.SelectedForum == null)
            {
                MessageBox.Show("Select a forum.", "No forum selected", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            OwnerForumOverviewViewModel vm = new OwnerForumOverviewViewModel(ViewModel.SelectedForum.Forum);
            OwnerForumOverviewView page = new OwnerForumOverviewView(vm);
            this.NavigationService.Navigate(page);
        }

        private void NavigateToForumCommand_Click(object sender, RoutedEventArgs e)
        {
            Execute_NavigateToForumCommand();
        }

        private void PreselectFirstItem(object sender, RoutedEventArgs e)
        {
            if (forumsListView.Items.Count > 0)
            {
                forumsListView.SelectedItem = forumsListView.Items[0];
                ListBoxItem selectedItem = (ListBoxItem)forumsListView.ItemContainerGenerator.ContainerFromItem(forumsListView.SelectedItem);
                selectedItem.Focus();
            }
        }
    }
}
