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
        private bool _shouldValidate;
        public Guest1AccommodationReservationView()
        {
            InitializeComponent();

            _shouldValidate = true;

            datePickerFirstDate.DisplayDateStart = DateTime.Today;
            datePickerLastDate.DisplayDateStart = DateTime.Today;
            listViewAvailableDateSpans.Visibility = Visibility.Hidden;
            textBlockGuestNumber.Visibility = Visibility.Hidden;
            integerUpDownGuestNumber.Visibility = Visibility.Hidden;
            textBlockAvailableDates.Visibility = Visibility.Hidden;
        }

        private void ButtonPreviousPhoto_Click(object sender, RoutedEventArgs e)
        {
            /*ViewModel.GetPreviousPhoto();*/
        }

        private void ButtonNextPhoto_Click(object sender, RoutedEventArgs e)
        {
            /*ViewModel.GetNextPhoto();*/
        }

        private void IntegerUpDownDayNumber_ValueChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {/*
            _shouldValidate = false;
            ViewModel.TriggerValidationMessage(true, true);
            _shouldValidate = true;*/
        }

        private void DatePicker_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {/*
            if (!_shouldValidate)
            {
                return;
            }
            _shouldValidate = false;

            var datePicker = (sender as DatePicker);

            if (datePicker.Name == datePickerFirstDate.Name)
            {
                ViewModel.TriggerValidationMessage(false, true);
                _shouldValidate = true;
            }

            if (datePicker.Name == datePickerLastDate.Name)
            {
                ViewModel.TriggerValidationMessage(true, false);
                _shouldValidate = true;
            }*/
        }

        private void ButtonFindDates_Click(object sender, RoutedEventArgs e)
        {/*
            if (ViewModel.FindAvailableDates())
            {
                listViewAvailableDateSpans.Visibility = Visibility.Visible;
                textBlockGuestNumber.Visibility = Visibility.Visible;
                integerUpDownGuestNumber.Visibility = Visibility.Visible;
                integerUpDownGuestNumber.Value = 1;
                textBlockAvailableDates.Visibility = Visibility.Visible;
            }
            else
            {
                System.Windows.MessageBox.Show("Date span wasn't properly specified!");
            }*/
        }

        private void ButtonMakeReservation_Click(object sender, RoutedEventArgs e)
        {
            listViewAvailableDateSpans.SelectedItem = ((FrameworkElement)sender).DataContext;
            /*if (ViewModel.MakeReservation())
            {
                NavigateBack();
            }
            else
            {
                System.Windows.MessageBox.Show("Reservation wasn't properly specified!");
            }*/
        }

        private void ButtonBack_Click(object sender, RoutedEventArgs e)
        {/*
            NavigateBack();*/
        }

        private void TextBlockSerachAccommodations_MouseUp(object sender, MouseButtonEventArgs e)
        {/*
            NavigateBack();*/
        }

        private void NavigateBack()
        {/*
            this.NavigationService.Navigate(_returnPage);*/
        }

        private void TextBlockAccommodationsReservations_MouseUp(object sender, MouseButtonEventArgs e)
        {/*
            this.NavigationService.Navigate(new AccommodationsReservationsMenuView(_mainWindow, ViewModel.Guest));*/
        }
    }
}
