using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using TravelAgency.Serializer;

namespace TravelAgency.Domain.Models
{
    public class AccommodationRenovation : ISerializable, INotifyPropertyChanged, IDataErrorInfo
    {
        public int Id { get; set; }
        public int AccommodationId { get; set; }
        public Accommodation? Accommodation { get; set; }
        private DateSpan dateSpan;
        public DateSpan DateSpan
        {
            get { return dateSpan; }
            set
            {
                dateSpan = value;
                OnPropertyChanged(nameof(DateSpan));
            }
        }
        private string description;
        public string Description
        {
            get { return description; }
            set { description = value; }
        }

        public AccommodationRenovation()
        {
            Id = -1;
            AccommodationId = -1;
            DateSpan = new DateSpan();
            Description = string.Empty;
        }

        public void FromCSV(string[] values)
        {
            Id = Convert.ToInt32(values[0]);
            AccommodationId = Convert.ToInt32(values[1]);
            DateSpan.StartDate = DateOnly.ParseExact(values[2], "dd/MM/yyyy");
            DateSpan.EndDate = DateOnly.ParseExact(values[3], "dd/MM/yyyy");
            Description = values[4];
        }

        public string[] ToCSV()
        {
            string[] csvValues =
            {
                Id.ToString(),
                AccommodationId.ToString(),
                DateSpan.StartDate.ToString("dd/MM/yyyy"),
                DateSpan.EndDate.ToString("dd/MM/yyyy"),
                Description
            };

            return csvValues;
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public string Error => null;

        public string this[string columnName] => throw new NotImplementedException();
    }
}
