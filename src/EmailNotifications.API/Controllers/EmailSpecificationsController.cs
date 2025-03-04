using EmailNotifications.Domain.Entities;
using EmailNotifications.Domain.Enums;
using EmailNotifications.Infrastructure.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace EmailNotifications.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class EmailSpecificationsController(
    IEmailSpecificationRepository repository,
    ILogger<EmailSpecificationsController> logger)
    : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<IEnumerable<EmailSpecification>>> GetAll(CancellationToken cancellationToken)
    {
        try
        {
            var specifications = await repository.GetAllAsync(cancellationToken);
            return Ok(specifications);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error retrieving all email specifications");
            return StatusCode(500, "An error occurred while retrieving email specifications");
        }
    }

    [HttpGet("{notificationType}")]
    public async Task<ActionResult<EmailSpecification>> GetByNotificationType(NotificationType notificationType, CancellationToken cancellationToken)
    {
        var specification = await repository.GetByNotificationTypeAsync(notificationType, cancellationToken);
        if (specification == null)
        {
            return NotFound($"Email specification for notification type {notificationType} not found");
        }

        return Ok(specification);
    }

    [HttpPost]
    public async Task<ActionResult<EmailSpecification>> Create(EmailSpecification specification, CancellationToken cancellationToken)
    {
        try
        {
            var created = await repository.AddAsync(specification, cancellationToken);
            return CreatedAtAction(nameof(GetByNotificationType), new { notificationType = created.NotificationType }, created);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error creating email specification");
            return StatusCode(500, "An error occurred while creating the email specification");
        }
    }

    [HttpPut("{id}")]
    public async Task<ActionResult<EmailSpecification>> Update(Guid id, EmailSpecification specification, CancellationToken cancellationToken)
    {
        if (id != specification.Id)
        {
            return BadRequest("ID mismatch");
        }

        try
        {
            var updated = await repository.UpdateAsync(specification, cancellationToken);
            return Ok(updated);
        }
        catch (InvalidOperationException ex)
        {
            return NotFound(ex.Message);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error updating email specification");
            return StatusCode(500, "An error occurred while updating the email specification");
        }
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(Guid id, CancellationToken cancellationToken)
    {
        try
        {
            await repository.DeleteAsync(id, cancellationToken);
            return NoContent();
        }
        catch (InvalidOperationException ex)
        {
            return NotFound(ex.Message);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error deleting email specification");
            return StatusCode(500, "An error occurred while deleting the email specification");
        }
    }

    [HttpGet("{specificationId}/recipients")]
    public async Task<ActionResult<IEnumerable<EmailRecipient>>> GetRecipients(Guid specificationId, CancellationToken cancellationToken)
    {
        try
        {
            var recipients = await repository.GetRecipientsAsync(specificationId, cancellationToken);
            return Ok(recipients);
        }
        catch (InvalidOperationException ex)
        {
            return NotFound(ex.Message);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error retrieving recipients for specification {SpecificationId}", specificationId);
            return StatusCode(500, "An error occurred while retrieving recipients");
        }
    }

    [HttpPost("{specificationId}/recipients")]
    public async Task<ActionResult<EmailRecipient>> AddRecipient(Guid specificationId, EmailRecipient recipient, CancellationToken cancellationToken)
    {
        try
        {
            var added = await repository.AddRecipientAsync(specificationId, recipient, cancellationToken);
            return CreatedAtAction(nameof(GetRecipients), new { specificationId }, added);
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(ex.Message);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error adding recipient to specification {SpecificationId}", specificationId);
            return StatusCode(500, "An error occurred while adding the recipient");
        }
    }

    [HttpPut("{specificationId}/recipients")]
    public async Task<ActionResult<EmailRecipient>> UpdateRecipient(Guid specificationId, EmailRecipient recipient, CancellationToken cancellationToken)
    {
        try
        {
            var updated = await repository.UpdateRecipientAsync(specificationId, recipient, cancellationToken);
            return Ok(updated);
        }
        catch (InvalidOperationException ex)
        {
            return NotFound(ex.Message);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error updating recipient in specification {SpecificationId}", specificationId);
            return StatusCode(500, "An error occurred while updating the recipient");
        }
    }

    [HttpDelete("{specificationId}/recipients/{email}")]
    public async Task<IActionResult> DeleteRecipient(Guid specificationId, string email, CancellationToken cancellationToken)
    {
        try
        {
            await repository.DeleteRecipientAsync(specificationId, email, cancellationToken);
            return NoContent();
        }
        catch (InvalidOperationException ex)
        {
            return NotFound(ex.Message);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error deleting recipient from specification {SpecificationId}", specificationId);
            return StatusCode(500, "An error occurred while deleting the recipient");
        }
    }
} 