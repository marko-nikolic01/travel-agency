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
    public enum AccommodationReservationMoveRequestStatus { WAITING, ACCEPTED, REJECTED }
    public class AccommodationReservationMoveRequest : ISerializable, IDataErrorInfo
    {
        public int Id { get; set; }
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
        public DateOnly RequestDate { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public AccommodationReservationMoveRequest()
        {
            Id = -1;
            Reservation = new AccommodationReservation();
            Status = AccommodationReservationMoveRequestStatus.WAITING;
            StatusChanged = false;
            RejectionExplanation = "";
            DateSpan = new DateSpan();
            RequestDate = DateOnly.FromDateTime(DateTime.Now);
        }

        public AccommodationReservationMoveRequest(AccommodationReservation reservation)
        {
            Id = -1;
            Reservation = reservation;
            Status = AccommodationReservationMoveRequestStatus.WAITING;
            StatusChanged = false;
            RejectionExplanation = "";
            RequestDate = DateOnly.FromDateTime(DateTime.Now);
        }

        public bool CheckExpiration()
        {
            bool requestExpired = (Status == AccommodationReservationMoveRequestStatus.WAITING) &&
                            ((DateSpan.StartDate.CompareTo(DateOnly.FromDateTime(DateTime.Now)) <= 0) ||
                            (Reservation.DateSpan.StartDate.CompareTo(DateOnly.FromDateTime(DateTime.Now)) <= 0));
            if (requestExpired)
            {
                Status = AccommodationReservationMoveRequestStatus.REJECTED;
                RejectionExplanation = "Zahtev je istekao.";
                StatusChanged = true;
                return true;
            }
            return false;
        }

        public string[] ToCSV()
        {
            string[] csvValues =
            {
                Id.ToString(),
                Reservation.Id.ToString(),
                Convert.ToInt32(Status).ToString(),
                Convert.ToInt32(StatusChanged).ToString(),
                DateSpan.StartDate.ToString("dd/MM/yyyy"),
                DateSpan.EndDate.ToString("dd/MM/yyyy"),
                RequestDate.ToString("dd/MM/yyyy"),
                RejectionExplanation
            };
            return csvValues;
        }

        public void FromCSV(string[] values)
        {
            Id = Convert.ToInt32(values[0]);
            Reservation.Id = Convert.ToInt32(values[1]);
            Status = (AccommodationReservationMoveRequestStatus)Convert.ToInt32(values[2]);
            StatusChanged = Convert.ToBoolean(Convert.ToInt32(values[3]));
            DateSpan.StartDate = DateOnly.ParseExact(values[4], "dd/MM/yyyy");
            DateSpan.EndDate = DateOnly.ParseExact(values[5], "dd/MM/yyyy");
            RequestDate = DateOnly.ParseExact(values[6], "dd/MM/yyyy");
            RejectionExplanation = values[7];
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
                    else if (DateSpan.StartDate.CompareTo(Reservation.DateSpan.StartDate) == 0)
                    {
                        return "* Date span for new date must be different from old date span";
                    }
                }

                return null;
            }
        }

        private readonly string[] _validatedProperties = { "DateSpan" };

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
