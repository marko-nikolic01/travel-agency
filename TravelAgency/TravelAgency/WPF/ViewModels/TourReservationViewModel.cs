﻿using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
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
        private string guestsLeft;
        private string addVisible;
        private bool submitEnabled;
        public string TourDescription { get; set; }
        public string GuestName { get => guestName; 
                                  set{ if (value != guestName) { guestName = value; OnPropertyChanged(); }}}
        public string GuestsNumber{ get => guestsNumber; 
                                    set { if (value != guestsNumber) { guestsNumber = value; OnPropertyChanged(); CheckSpotsNumber(); } } }
        public string GuestsLeft
        {
            get => guestsLeft;
            set
            {
                if (value != guestsLeft)
                {
                    guestsLeft = value; OnPropertyChanged();
                }
            }
        }
        public string SpotsLeft { get => spotsLeft;
                                  set { if (value != spotsLeft) { spotsLeft = value; OnPropertyChanged(); } } }
        public string AddButtonVisible { get => addVisible;
                                         set { if (value != addVisible) { addVisible = value; OnPropertyChanged(); } } }
        public bool IsSubmitButtonEnabled { get => submitEnabled;
            set { if (value != submitEnabled) { submitEnabled = value; OnPropertyChanged(); } } }
        public ObservableCollection<string> GuestsList { get; set; }

        private UserService userService;
        private TourOccurrenceService tourOccurrenceService;
        
        public string SelectedGuest { get; set; }
        public ButtonCommandNoParameter AddGuestCommand { get; set; }
        public ButtonCommandNoParameter RemoveGuestCommand { get; set; }
        private TourOccurrence tourOccurrence;

        public event PropertyChangedEventHandler? PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
        public TourReservationViewModel(TourOccurrence occurrence, int guestId)
        {
            AddButtonVisible = "Hidden";
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
            UpdateHelpText();
        }
        private void RemoveGuest()
        {
            if (SelectedGuest != null && SelectedGuest != GuestsList[0])
            {
                GuestsList.Remove(SelectedGuest);
                AddButtonVisible = "Visible";
                IsSubmitButtonEnabled = false;
                HowManyGuestsLeft();
            }
        }
        private void AddGuest()
        {
            if(GuestName != null && GuestName != "") 
            {
                GuestsList.Add(GuestName);
                GuestName = "";
                HowManyGuestsLeft();
                IsListBoxFull();
            }
        }
        private void IsListBoxFull()
        {
            if (GuestsList.Count == int.Parse(GuestsNumber))
            {
                AddButtonVisible = "Hidden";
                IsSubmitButtonEnabled = true;
            }
            else
            {
                AddButtonVisible = "Visible";
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
                    GuestsLeft = "";
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
                GuestsLeft = "";
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
                AddButtonVisible = "Hidden";
                IsSubmitButtonEnabled = true;
            }
            else if (currentGuestNumber > GuestsList.Count)
            {
                AddButtonVisible = "Visible";
                IsSubmitButtonEnabled = false;
                int x = currentGuestNumber - GuestsList.Count;
                GuestsLeft = "Add "+x+" more guests";
            }
            else
            {
                AddButtonVisible = "Hidden";
                IsSubmitButtonEnabled = true;
                GuestsLeft = "";
            }
        }
        private void HowManyGuestsLeft()
        {
            int currentGuestNumber = int.Parse(GuestsNumber);
            int x;
            if ((x = currentGuestNumber - GuestsList.Count) > 0)
                GuestsLeft = "Add " + x + " more guests";
            else
                GuestsLeft = "";
        }
        private void UpdateHelpText()
        {
            string file = @"../../../Resources/HelpTexts/MakeReservationHelp.txt";
            Guest2MainViewModel.HelpText = File.ReadAllText(file);
        }
    }
}
