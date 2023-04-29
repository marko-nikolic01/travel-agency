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
        private List<string> guestsList;
        public string TourDescription { get; set; }
        public string GuestName { get => guestName; 
                                  set{ if (value != guestName) { guestName = value; OnPropertyChanged(); }}}
        public string GuestsNumber{ get => guestsNumber; 
                                    set { if (value != guestsNumber) { guestsNumber = value; OnPropertyChanged(); } } }

        public string SpotsLeft { get => spotsLeft;
                                  set { if (value != spotsLeft) { spotsLeft = value; OnPropertyChanged(); } } }
        public bool IsAddButtonEnabled { get => addEnabled;
                                         set { if (value != addEnabled) { addEnabled = value; OnPropertyChanged(); } } }
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
            GuestsNumber = "1";
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
            AddGuestCommand = new ButtonCommandNoParameter(AddGuest);
            RemoveGuestCommand = new ButtonCommandNoParameter(RemoveGuest);
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
            List<User> users = new List<User>();
            TourReservation tourReservation;
            User user;
            for (int i = 0; i < GuestsList.Count; i++)
            {
                user = GetUserByName(i);
                if (user == null)
                {
                    user = new User(GuestsList[i], "ftn", Roles.Guest2, new DateOnly(2004, 2, 15));
                    userService.SaveUser(user);
                }
                users.Add(user);
                tourReservation = new TourReservation(tourOccurrence.Id, user.Id);
                tourOccurrenceService.SaveTourReservation(tourReservation);
            }
            tourOccurrence.Guests.AddRange(users);
            tourOccurrence.FreeSpots -= users.Count;
            tourOccurrenceService.UpdateTour(tourOccurrence);

        }
        private User GetUserByName(int i)
        {
            return userService.GetAllUsers().Find(x => (x.Username == GuestsList[i] && x.Role == Roles.Guest2));
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
                    SpotsLeft = "Not enough spots on tour";
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
