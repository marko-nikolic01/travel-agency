using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics.Metrics;
using System.Globalization;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace TravelAgency.Model
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
        public CurrentState CurrentState { get; set; }

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

        private int toDisplay;
        public int ToDisplay
        {
            get => toDisplay;
            set
            {
                if (value != toDisplay)
                {
                    toDisplay = value;
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
        }

        public TourOccurrence()
        {
            KeyPoints = new List<KeyPoint>();
            Guests = new List<User>();
        }

        public string[] ToCSV()
        {
            string[] csvValues = { Id.ToString(), TourId.ToString(), DateTime.ToString("dd-MM-yyyy hh-mm"), ((int)CurrentState).ToString() };
            return csvValues;
        }

        public void FromCSV(string[] values)
        {
            Id = int.Parse(values[0]);
            TourId = int.Parse(values[1]);
            DateTime = DateTime.ParseExact(values[2], "dd-MM-yyyy HH-mm", CultureInfo.InvariantCulture);
            CurrentState = (CurrentState)Convert.ToInt32(values[3]);
        }
    }
}
