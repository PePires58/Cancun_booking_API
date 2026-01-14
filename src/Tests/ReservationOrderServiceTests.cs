using Application.Dto;
using Application.Exceptions;
using Application.Interfaces;
using Application.Services;
using Domain.Entities;
using Domain.Enuns;
using Infra;
using Microsoft.EntityFrameworkCore;
using Moq;

namespace Tests
{
    public class ReservationOrderServiceTests
    {
        private CancunDbContext GetInMemoryDbContext()
        {
            var options = new DbContextOptionsBuilder<CancunDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            return new CancunDbContext(options);
        }

        [Fact]
        public void CreateReservationOrder_WithValidData_ShouldSucceed()
        {
            // Arrange
            var dbContext = GetInMemoryDbContext();
            var roomAvailabilityService = new Mock<IRoomAvailabilityService>();
            roomAvailabilityService.Setup(x => x.CheckAvailability(It.IsAny<ReservationOrderDto>()));
            
            var service = new ReservationOrderService(dbContext, roomAvailabilityService.Object);
            
            var dto = new ReservationOrderDto
            {
                StartDate = DateTime.Today.AddDays(5),
                EndDate = DateTime.Today.AddDays(7),
                CustomerEmail = "test@example.com",
                Status = ReservationStatus.Reserved
            };

            // Act
            service.CreateReservationOrder(dto);

            // Assert
            var reservations = dbContext.ReservationOrders.ToList();
            Assert.Single(reservations);
            Assert.Equal("test@example.com", reservations[0].CustomerEmail);
            roomAvailabilityService.Verify(x => x.CheckAvailability(dto), Times.Once);
        }

        [Fact]
        public void UpdateReservationOrder_WithValidData_ShouldSucceed()
        {
            // Arrange
            var dbContext = GetInMemoryDbContext();
            var startDate = DateTime.Today.AddDays(5);
            var endDate = startDate.AddDays(2);
            
            var reservation = new ReservationOrder(
                id: 1,
                startDate: startDate,
                endDate: endDate,
                customerEmail: "test@example.com",
                status: ReservationStatus.Reserved
            );
            dbContext.ReservationOrders.Add(reservation);
            dbContext.SaveChanges();

            var roomAvailabilityService = new Mock<IRoomAvailabilityService>();
            roomAvailabilityService.Setup(x => x.CheckAvailability(It.IsAny<ReservationOrderDto>()));
            
            var service = new ReservationOrderService(dbContext, roomAvailabilityService.Object);
            
            var newStartDate = DateTime.Today.AddDays(10);
            var newEndDate = newStartDate.AddDays(2);
            var dto = new ReservationOrderDto
            {
                Id = 1,
                StartDate = newStartDate,
                EndDate = newEndDate,
                CustomerEmail = "test@example.com"
            };

            // Act
            service.UpdateReservationOrder(dto);

            // Assert
            var updatedReservation = dbContext.ReservationOrders.Find(1);
            Assert.NotNull(updatedReservation);
            Assert.Equal(newStartDate, updatedReservation.StartDate);
            Assert.Equal(newEndDate, updatedReservation.EndDate);
            roomAvailabilityService.Verify(x => x.CheckAvailability(dto), Times.Once);
        }

        [Fact]
        public void UpdateReservationOrder_WhenNotFound_ShouldThrowException()
        {
            // Arrange
            var dbContext = GetInMemoryDbContext();
            var roomAvailabilityService = new Mock<IRoomAvailabilityService>();
            roomAvailabilityService.Setup(x => x.CheckAvailability(It.IsAny<ReservationOrderDto>()));
            
            var service = new ReservationOrderService(dbContext, roomAvailabilityService.Object);
            
            var dto = new ReservationOrderDto
            {
                Id = 999,
                StartDate = DateTime.Today.AddDays(10),
                EndDate = DateTime.Today.AddDays(12),
                CustomerEmail = "test@example.com"
            };

            // Act & Assert
            var exception = Assert.Throws<ReservationNotFoundException>(() => 
                service.UpdateReservationOrder(dto)
            );
            
            Assert.Contains("999", exception.Message);
        }

        [Fact]
        public void CancelReservationOrder_WithValidData_ShouldSucceed()
        {
            // Arrange
            var dbContext = GetInMemoryDbContext();
            var startDate = DateTime.Today.AddDays(5);
            var endDate = startDate.AddDays(2);
            
            var reservation = new ReservationOrder(
                id: 1,
                startDate: startDate,
                endDate: endDate,
                customerEmail: "test@example.com",
                status: ReservationStatus.Reserved
            );
            dbContext.ReservationOrders.Add(reservation);
            dbContext.SaveChanges();

            var roomAvailabilityService = new Mock<IRoomAvailabilityService>();
            var service = new ReservationOrderService(dbContext, roomAvailabilityService.Object);
            
            var dto = new ReservationOrderDto
            {
                Id = 1,
                CustomerEmail = "test@example.com"
            };

            // Act
            service.CancelReservationOrder(dto);

            // Assert
            var canceledReservation = dbContext.ReservationOrders.Find(1);
            Assert.NotNull(canceledReservation);
            Assert.Equal(ReservationStatus.Canceled, canceledReservation.Status);
        }

        [Fact]
        public void CancelReservationOrder_WhenNotFound_ShouldThrowException()
        {
            // Arrange
            var dbContext = GetInMemoryDbContext();
            var roomAvailabilityService = new Mock<IRoomAvailabilityService>();
            var service = new ReservationOrderService(dbContext, roomAvailabilityService.Object);
            
            var dto = new ReservationOrderDto
            {
                Id = 999,
                CustomerEmail = "test@example.com"
            };

            // Act & Assert
            var exception = Assert.Throws<ReservationNotFoundException>(() => 
                service.CancelReservationOrder(dto)
            );
            
            Assert.Contains("999", exception.Message);
        }

        [Fact]
        public void CreateReservationOrder_WhenRoomNotAvailable_ShouldThrowException()
        {
            // Arrange
            var dbContext = GetInMemoryDbContext();
            var roomAvailabilityService = new Mock<IRoomAvailabilityService>();
            roomAvailabilityService
                .Setup(x => x.CheckAvailability(It.IsAny<ReservationOrderDto>()))
                .Throws(new RoomNotAvailableException());
            
            var service = new ReservationOrderService(dbContext, roomAvailabilityService.Object);
            
            var dto = new ReservationOrderDto
            {
                StartDate = DateTime.Today.AddDays(5),
                EndDate = DateTime.Today.AddDays(7),
                CustomerEmail = "test@example.com",
                Status = ReservationStatus.Reserved
            };

            // Act & Assert
            var exception = Assert.Throws<RoomNotAvailableException>(() => 
                service.CreateReservationOrder(dto)
            );
            
            Assert.Contains("Room is not available", exception.Message);
        }
    }
}
