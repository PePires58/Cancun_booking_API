using Domain.Enuns;
using Domain.Exceptions;

namespace Domain.Entities
{
    public class ReservationOrder
    {
        public static int MinDaysAdvanceBooking => 1;
        public static int MaxDaysAdvanceBooking => 30;
        public static int MaxStayDays => 3;

        public ReservationOrder(int id, DateTime startDate, DateTime endDate, string customerEmail,
            ReservationStatus status)
        {
            Id = id;
            StartDate = startDate;
            EndDate = endDate;
            CustomerEmail = customerEmail;
            Status = status;

            RoomId = 1; // Default room assignment for simplicity and testing purposes

            CheckStartDate();
            CheckStayDays();
        }

        public int Id { get; init; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string CustomerEmail { get; set; }
        public int StayDays => EndDate.Date.Subtract(StartDate.Date).Days;
        public ReservationStatus Status { get; set; }
        public int RoomId { get; set; }

        public void Cancel()
        {
            if (Status == ReservationStatus.Canceled)
                throw new InvalidOperationException("Reservation is already canceled.");
            if (Status == ReservationStatus.Finished)
                throw new InvalidOperationException("Finished reservations cannot be canceled.");

            Status = ReservationStatus.Canceled;
        }

        private void CheckStartDate()
        {
            var today = DateTime.Today;
            var daysInAdvance = (StartDate.Date - today).Days;
            if (daysInAdvance < MinDaysAdvanceBooking || daysInAdvance > MaxDaysAdvanceBooking)
                throw new ArgumentException($"Start date must be booked between {MinDaysAdvanceBooking} and {MaxDaysAdvanceBooking} days in advance.");
        }

        private void CheckStayDays()
        {
            var stayDays = (EndDate.Date - StartDate.Date).Days;
            if (stayDays < 1 || stayDays > MaxStayDays)
                throw new ArgumentException($"Stay duration must be between 1 and {MaxStayDays} days.");
        }

        public void UpdateDates(DateTime startDate, DateTime endDate)
        {
            if (Status == ReservationStatus.Canceled ||
                Status == ReservationStatus.Finished)
                throw new ReservationCannotBeUpdatedException();

            StartDate = startDate;
            EndDate = endDate;

            CheckStartDate();
            CheckStayDays();
        }
    }
}
