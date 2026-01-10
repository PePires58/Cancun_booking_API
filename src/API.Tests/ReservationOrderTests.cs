using Application.Dto;
using Domain.Entities;
using Domain.Enuns;

namespace API.Tests
{
    /// <summary>
    /// Tests for reservation booking rules based on README.md requirements:
    /// - Maximum stay days is 3
    /// - Maximum booking advance is 30 days
    /// - Minimum booking advance is 1 day (cannot book same day)
    /// </summary>
    public class ReservationOrderTests
    {
        [Fact]
        public void CreateReservation_WithValidDates_ShouldSucceed()
        {
            // Arrange
            var startDate = DateTime.Today.AddDays(5);
            var endDate = startDate.AddDays(2);

            // Act
            var reservation = new ReservationOrder(
                id: 1,
                startDate: startDate,
                endDate: endDate,
                customerEmail: "test@example.com",
                status: ReservationStatus.Reserved
            );

            // Assert
            Assert.NotNull(reservation);
            Assert.Equal(startDate, reservation.StartDate);
            Assert.Equal(endDate, reservation.EndDate);
            Assert.Equal(2, reservation.StayDays);
        }

        [Fact]
        public void CreateReservation_WithMaximumStayDays_ShouldSucceed()
        {
            // Arrange - Maximum stay is 3 days
            var startDate = DateTime.Today.AddDays(5);
            var endDate = startDate.AddDays(3);

            // Act
            var reservation = new ReservationOrder(
                id: 1,
                startDate: startDate,
                endDate: endDate,
                customerEmail: "test@example.com",
                status: ReservationStatus.Reserved
            );

            // Assert
            Assert.Equal(3, reservation.StayDays);
        }

        [Fact]
        public void CreateReservation_ExceedingMaximumStayDays_ShouldThrowException()
        {
            // Arrange - Stay exceeds 3 days
            var startDate = DateTime.Today.AddDays(5);
            var endDate = startDate.AddDays(4);

            // Act & Assert
            var exception = Assert.Throws<ArgumentException>(() =>
                new ReservationOrder(
                    id: 1,
                    startDate: startDate,
                    endDate: endDate,
                    customerEmail: "test@example.com",
                    status: ReservationStatus.Reserved
                )
            );

            Assert.Contains("Stay duration must be between 1 and 3 days", exception.Message);
        }

        [Fact]
        public void CreateReservation_WithMinimumBookingAdvance_ShouldSucceed()
        {
            // Arrange - Minimum advance is 1 day
            var startDate = DateTime.Today.AddDays(1);
            var endDate = startDate.AddDays(1);

            // Act
            var reservation = new ReservationOrder(
                id: 1,
                startDate: startDate,
                endDate: endDate,
                customerEmail: "test@example.com",
                status: ReservationStatus.Reserved
            );

            // Assert
            Assert.Equal(startDate, reservation.StartDate);
        }

        [Fact]
        public void CreateReservation_BookingSameDay_ShouldThrowException()
        {
            // Arrange - Booking for same day (0 days advance)
            var startDate = DateTime.Today;
            var endDate = startDate.AddDays(1);

            // Act & Assert
            var exception = Assert.Throws<ArgumentException>(() =>
                new ReservationOrder(
                    id: 1,
                    startDate: startDate,
                    endDate: endDate,
                    customerEmail: "test@example.com",
                    status: ReservationStatus.Reserved
                )
            );

            Assert.Contains("Start date must be booked between 1 and 30 days in advance", exception.Message);
        }

        [Fact]
        public void CreateReservation_WithMaximumBookingAdvance_ShouldSucceed()
        {
            // Arrange - Maximum advance is 30 days
            var startDate = DateTime.Today.AddDays(30);
            var endDate = startDate.AddDays(1);

            // Act
            var reservation = new ReservationOrder(
                id: 1,
                startDate: startDate,
                endDate: endDate,
                customerEmail: "test@example.com",
                status: ReservationStatus.Reserved
            );

            // Assert
            Assert.Equal(startDate, reservation.StartDate);
        }

        [Fact]
        public void CreateReservation_ExceedingMaximumBookingAdvance_ShouldThrowException()
        {
            // Arrange - Booking exceeds 30 days advance
            var startDate = DateTime.Today.AddDays(31);
            var endDate = startDate.AddDays(1);

            // Act & Assert
            var exception = Assert.Throws<ArgumentException>(() =>
                new ReservationOrder(
                    id: 1,
                    startDate: startDate,
                    endDate: endDate,
                    customerEmail: "test@example.com",
                    status: ReservationStatus.Reserved
                )
            );

            Assert.Contains("Start date must be booked between 1 and 30 days in advance", exception.Message);
        }

