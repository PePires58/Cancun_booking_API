using Application.Dto;
using Application.Services;
using Domain.Enuns;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ReservationOrdersController : ControllerBase
    {
        private readonly ReservationOrderService _reservationOrderService;
        private readonly ILogger<ReservationOrdersController> _logger;

        public ReservationOrdersController(
            ReservationOrderService reservationOrderService,
            ILogger<ReservationOrdersController> logger)
        {
            _reservationOrderService = reservationOrderService;
            _logger = logger;
        }

        /// <summary>
        /// Creates a new reservation order
        /// </summary>
        /// <param name="orderDto">Reservation order details</param>
        /// <returns>Created reservation order</returns>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public IActionResult CreateReservation([FromBody] ReservationOrderDto orderDto)
        {
            try
            {
                orderDto.Status = ReservationStatus.Reserved;
                _reservationOrderService.CreateReservationOrder(orderDto);
                _logger.LogInformation("Reservation created for customer: {Email}", orderDto.CustomerEmail);
                return CreatedAtAction(nameof(CreateReservation), new { id = orderDto.Id }, orderDto);
            }
            catch (ArgumentException ex)
            {
                _logger.LogWarning("Invalid reservation request: {Message}", ex.Message);
                return BadRequest(new { error = ex.Message });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating reservation");
                return BadRequest(new { error = ex.Message });
            }
        }

        /// <summary>
        /// Updates an existing reservation order
        /// </summary>
        /// <param name="orderDto">Updated reservation order details</param>
        /// <returns>No content on success</returns>
        [HttpPut]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult UpdateReservation([FromBody] ReservationOrderDto orderDto)
        {
            try
            {
                _reservationOrderService.UpdateReservationOrder(orderDto);
                _logger.LogInformation("Reservation {Id} updated for customer: {Email}", orderDto.Id, orderDto.CustomerEmail);
                return NoContent();
            }
            catch (ArgumentException ex)
            {
                _logger.LogWarning("Invalid reservation update: {Message}", ex.Message);
                return BadRequest(new { error = ex.Message });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating reservation");
                return BadRequest(new { error = ex.Message });
            }
        }

        /// <summary>
        /// Cancels a reservation order
        /// </summary>
        /// <param name="id">Reservation ID from header</param>
        /// <param name="email">Customer email from header</param>
        /// <returns>No content on success</returns>
        [HttpDelete]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult CancelReservation(
            [FromHeader(Name = "X-Reservation-Id")] int id,
            [FromHeader(Name = "X-Customer-Email")] string email)
        {
            try
            {
                var cancelDto = new CancelReservationDto
                {
                    Id = id,
                    CustomerEmail = email
                };

                var orderDto = new ReservationOrderDto
                {
                    Id = cancelDto.Id,
                    CustomerEmail = cancelDto.CustomerEmail
                };

                _reservationOrderService.CancelReservationOrder(orderDto);
                _logger.LogInformation("Reservation {Id} canceled for customer: {Email}", id, email);
                return NoContent();
            }
            catch (InvalidOperationException ex)
            {
                _logger.LogWarning("Cannot cancel reservation: {Message}", ex.Message);
                return BadRequest(new { error = ex.Message });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error canceling reservation");
                return BadRequest(new { error = ex.Message });
            }
        }
    }
}
