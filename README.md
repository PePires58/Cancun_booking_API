# CANCUN Booking API

About the reservations:
- The maximum stay days is three
- The maximum booking advance is 30 days
- The minimum booking advance is one day, you cannot booking a room for the same day

About the end-users routes:
- POST /ReservationOrder
- POST /CancelReservationOrder
- POST /ModifyReservationOrder
- POST /CheckRoomAvailability 

All the details about each lambda are inside it's readme.md file.

About the project:
- Made with .NET 6 using TDD methodology
- Used GitHub actions for CI/CD with scripting commands
- API Gateway
- One Lambda per endpoint in order to isolate each part
- Cloud Provider is AWS
- Database is MySQL with Aurora Serverless Cluster

To ensure that the solution will be high available and cost optimized, all the services are "serverless" and with region scope, that means if one AZ fall out, the availability of the solution will not be impacted. Also the database is serverless with Aurora (database optimized for cloud solutions with AWS).

