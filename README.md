# Email Notifications System

A clean architecture .NET Core 8 application focused specifically on email notifications. This system provides a robust, reusable framework for sending email notifications in any .NET application.

## Features

- **Clean Architecture**: Domain, Application, Infrastructure, and API layers with clear separation of concerns
- **Type-safe Email Templates**: Strong typing for email templates and notification types
- **Factory Pattern**: Flexible notification request creation with type safety
- **Database Integration**: Entity Framework Core with SQL Server for storing email templates and logs
- **SMTP Email Sending**: Support for SMTP with retry logic for transient errors
- **Scriban Template Rendering**: Powerful template rendering with model binding and caching
- **Database-Stored Templates**: Email templates are stored in the database for easy management and customization
- **API Endpoints**: RESTful API for sending notifications

## Project Structure

- **EmailNotifications.Domain**: Core domain entities and business logic
- **EmailNotifications.Application**: Application services, interfaces, and DTOs
- **EmailNotifications.Infrastructure**: Implementation of interfaces, database context, and external services
- **EmailNotifications.API**: API controllers and configuration

## Architecture

The system follows clean architecture principles with a clear separation of concerns:

- **Domain Layer**: Contains the core business entities and logic
- **Application Layer**: Contains the use cases and business rules
- **Infrastructure Layer**: Contains the implementation details (email sending, database access, etc.)
- **API Layer**: Contains the API controllers and configuration

### Key Interfaces

- **INotificationService**: The main service for sending notifications
- **INotificationRequestFactory**: Factory for creating notification requests with type safety

### Implementation Details

The infrastructure layer contains the implementation details:

- **EmailSpecificationRepository**: Retrieves email templates from the database
- **SmtpEmailSender**: Sends emails using SMTP
- **ScribanEmailTemplateRenderer**: Renders templates using Scriban with caching
- **NotificationService**: Orchestrates the notification process

## Template System

The system uses Scriban for template rendering with the following features:

- **Database Storage**: Templates are stored in the database for easy management and customization
- **Template Wrapper**: A common wrapper template surrounds all notification content
- **Template Caching**: Templates are cached for better performance
- **Strong Typing**: Templates are rendered with strongly-typed models
- **Single Call Rendering**: The template renderer handles both the notification content and wrapper in a single call

### Template Structure

The email template system consists of two main components:

1. **Email Wrapper Template**: A static HTML template with a `{{ Content }}` placeholder where notification-specific content is inserted
2. **Notification Templates**: HTML templates stored in the database for each notification type (e.g., `NewUserCreated`, `PasswordReset`)

The wrapper template is stored in the Infrastructure layer at `Templates/email_wrapper_template.html`.

## Getting Started

### Prerequisites

- .NET 8.0 SDK
- SQL Server (or SQL Server LocalDB)

### Installation

1. Clone the repository
2. Update the connection string in `appsettings.json` if needed
3. Run the following commands:

```bash
dotnet restore
dotnet ef database update --project src/EmailNotifications.Infrastructure --startup-project src/EmailNotifications.API
dotnet run --project src/EmailNotifications.API
```

### Configuration

Configure SMTP settings in `appsettings.json`:

```json
"Smtp": {
  "Host": "your-smtp-server",
  "Port": 587,
  "EnableSsl": true,
  "Username": "your-username",
  "Password": "your-password"
}
```

## Usage

### Sending a New User Notification

```http
POST /api/notifications/new-user
Content-Type: application/json

{
  "username": "johndoe",
  "email": "john.doe@example.com",
  "fullName": "John Doe",
  "activationLink": "https://example.com/activate?token=abc123"
}
```

### Sending a Password Reset Notification

```http
POST /api/notifications/password-reset
Content-Type: application/json

{
  "username": "johndoe",
  "email": "john.doe@example.com",
  "resetLink": "https://example.com/reset-password?token=xyz789",
  "expirationHours": 24
}
```

## Extending the System

### Adding a New Notification Type

1. Add a new value to the `NotificationType` enum in `EmailNotifications.Application/Enums/NotificationType.cs`
2. Create a new `EmailSpecification` in the database with the same name as the enum value
3. Create model classes for the notification
4. Add a new endpoint in the API controller

### Using in Your Application

```csharp
// Inject the required services
private readonly INotificationService _notificationService;
private readonly INotificationRequestFactory _requestFactory;

// Create a notification request
var model = new YourNotificationModel { /* ... */ };
var request = _requestFactory.Create(NotificationType.YourNotificationType, model);

// Send the notification
await _notificationService.SendAsync(request);
```

## License

This project is licensed under the MIT License - see the LICENSE file for details. 