# Check Room Availability Lambda

Responsible for check room availability. The lambda is triggered by the path bellow:
- POST /CheckRoomAvailability

Entry Model:
- StartDate: DateTime - Required
- EndDate: DateTime - Required
- CustomerEmail: string - Required

JSON object example:
{
	"StartDate": "2022-12-19",
	"EndDate": "2022-12-21",
	"CustomerEmail": "Customer@email.com"
}