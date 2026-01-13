using Application.Dto;
using Application.Exceptions;
using Application.Interfaces;
using Domain.Entities;
using Infra;

namespace Application.Services
{
    public class ReservationOrderService(CancunDbContext _dbContext, 
        IRoomAvailabilityService _roomAvailabilityService)
    {
        public void CreateReservationOrder(ReservationOrderDto orderDto)
        {
            ReservationOrder reservation = orderDto.MapToEntity();

            _roomAvailabilityService.CheckAvailability(orderDto);

            _dbContext.ReservationOrders.Add(reservation);
            _dbContext.SaveChanges();

            orderDto.Id = reservation.Id;
        }

        public void UpdateReservationOrder(ReservationOrderDto orderDto)
        {
            _roomAvailabilityService.CheckAvailability(orderDto);

            ReservationOrder? reservation = _dbContext.ReservationOrders
                .FirstOrDefault(c => c.Id == orderDto.Id
                && c.CustomerEmail == orderDto.CustomerEmail) ?? throw new ReservationNotFoundException(orderDto.Id);

            reservation.UpdateDates(orderDto.StartDate, orderDto.EndDate);
            _dbContext.SaveChanges();
        }

        public void CancelReservationOrder(ReservationOrderDto orderDto)
        {
            ReservationOrder? reservation = _dbContext.ReservationOrders
                .FirstOrDefault(c => c.Id == orderDto.Id 
                && c.CustomerEmail == orderDto.CustomerEmail) ?? throw new ReservationNotFoundException(orderDto.Id);

            reservation.Cancel();
            _dbContext.SaveChanges();
        }
    }
}
