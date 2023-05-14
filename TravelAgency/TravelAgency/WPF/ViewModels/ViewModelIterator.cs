﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using TravelAgency.Services;

namespace TravelAgency.WPF.ViewModels
{
    public class ViewModelIterator : INotifyPropertyChanged
    {
        private string backButtonVisibility;
        private string nextButtonVisibility;
        public string BackButtonVisibility
        {
            get => backButtonVisibility;
            set { if (value != backButtonVisibility) { backButtonVisibility = value; OnPropertyChanged(); } }
        }
        public string NextButtonVisibility
        {
            get => nextButtonVisibility;
            set { if (value != nextButtonVisibility) { nextButtonVisibility = value; OnPropertyChanged(); } }
        }
        public List<TourRequestFormViewModel> viewModels;
        int i;
        public int currentGuestId;
        public event PropertyChangedEventHandler? PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        public ViewModelIterator(int guestId)
        {
            currentGuestId = guestId;
            viewModels = new List<TourRequestFormViewModel>();
            viewModels.Add(new TourRequestFormViewModel(currentGuestId));
            i = 0;
            BackButtonVisibility = "Hidden";
            NextButtonVisibility = "Hidden";
        }
        public TourRequestFormViewModel GetViewModelInstance()
        {
            return viewModels[i];
        }
        public TourRequestFormViewModel AddNextViewModel()
        {
            if (viewModels[viewModels.Count - 1].Valid())
            {
                viewModels.Add(new TourRequestFormViewModel(currentGuestId));
            }
            i = viewModels.Count - 1;
            BackButtonVisibility = "Visible";
            NextButtonVisibility = "Hidden";
            return viewModels[i];
        }
        public TourRequestFormViewModel GetPreviousViewModel()
        {
            i--;
            if(i == 0)
            {
                BackButtonVisibility = "Hidden";
            }
            NextButtonVisibility = "Visible";
            return viewModels[i];
        }
        public TourRequestFormViewModel GetNextViewModel()
        {
            i++;
            if (i == viewModels.Count - 1)
            {
                NextButtonVisibility = "Hidden";
            }
            BackButtonVisibility = "Visible";
            return viewModels[i];
        }
        public void SaveSpecialTourRequest()
        {
            SpecialTourRequestService service = new SpecialTourRequestService();
            int specialRequestId = service.SaveSpecialTourRequest(currentGuestId);
            foreach (TourRequestFormViewModel tourRequest in viewModels)
            {
                tourRequest.SubmitRequest(specialRequestId);
            }
        }
    }
}