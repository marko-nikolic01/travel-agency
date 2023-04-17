using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using TravelAgency.Serializer;

namespace TravelAgency.Model
{
    public enum AccommodationReservationMoveRequestStatus { WAITING, ACCEPTED, REJECTED }
    public class AccommodationReservationMoveRequest: ISerializable, IDataErrorInfo
    {
        public int Id { get; set; }
        public int ReservationId { get; set; }
        public AccommodationReservation Reservation { get; set; }
        public AccommodationReservationMoveRequestStatus Status { get; set; }
        public bool StatusChanged { get; set; }
        public string RejectionExplanation { get; set; }
        private DateSpan _dateSpan;

        public DateSpan DateSpan
        {
            get => _dateSpan;
            set
            {
                if (value != _dateSpan)
                {
                    _dateSpan = value;
                    OnPropertyChanged("DateSpan");
                }
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public AccommodationReservationMoveRequest()
        {
            Id = -1;
            ReservationId = -1;
            Status = AccommodationReservationMoveRequestStatus.WAITING;
            StatusChanged = false;
            RejectionExplanation = "";
            DateSpan = new DateSpan();
        }

        public AccommodationReservationMoveRequest(AccommodationReservation reservation)
        {
            Id = -1;
            ReservationId = reservation.Id;
            Reservation = reservation;
            Status = AccommodationReservationMoveRequestStatus.WAITING;
            StatusChanged = false;
            RejectionExplanation = "";
        }

        public string[] ToCSV()
        {
            string[] csvValues =
            {
                Id.ToString(),
                ReservationId.ToString(),
                Convert.ToInt32(Status).ToString(),
                Convert.ToInt32(StatusChanged).ToString(),
                DateSpan.StartDate.ToString("dd/MM/yyyy"),
                DateSpan.EndDate.ToString("dd/MM/yyyy"),
                RejectionExplanation
            };
            return csvValues;
        }

        public void FromCSV(string[] values)
        {
            Id = Convert.ToInt32(values[0]);
            ReservationId = Convert.ToInt32(values[1]);
            Status = (AccommodationReservationMoveRequestStatus)Convert.ToInt32(values[2]);
            StatusChanged = Convert.ToBoolean(Convert.ToInt32(values[3]));
            DateSpan.StartDate = DateOnly.ParseExact(values[4], "dd/MM/yyyy");
            DateSpan.EndDate = DateOnly.ParseExact(values[5], "dd/MM/yyyy");
            RejectionExplanation = values[6];
        }
        

        public string Error => null;

        public string this[string columnName]
        {
            get
            {
                if (columnName == "DateSpan")
                {
                    if (DateSpan == null)
                    {
                        return "* Select a date span";
                    }
                }

                return null;
            }
        }

        private readonly string[] _validatedProperties = { "NumberOfGuests", "DateSpan" };

        public bool IsValid
        {
            get
            {
                foreach (var property in _validatedProperties)
                {
                    if (this[property] != null)
                        return false;
                }

                return true;
            }
        }
    }
}
