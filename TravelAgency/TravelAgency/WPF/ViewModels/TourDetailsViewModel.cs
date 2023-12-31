﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using TravelAgency.Domain.Models;

namespace TravelAgency.WPF.ViewModels
{
    public class TourDetailsViewModel : INotifyPropertyChanged
    {
        public TourRating TourRating { get; set; }
        public User Guest { get; set; }
        public TourOccurrence TourOccurrence { get; set; }
        public KeyPoint ArrivalKeyPoint { get; set; }

        private TourRatingPhoto currentPhoto;
        public TourRatingPhoto CurrentPhoto
        {
            get { return currentPhoto; }
            set
            {
                if (value != currentPhoto)
                {
                    currentPhoto = value;
                    OnPropertyChanged();
                }
            }
        }
        private bool canUndo;
        public bool CanUndo
        {
            get { return canUndo; }
            set 
            { 
                canUndo = value;
                OnPropertyChanged();
            }
        }
        public event PropertyChangedEventHandler? PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public TourDetailsViewModel(TourRating tourRating)
        {
            TourRating = tourRating;
            if (tourRating.PhotoUrls.Count > 0)
            {
                CurrentPhoto = tourRating.PhotoUrls[0];
            }
            CanUndo = false;
        }
    }
}