        [Fact]
        public void CancelReservation_WithReservedStatus_ShouldSucceed()
        {
            // Arrange
            var startDate = DateTime.Today.AddDays(5);
            var endDate = startDate.AddDays(2);
            var reservation = new ReservationOrder(
                id: 1,
                startDate: startDate,
                endDate: endDate,
                customerEmail: "test@example.com",
                status: ReservationStatus.Reserved
            );

            // Act
            reservation.Cancel();

            // Assert
            Assert.Equal(ReservationStatus.Canceled, reservation.Status);
        }

        [Fact]
        public void CancelReservation_AlreadyCanceled_ShouldThrowException()
        {
            // Arrange
            var startDate = DateTime.Today.AddDays(5);
            var endDate = startDate.AddDays(2);
            var reservation = new ReservationOrder(
                id: 1,
                startDate: startDate,
                endDate: endDate,
                customerEmail: "test@example.com",
                status: ReservationStatus.Canceled
            );

            // Act & Assert
            var exception = Assert.Throws<InvalidOperationException>(() =>
                reservation.Cancel()
            );

            Assert.Contains("Reservation is already canceled", exception.Message);
        }

        [Fact]
        public void CancelReservation_FinishedStatus_ShouldThrowException()
        {
            // Arrange
            var startDate = DateTime.Today.AddDays(5);
            var endDate = startDate.AddDays(2);
            var reservation = new ReservationOrder(
                id: 1,
                startDate: startDate,
                endDate: endDate,
                customerEmail: "test@example.com",
                status: ReservationStatus.Finished
            );

            // Act & Assert
            var exception = Assert.Throws<InvalidOperationException>(() =>
                reservation.Cancel()
            );

            Assert.Contains("Finished reservations cannot be canceled", exception.Message);
        }

        [Fact]
        public void UpdateReservationDates_WithValidDates_ShouldSucceed()
        {
            // Arrange
            var startDate = DateTime.Today.AddDays(5);
            var endDate = startDate.AddDays(2);
            var reservation = new ReservationOrder(
                id: 1,
                startDate: startDate,
                endDate: endDate,
                customerEmail: "test@example.com",
                status: ReservationStatus.Reserved
            );

            var newStartDate = DateTime.Today.AddDays(10);
            var newEndDate = newStartDate.AddDays(2);

            // Act
            reservation.UpdateDates(newStartDate, newEndDate);

            // Assert
            Assert.Equal(newStartDate, reservation.StartDate);
            Assert.Equal(newEndDate, reservation.EndDate);
        }

        [Fact]
        public void UpdateReservationDates_ExceedingMaxStay_ShouldThrowException()
        {
            // Arrange
            var startDate = DateTime.Today.AddDays(5);
            var endDate = startDate.AddDays(2);
            var reservation = new ReservationOrder(
                id: 1,
                startDate: startDate,
                endDate: endDate,
                customerEmail: "test@example.com",
                status: ReservationStatus.Reserved
            );

            var newStartDate = DateTime.Today.AddDays(10);
            var newEndDate = newStartDate.AddDays(4); // Exceeds 3 days

            // Act & Assert
            var exception = Assert.Throws<ArgumentException>(() =>
                reservation.UpdateDates(newStartDate, newEndDate)
            );

            Assert.Contains("Stay duration must be between 1 and 3 days", exception.Message);
        }

        [Fact]
        public void ReservationOrderDto_MapToEntity_ShouldCreateValidEntity()
        {
            // Arrange
            var dto = new ReservationOrderDto
            {
                Id = 0,
                StartDate = DateTime.Today.AddDays(5),
                EndDate = DateTime.Today.AddDays(7),
                CustomerEmail = "test@example.com",
                Status = ReservationStatus.Reserved,
                RoomId = 1
            };

            // Act
            var entity = dto.MapToEntity();

            // Assert
            Assert.NotNull(entity);
            Assert.Equal(dto.StartDate, entity.StartDate);
            Assert.Equal(dto.EndDate, entity.EndDate);
            Assert.Equal(dto.CustomerEmail, entity.CustomerEmail);
        }
    }
}
