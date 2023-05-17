using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using TravelAgency.Commands;
using TravelAgency.Domain.Models;
using TravelAgency.Services;

namespace TravelAgency.WPF.ViewModels
{
    public class TourReservationViewModel: INotifyPropertyChanged
    {
        int input;
        private string guestName;
        private string guestsNumber;
        private string spotsLeft;
        private bool addEnabled;
        private bool submitEnabled;
        private bool guestsHelpClicked;
        private bool numGuestsHelpClicked;
        private bool voucherHelpClicked;
        public string TourDescription { get; set; }
        public string GuestName { get => guestName; 
                                  set{ if (value != guestName) { guestName = value; OnPropertyChanged(); }}}
        public string GuestsNumber{ get => guestsNumber; 
                                    set { if (value != guestsNumber) { guestsNumber = value; OnPropertyChanged(); CheckSpotsNumber(); } } }

        public string SpotsLeft { get => spotsLeft;
                                  set { if (value != spotsLeft) { spotsLeft = value; OnPropertyChanged(); } } }
        public bool IsAddButtonEnabled { get => addEnabled;
                                         set { if (value != addEnabled) { addEnabled = value; OnPropertyChanged(); } } }
        public bool IsSubmitButtonEnabled { get => submitEnabled;
            set { if (value != submitEnabled) { submitEnabled = value; OnPropertyChanged(); } } }
        public bool GuestsHelpClicked { get => guestsHelpClicked;
            set { if (value != guestsHelpClicked) { guestsHelpClicked = value; OnPropertyChanged(); } } }
        public bool NumGuestsHelpClicked
        {
            get => numGuestsHelpClicked;
            set { if (value != numGuestsHelpClicked) { numGuestsHelpClicked = value; OnPropertyChanged(); } }
        }
        public bool VoucherHelpClicked{ get => voucherHelpClicked;
            set { if (value != voucherHelpClicked) { voucherHelpClicked = value; OnPropertyChanged(); } } }
        public ObservableCollection<string> GuestsList { get; set; }

        private UserService userService;
        private TourOccurrenceService tourOccurrenceService;
        
        public string SelectedGuest { get; set; }
        public ButtonCommandNoParameter AddGuestCommand { get; set; }
        public ButtonCommandNoParameter RemoveGuestCommand { get; set; }
        public ButtonCommandNoParameter GuestsHelpCommand { get; set; }
        public ButtonCommandNoParameter NumGuestsHelpCommand { get; set; }
        public ButtonCommandNoParameter VoucherHelpCommand { get; set; }
        private TourOccurrence tourOccurrence;

        public event PropertyChangedEventHandler? PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
        public TourReservationViewModel(TourOccurrence occurrence, int guestId)
        {
            IsAddButtonEnabled = false;
            IsSubmitButtonEnabled = true;
            tourOccurrence = occurrence;
            userService = new UserService();
            tourOccurrenceService = new TourOccurrenceService();
            GuestsList = new ObservableCollection<string>();
            User user = userService.GetById(guestId);
            GuestsList.Add(user.Username);
            TourDescription = tourOccurrence.Tour.Name + " in " + tourOccurrence.Tour.Location.Country +
                ", " + tourOccurrence.Tour.Location.City + ". " + tourOccurrence.Tour.Description;
            GuestsNumber = "1";
            AddGuestCommand = new ButtonCommandNoParameter(AddGuest);
            RemoveGuestCommand = new ButtonCommandNoParameter(RemoveGuest);
            GuestsHelpCommand = new ButtonCommandNoParameter(GuestsHelpClick);
            NumGuestsHelpCommand = new ButtonCommandNoParameter(NumGuestsHelpClick);
            VoucherHelpCommand = new ButtonCommandNoParameter(VoucherHelpClick);
        }
        private void NumGuestsHelpClick()
        {
            NumGuestsHelpClicked = !NumGuestsHelpClicked;
        }
        private void GuestsHelpClick()
        {
            GuestsHelpClicked = !GuestsHelpClicked;
        }
        private void VoucherHelpClick()
        {
            VoucherHelpClicked = !VoucherHelpClicked;
        }
        private void RemoveGuest()
        {
            if (SelectedGuest != null && SelectedGuest != GuestsList[0])
            {
                GuestsList.Remove(SelectedGuest);
                IsAddButtonEnabled = true;
                IsSubmitButtonEnabled = false;
            }
        }
        private void AddGuest()
        {
            if(GuestName != null && GuestName != "") 
            {
                GuestsList.Add(GuestName);
                GuestName = "";
                IsListBoxFull();
            }
        }
        private void IsListBoxFull()
        {
            if (GuestsList.Count == int.Parse(GuestsNumber))
            {
                IsAddButtonEnabled = false;
                IsSubmitButtonEnabled = true;
            }
            else
            {
                IsAddButtonEnabled = true;
                IsSubmitButtonEnabled = false;
            }
        }
        public void SubmitReservation()
        {
            TourReservationService service = new TourReservationService();
            service.SubmitReservation(GuestsList, tourOccurrence);
            tourOccurrenceService.UpdateTour(tourOccurrence);
        }

        private bool IsValid()
        {
           
            if (int.TryParse(GuestsNumber, out input))
            {
                if(input > 0)
                    return true;
                else 
                    return false;
            }
            else
                return false;
        }
        public void CheckSpotsNumber()
        {
            if(IsValid())
            {
                int spotsLeft = tourOccurrence.Tour.MaxGuestNumber - (tourOccurrence.Guests.Count + input);
                if (spotsLeft < 0)
                {
                    SpotsLeft = "Not enough\nspots on tour";
                    IsSubmitButtonEnabled = false;
                }
                else
                {
                    UpdateList();
                    SpotsLeft = "Spots left: " + spotsLeft.ToString();
                }
            }
            else
            {
                SpotsLeft = "Wrong input";
                IsSubmitButtonEnabled = false;
            }   
        }
        private void UpdateList()
        {
            int currentGuestNumber = int.Parse(GuestsNumber);
            if (currentGuestNumber < GuestsList.Count)
            {
                for (int i = GuestsList.Count - 1; i >= currentGuestNumber; i--)
                    GuestsList.RemoveAt(i);
                IsAddButtonEnabled = false;
                IsSubmitButtonEnabled = true;
            }
            else if (currentGuestNumber > GuestsList.Count)
            {
                IsAddButtonEnabled = true;
                IsSubmitButtonEnabled = false;
            }
            else
            {
                IsAddButtonEnabled = false;
                IsSubmitButtonEnabled = true;
            }
        }
    }
}
