# Cancel Reservation Lambda

Responsible for cancel a reservation. The lambda is triggered by the path bellow:
- POST /CancelReservationOrder

Entry Model:
- ReservationId: int - Required
- CustomerEmail: string - Required

JSON object example:
{
	"ReservationId": 1
	"CustomerEmail": "Customer@email.com"
}