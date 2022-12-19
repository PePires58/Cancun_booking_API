# Modify Reservation Lambda

Responsible for modify a reservation. The lambda is triggered by the path bellow:
- POST /ModifyReservationOrder

Entry Model:
- Id: int - Required
- StartDate: DateTime - Required
- EndDate: DateTime - Required
- CustomerEmail: string - Required

JSON object example:
{
	"Id": 1
	"StartDate": "2022-12-19",
	"EndDate": "2022-12-21",
	"CustomerEmail": "Customer@email.com"
}