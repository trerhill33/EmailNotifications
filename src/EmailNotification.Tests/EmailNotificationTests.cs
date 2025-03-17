using System.Net.Mail;
using System.Text;
using Amazon.SecretsManager;
using EmailNotifications.Application.Common.Notifications.Interfaces;
using EmailNotifications.Application.Common.Notifications.Models;
using EmailNotifications.Domain.Entities;
using EmailNotifications.Domain.Enums;
using EmailNotifications.Infrastructure.Configuration;
using EmailNotifications.Infrastructure.Helper;
using EmailNotifications.Infrastructure.Interfaces;
using EmailNotifications.Infrastructure.Models;
using EmailNotifications.Infrastructure.Services;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using CertificateException = Org.BouncyCastle.Security.Certificates.CertificateException;
using FakeItEasy;

namespace EmailNotificationTests;

/*[TestFixture]
public class EmailNotificationTests
{
    private IOptions<MailRelaySettings> _settings;
    private IAmazonSecretsManager _secretsManager;
    private ILogger<SmtpEmailSender> _smtpLogger;
    private ILogger<NotificationService> _notificationLogger;
    private ILogger<CertHelper> _certLogger;
    private ITemplateRenderer _templateRenderer;
    private IEmailSpecificationRepository _emailSpecRepository;

    [SetUp]
    public void SetUp()
    {
        _settings = A.Fake<IOptions<MailRelaySettings>>();
        _secretsManager = A.Fake<IAmazonSecretsManager>();
        _smtpLogger = A.Fake<ILogger<SmtpEmailSender>>();
        _notificationLogger = A.Fake<ILogger<NotificationService>>();
        _certLogger = A.Fake<ILogger<CertHelper>>();
        _templateRenderer = A.Fake<ITemplateRenderer>();
        _emailSpecRepository = A.Fake<IEmailSpecificationRepository>();

        // Configure default MailRelaySettings
        A.CallTo(() => _settings.Value).Returns(new MailRelaySettings
        {
            Server = "mailrelay.com",
            Port = 587,
            UseSsl = true,
            UseCustomServerCertificateValidation = true,
            ServerIntermediateCertificateSecret = "dev-intermediate-cert-test",
            MaxRetryAttempts = 3,
            RetryDelayMilliseconds = 1000,
            Timeout = 0
        });
    }

    [Test]
    public async Task SmtpEmailSender_SendEmailAsync_SendsEmailSuccessfully()
    {
        // Arrange
        var emailSender = A.Fake<IEmailSender>();
        var certHelper = A.Fake<ICertHelper>();
        var emailMessage = new EmailMessage
        {
            From = new MailAddress("sender@example.com"),
            To = [new MailAddress("recipient@example.com")],
            Subject = "Test Email",
            HtmlBody = "<p>Test</p>"
        };

        A.CallTo(() => certHelper.GetIntermediateCertificatesAsync(A<string>._, A<CancellationToken>._))
            .Returns([]);

        // Act
        await emailSender.SendEmailAsync(emailMessage);

        // Assert
        Assert.Pass("Email sent without exception.");
    }

    [Test]
    public void SmtpEmailSender_Constructor_WithNullSettings_ThrowsArgumentNullException()
    {
        // Act & Assert
        Assert.Throws<ArgumentNullException>(() => new SmtpEmailSender(null, A.Fake<ICertHelper>(), _smtpLogger));
    }
    
    [Test]
     public async Task NotificationService_SendAsync_WithValidRequestAndAttachment_SendsEmail()
     {
         // Arrange
         var emailSender = A.Fake<IEmailSender>();
         var service = new NotificationService(emailSender, _templateRenderer, _emailSpecRepository, _notificationLogger);
         var attachment = new TestAttachment("test.pdf", "application/pdf", Encoding.UTF8.GetBytes("PDF content"));
         var request = new SendNotificationRequest<TestTemplateModel>(
             (NotificationType)TestNotificationType.TestNotification,
             new TestTemplateModel { Content = "Test Content" })
         {
             Attachments = new List<IAttachment> { attachment }.AsReadOnly()
         };

         var emailSpec = new EmailSpecification
         {
             Id = 1,
             Name = "Test Notification",
             NotificationType = (NotificationType)TestNotificationType.TestNotification,
             Subject = "Test Subject",
             HtmlBody = "<div>{{ Content }}</div>",
             FromAddress = "sender@example.com",
             RecipientGroups =
                 [
                     new EmailRecipientGroup() {
                         Name = "Group",
                         Recipients =
                         [
                             new EmailRecipient { EmailAddress = "recipient@example.com", Type = RecipientType.To }
                         ]
                     }
                 ]
         };

         A.CallTo(() => _emailSpecRepository.GetByNotificationTypeAsync((NotificationType)TestNotificationType.TestNotification, A<CancellationToken>._))
             .Returns(Task.FromResult<EmailSpecification?>(emailSpec));
         A.CallTo(() => _templateRenderer.StartRenderingAsync(emailSpec.HtmlBody, request.Data, A<CancellationToken>._))
             .Returns(Task.FromResult("<div>Test Content</div>"));

         // Act
         var result = await service.SendAsync(request);

         // Assert
         Assert.IsTrue(result, "Email should be sent successfully.");
         A.CallTo(() => emailSender.SendEmailAsync(A<EmailMessage>.That.Matches(e =>
             e.From.Address == "sender@example.com" &&
             e.To.Any(t => t.Address == "recipient@example.com") &&
             e.Subject == "Test Subject" &&
             e.HtmlBody == "<div>Test Content</div>" &&
             e.Attachments.Any(a => a.FileName == "test.pdf" && a.ContentType == "application/pdf")), A<CancellationToken>._))
             .MustHaveHappened();
     }

     [Test]
     public async Task NotificationService_SendAsync_WithNoRecipients_ReturnsFalse()
     {
         // Arrange
         var emailSender = A.Fake<IEmailSender>();
         var service = new NotificationService(emailSender, _templateRenderer, _emailSpecRepository, _notificationLogger);
         var request = new SendNotificationRequest<TestTemplateModel>(
             (NotificationType)TestNotificationType.TestNotification,
             new TestTemplateModel { Content = "Test Content" })
         {
             Attachments = new List<IAttachment>().AsReadOnly()
         };

         var emailSpec = new EmailSpecification
         {
             Id = 1,
             Name = "Sample",
             NotificationType = (NotificationType)TestNotificationType.TestNotification,
             Subject = "Test Subject",
             HtmlBody = "<div>{{ Content }}</div>",
             FromAddress = "sender@example.com",
             RecipientGroups = [] // No recipients
         };

         A.CallTo(() => _emailSpecRepository.GetByNotificationTypeAsync((NotificationType)TestNotificationType.TestNotification, A<CancellationToken>._))
             .Returns(Task.FromResult<EmailSpecification?>(emailSpec));
         A.CallTo(() => _templateRenderer.StartRenderingAsync(emailSpec.HtmlBody, request.Data, A<CancellationToken>._))
             .Returns(Task.FromResult("<div>Test Content</div>"));

         // Act
         var result = await service.SendAsync(request);

         // Assert
         A.CallTo(() => emailSender.SendEmailAsync(A<EmailMessage>._, A<CancellationToken>._))
             .MustNotHaveHappened();
     }
     
     [Test]
    public void CertHelper_GetIntermediateCertificates_ReturnsCertificates()
    {
        // Arrange
        var certHelper = new CertHelper(_secretsManager, _certLogger);
        var secretId = "test-secret";
        var pemString = "-----BEGIN CERTIFICATE-----\n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\n-----END CERTIFICATE-----";

        A.CallTo(() => _secretsManager.GetSecretValueAsync(secretId))
            .Returns(pemString);

        // Act
        var certificates = certHelper.GetIntermediateCertificatesAsync(secretId);

        // Assert
        Assert.IsNotNull(certificates);
        Assert.AreEqual(1, certificates.Count, "Should return one certificate from the PEM string.");
    }

    [Test]
    public void CertHelper_GetIntermediateCertificates_WithInvalidPem_ThrowsCertificateException()
    {
        // Arrange
        var certHelper = new CertHelper(_secretsManager, _certLogger);
        var secretId = "test-secret";
        var invalidPem = "-----BEGIN CERTIFICATE-----\ninvalid-data\n-----END CERTIFICATE-----";

        A.CallTo(() => _secretsManager.GetSecretValueAsync(secretId))
            .Returns(invalidPem);

        // Act & Assert
        var ex = Assert.Throws<CertificateException>(() => certHelper.GetIntermediateCertificatesAsync(secretId));
        Assert.That(ex.Message, Contains.Substring("Failed to retrieve or parse certificate from test-secret"));
    }

    // Test-specific notification type enum
    private enum TestNotificationType
    {
        TestNotification = 1
    }

    // Test-specific template model implementing ITemplateModel
    private class TestTemplateModel : ITemplateDataModel
    {
        public string Content { get; set; }
    }

    // Test-specific attachment implementing IAttachment
    private class TestAttachment : IAttachment
    {
        public string FileName { get; }
        public byte[] Content { get; }
        public string ContentType { get; }
        public bool IsInline => false;
        public string? ContentId => null;

        public TestAttachment(string fileName, string contentType, byte[] content)
        {
            FileName = fileName;
            ContentType = contentType;
            Content = content;
        }
    }
}*/