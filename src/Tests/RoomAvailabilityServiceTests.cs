using Application.Dto;
using Application.Exceptions;
using Application.Services;
using Domain.Entities;
using Domain.Enuns;
using Infra;
using Microsoft.EntityFrameworkCore;

namespace Tests
{
    public class RoomAvailabilityServiceTests
    {
        private CancunDbContext GetInMemoryDbContext()
        {
            var options = new DbContextOptionsBuilder<CancunDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            return new CancunDbContext(options);
        }

        [Fact]
        public void CheckAvailability_WhenRoomIsAvailable_ShouldNotThrowException()
        {
            // Arrange
            var dbContext = GetInMemoryDbContext();
            var service = new RoomAvailabilityService(dbContext);
            
            var dto = new ReservationOrderDto
            {
                Id = 0,
                StartDate = DateTime.Today.AddDays(5),
                EndDate = DateTime.Today.AddDays(7),
                CustomerEmail = "test@example.com",
                RoomId = 1
            };

            // Act & Assert
            var exception = Record.Exception(() => service.CheckAvailability(dto));
            Assert.Null(exception);
        }

        [Fact]
        public void CheckAvailability_WhenRoomIsBooked_ShouldThrowException()
        {
            // Arrange
            var dbContext = GetInMemoryDbContext();
            var existingReservation = new ReservationOrder(
                id: 1,
                startDate: DateTime.Today.AddDays(5),
                endDate: DateTime.Today.AddDays(7),
                customerEmail: "existing@example.com",
                status: ReservationStatus.Reserved
            );
            dbContext.ReservationOrders.Add(existingReservation);
            dbContext.SaveChanges();

            var service = new RoomAvailabilityService(dbContext);
            
            var dto = new ReservationOrderDto
            {
                Id = 0,
                StartDate = DateTime.Today.AddDays(6),
                EndDate = DateTime.Today.AddDays(8),
                CustomerEmail = "test@example.com",
                RoomId = 1
            };

            // Act & Assert
            var exception = Assert.Throws<RoomNotAvailableException>(() => service.CheckAvailability(dto));
            Assert.Contains("Room is not available", exception.Message);
        }

        [Fact]
        public void CheckAvailability_WhenUpdatingSameReservation_ShouldNotThrowException()
        {
            // Arrange
            var dbContext = GetInMemoryDbContext();
            var existingReservation = new ReservationOrder(
                id: 1,
                startDate: DateTime.Today.AddDays(5),
                endDate: DateTime.Today.AddDays(7),
                customerEmail: "test@example.com",
                status: ReservationStatus.Reserved
            );
            dbContext.ReservationOrders.Add(existingReservation);
            dbContext.SaveChanges();

            var service = new RoomAvailabilityService(dbContext);
            
            var dto = new ReservationOrderDto
            {
                Id = 1,
                StartDate = DateTime.Today.AddDays(6),
                EndDate = DateTime.Today.AddDays(8),
                CustomerEmail = "test@example.com",
                RoomId = 1
            };

            // Act & Assert
            var exception = Record.Exception(() => service.CheckAvailability(dto));
            Assert.Null(exception);
        }

        [Fact]
        public void CheckAvailability_WhenCanceledReservationExists_ShouldNotThrowException()
        {
            // Arrange
            var dbContext = GetInMemoryDbContext();
            var canceledReservation = new ReservationOrder(
                id: 1,
                startDate: DateTime.Today.AddDays(5),
                endDate: DateTime.Today.AddDays(7),
                customerEmail: "canceled@example.com",
                status: ReservationStatus.Canceled
            );
            dbContext.ReservationOrders.Add(canceledReservation);
            dbContext.SaveChanges();

            var service = new RoomAvailabilityService(dbContext);
            
            var dto = new ReservationOrderDto
            {
                Id = 0,
                StartDate = DateTime.Today.AddDays(6),
                EndDate = DateTime.Today.AddDays(8),
                CustomerEmail = "test@example.com",
                RoomId = 1
            };

            // Act & Assert
            var exception = Record.Exception(() => service.CheckAvailability(dto));
            Assert.Null(exception);
        }

        [Fact]
        public void CheckAvailability_WhenReservationsDoNotOverlap_ShouldNotThrowException()
        {
            // Arrange
            var dbContext = GetInMemoryDbContext();
            var existingReservation = new ReservationOrder(
                id: 1,
                startDate: DateTime.Today.AddDays(5),
                endDate: DateTime.Today.AddDays(7),
                customerEmail: "existing@example.com",
                status: ReservationStatus.Reserved
            );
            dbContext.ReservationOrders.Add(existingReservation);
            dbContext.SaveChanges();

            var service = new RoomAvailabilityService(dbContext);
            
            var dto = new ReservationOrderDto
            {
                Id = 0,
                StartDate = DateTime.Today.AddDays(10),
                EndDate = DateTime.Today.AddDays(12),
                CustomerEmail = "test@example.com",
                RoomId = 1
            };

            // Act & Assert
            var exception = Record.Exception(() => service.CheckAvailability(dto));
            Assert.Null(exception);
        }
    }
}
