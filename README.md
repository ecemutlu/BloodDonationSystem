# BloodDonation
ER Diagram:![ERBloodDonation](https://github.com/ecemutlu/BloodDonationSystem/assets/109739236/55256414-9494-423e-9178-a45b28f314ba)

Donor information is captured through the UI, stored in the database via the API.
Blood donations are recorded, and donors are notified through emails using the EmailService.
Requesters submit blood requests through the UI, which are processed by the API.
Requesters are informed of request status via emails using the EmailService.

Assumptions:

Assumes that Azure CDN is set up for efficiently storing and serving donor photos.
Assumes a working email configuration for sending notifications.
Assumes the availability of email templates for donor notifications and requester notifications.
Assumes proper security measures are implemented for both API and UI.
Assumes a well-designed database schema and that database operations are handled by the Entity Framework.

Issues Encountered:

Setting up and configuring Azure CDN for storing and serving donor photos might require careful consideration and troubleshooting.
Integrating and testing the email service for donor and requester notifications could be challenging.
Ensuring consistency between data stored in the API's database and any external services (e.g., Azure CDN) is critical.
Designing a seamless user experience for capturing donor information, blood donations, and blood requests can be complex.
Implementing robust error handling mechanisms to deal with potential issues in API calls, email delivery failures, or database operations.
Ensuring that the system can scale with increased usage, especially when dealing with a potentially large number of donors and requests.
Comprehensive testing is required to validate the functionality of both the API and UI components.
Documenting the API endpoints, data models, and usage instructions for developers and users is essential for the project's maintainability.
