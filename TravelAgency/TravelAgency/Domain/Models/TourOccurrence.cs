﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics.Metrics;
using System.Globalization;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using TravelAgency.Domain.RepositoryInterfaces;

namespace TravelAgency.Domain.Models
{
    public enum CurrentState { NotStarted, Started, Ended }
    public class TourOccurrence : Serializer.ISerializable, INotifyPropertyChanged
    {
        public int Id { get; set; }
        public int TourId { get; set; }
        public Tour Tour { get; set; }
        public DateTime DateTime { get; set; }
        public List<KeyPoint> KeyPoints { get; set; }
        public List<User> Guests { get; set; }

        private CurrentState currentState;
        public CurrentState CurrentState
        {
            get { return currentState; }
            set 
            {
                if (value != currentState)
                {
                    currentState = value;
                    OnPropertyChanged();
                }
            }
        }
        public User Guide { get; set; }
        public int GuideId { get; set; }
        public int FreeSpots { get; set; }
        public int ActiveKeyPointId { get; set; }
        public string DetailedRowString { get; set; }
        public bool IsDeleted { get; set; }
        private int toShadow;
        public int ToShadow
        {
            get => toShadow;
            set
            {
                if (value != toShadow)
                {
                    toShadow = value;
                    OnPropertyChanged();
                }
            }
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        public TourOccurrence(int id, int tourId, Tour tour, DateTime dateTime, List<KeyPoint> keyPoints)
        {
            Id = id;
            TourId = tourId;
            Tour = tour;
            DateTime = dateTime;
            KeyPoints = keyPoints;
            FreeSpots = tour.MaxGuestNumber;
            ActiveKeyPointId = -1;
            IsDeleted = false;
        }

        public TourOccurrence()
        {
            KeyPoints = new List<KeyPoint>();
            Guests = new List<User>();
            ActiveKeyPointId = -1;
            IsDeleted = false;
        }

        public void MakeDetailedRowString()
        {
            DetailedRowString = "Description: " + Tour.Description + " Tour duration: " + Tour.Duration + " hours.";
        }
        public string[] ToCSV()
        {
            string[] csvValues = { Id.ToString(), TourId.ToString(), DateTime.ToString("dd-MM-yyyy HH-mm"), ((int)CurrentState).ToString(), GuideId.ToString(), FreeSpots.ToString(), ActiveKeyPointId.ToString(), IsDeleted.ToString() };
            return csvValues;
        }

        public void FromCSV(string[] values)
        {
            Id = int.Parse(values[0]);
            TourId = int.Parse(values[1]);
            DateTime = DateTime.ParseExact(values[2], "dd-MM-yyyy HH-mm", CultureInfo.InvariantCulture);
            CurrentState = (CurrentState)Convert.ToInt32(values[3]);
            GuideId = int.Parse(values[4]);
            FreeSpots = int.Parse(values[5]);
            ActiveKeyPointId = int.Parse(values[6]);
            IsDeleted = bool.Parse(values[7]);
        }

        public string GetKeyPointsString()
        {
            string result = "";
            foreach (var keyPoint in KeyPoints)
            {
                result = result + keyPoint.Name;
                if (KeyPoints[KeyPoints.Count - 1] != keyPoint)
                {
                    result = result + ", ";
                }
            }
            return result;
        }
        public string GetActiveTourString(string keyPointName)
        {
            string result;
            result = "Active tour: " + Tour.Name;
            result += "\n" + Tour.Description;
            result += "\nCurrent key point: " + keyPointName;
            return result;
        }
        public bool IsInAppropriateDateSpan(DateTime startDate, DateTime endDate)
        {
            if (DateTime >= startDate && DateTime <= endDate)
                return true;
            else
                return false;
        }

        public bool IsDateTimeFree(DateTime concreteDateTime, int duration)
        {
            if (DateTime.Date == concreteDateTime.Date)
            {
                if ((DateTime <= concreteDateTime) && (DateTime.AddHours(duration) >= concreteDateTime))
                {
                    return false;
                }
                else if ((DateTime >= concreteDateTime) && (DateTime <= concreteDateTime.AddHours(duration)))
                {
                    return false;
                }
            }
            return true;
        }
    }
}
