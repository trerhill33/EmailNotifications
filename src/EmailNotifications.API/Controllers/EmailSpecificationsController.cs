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

    [HttpPost("{specificationId}/recipient-groups")]
    public async Task<ActionResult<EmailSpecification>> AddRecipientGroup(
        Guid specificationId,
        EmailRecipientGroup group,
        CancellationToken cancellationToken)
    {
        try
        {
            var updated = await repository.AddRecipientGroupAsync(specificationId, group, cancellationToken);
            return Ok(updated);
        }
        catch (InvalidOperationException ex)
        {
            return NotFound(ex.Message);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error adding recipient group");
            return StatusCode(500, "An error occurred while adding the recipient group");
        }
    }

    [HttpPut("{specificationId}/recipient-groups/{groupId}")]
    public async Task<ActionResult<EmailSpecification>> UpdateRecipientGroup(
        Guid specificationId,
        Guid groupId,
        EmailRecipientGroup group,
        CancellationToken cancellationToken)
    {
        if (groupId != group.Id)
        {
            return BadRequest("Group ID mismatch");
        }

        try
        {
            var updated = await repository.UpdateRecipientGroupAsync(specificationId, group, cancellationToken);
            return Ok(updated);
        }
        catch (InvalidOperationException ex)
        {
            return NotFound(ex.Message);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error updating recipient group");
            return StatusCode(500, "An error occurred while updating the recipient group");
        }
    }

    [HttpDelete("{specificationId}/recipient-groups/{groupId}")]
    public async Task<ActionResult<EmailSpecification>> DeleteRecipientGroup(
        Guid specificationId,
        Guid groupId,
        CancellationToken cancellationToken)
    {
        try
        {
            var updated = await repository.DeleteRecipientGroupAsync(specificationId, groupId, cancellationToken);
            return Ok(updated);
        }
        catch (InvalidOperationException ex)
        {
            return NotFound(ex.Message);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error deleting recipient group");
            return StatusCode(500, "An error occurred while deleting the recipient group");
        }
    }
} 