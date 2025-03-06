using EmailNotifications.Application.Common.Results;
using EmailNotifications.Domain.Entities;
using EmailNotifications.Domain.Enums;
using EmailNotifications.Infrastructure.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace EmailNotifications.Api.Controllers;

/// <summary>
/// Manages email notification specifications
/// </summary>
[ApiController]
[Route("api/[controller]")]
[Produces("application/json")]
[Tags("Email Specifications")]
public class EmailSpecificationsController(
    IEmailSpecificationRepository repository,
    ILogger<EmailSpecificationsController> logger)
    : ControllerBase
{
    /// <summary>
    /// Gets all email specifications
    /// </summary>
    [HttpGet]
    [ProducesResponseType(typeof(Result<IEnumerable<EmailSpecification>>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAll(CancellationToken cancellationToken)
    {
        try
        {
            var specifications = await repository.GetAllAsync(cancellationToken);
            return Ok(Result<IEnumerable<EmailSpecification>>.Success(specifications));
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error retrieving all email specifications");
            return StatusCode(500, Result.FailAsync("An error occurred while retrieving email specifications"));
        }
    }

    /// <summary>
    /// Gets an email specification by notification type
    /// </summary>
    [HttpGet("{notificationType}")]
    [ProducesResponseType(typeof(Result<EmailSpecification>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(Result), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetByNotificationType(NotificationType notificationType, CancellationToken cancellationToken)
    {
        try
        {
            var specification = await repository.GetByNotificationTypeAsync(notificationType, cancellationToken);
            if (specification == null)
            {
                return NotFound(Result.FailAsync($"Email specification for notification type {notificationType} not found", ResultStatus.NotFound));
            }
            return Ok(Result<EmailSpecification>.Success(specification));
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error retrieving email specification for notification type {NotificationType}", notificationType);
            return StatusCode(500, Result.FailAsync($"An error occurred while retrieving email specification for notification type {notificationType}"));
        }
    }

    /// <summary>
    /// Creates a new email specification
    /// </summary>
    [HttpPost]
    [ProducesResponseType(typeof(Result<EmailSpecification>), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(Result), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Create(EmailSpecification specification, CancellationToken cancellationToken)
    {
        try
        {
            var result = await repository.AddAsync(specification, cancellationToken);
            return CreatedAtAction(nameof(GetByNotificationType), new { notificationType = specification.NotificationType }, Result<EmailSpecification>.Success(result));
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error creating email specification");
            return BadRequest(Result.FailAsync("Error creating email specification: " + ex.Message));
        }
    }

    /// <summary>
    /// Updates an existing email specification
    /// </summary>
    [HttpPut("{id}")]
    [ProducesResponseType(typeof(Result<EmailSpecification>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(Result), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(Result), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Update(int id, EmailSpecification specification, CancellationToken cancellationToken)
    {
        if (id != specification.Id)
        {
            return BadRequest(Result.FailAsync("ID mismatch", ResultStatus.Error));
        }

        try
        {
            var result = await repository.UpdateAsync(specification, cancellationToken);
            return Ok(Result<EmailSpecification>.Success(result));
        }
        catch (InvalidOperationException ex)
        {
            logger.LogError(ex, "Error updating email specification with ID {Id}", id);
            return NotFound(Result.FailAsync(ex.Message, ResultStatus.NotFound));
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error updating email specification with ID {Id}", id);
            return BadRequest(Result.FailAsync("Error updating email specification: " + ex.Message));
        }
    }

    /// <summary>
    /// Deletes an email specification
    /// </summary>
    [HttpDelete("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(Result), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Delete(int id, CancellationToken cancellationToken)
    {
        try
        {
            await repository.DeleteAsync(id, cancellationToken);
            return NoContent();
        }
        catch (InvalidOperationException ex)
        {
            logger.LogError(ex, "Error deleting email specification with ID {Id}", id);
            return NotFound(Result.FailAsync(ex.Message, ResultStatus.NotFound));
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error deleting email specification with ID {Id}", id);
            return BadRequest(Result.FailAsync("Error deleting email specification: " + ex.Message));
        }
    }

    /// <summary>
    /// Gets all recipients for an email specification
    /// </summary>
    [HttpGet("{specificationId}/recipients")]
    [ProducesResponseType(typeof(Result<IEnumerable<EmailRecipient>>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(Result), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetRecipients(int specificationId, CancellationToken cancellationToken)
    {
        try
        {
            var recipients = await repository.GetRecipientsAsync(specificationId, cancellationToken);
            return Ok(Result<IEnumerable<EmailRecipient>>.Success(recipients));
        }
        catch (InvalidOperationException ex)
        {
            logger.LogError(ex, "Error retrieving recipients for email specification with ID {Id}", specificationId);
            return NotFound(Result.FailAsync(ex.Message, ResultStatus.NotFound));
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error retrieving recipients for email specification with ID {Id}", specificationId);
            return BadRequest(Result.FailAsync("Error retrieving recipients: " + ex.Message));
        }
    }

    /// <summary>
    /// Adds a recipient to an email specification
    /// </summary>
    [HttpPost("{specificationId}/recipients")]
    [ProducesResponseType(typeof(Result<EmailRecipient>), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(Result), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(Result), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> AddRecipient(int specificationId, EmailRecipient recipient, CancellationToken cancellationToken)
    {
        try
        {
            var result = await repository.AddRecipientAsync(specificationId, recipient, cancellationToken);
            return CreatedAtAction(nameof(GetRecipients), new { specificationId }, Result<EmailRecipient>.Success(result));
        }
        catch (InvalidOperationException ex)
        {
            logger.LogError(ex, "Error adding recipient to email specification with ID {Id}", specificationId);
            return NotFound(Result.FailAsync(ex.Message, ResultStatus.NotFound));
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error adding recipient to email specification with ID {Id}", specificationId);
            return BadRequest(Result.FailAsync("Error adding recipient: " + ex.Message));
        }
    }

    /// <summary>
    /// Updates a recipient in an email specification
    /// </summary>
    [HttpPut("{specificationId}/recipients")]
    [ProducesResponseType(typeof(Result<EmailRecipient>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(Result), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(Result), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> UpdateRecipient(int specificationId, EmailRecipient recipient, CancellationToken cancellationToken)
    {
        try
        {
            var result = await repository.UpdateRecipientAsync(specificationId, recipient, cancellationToken);
            return Ok(Result<EmailRecipient>.Success(result));
        }
        catch (InvalidOperationException ex)
        {
            logger.LogError(ex, "Error updating recipient in email specification with ID {Id}", specificationId);
            return NotFound(Result.FailAsync(ex.Message, ResultStatus.NotFound));
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error updating recipient in email specification with ID {Id}", specificationId);
            return BadRequest(Result.FailAsync("Error updating recipient: " + ex.Message));
        }
    }

    /// <summary>
    /// Deletes a recipient from an email specification
    /// </summary>
    [HttpDelete("{specificationId}/recipients/{email}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(Result), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> DeleteRecipient(int specificationId, string email, CancellationToken cancellationToken)
    {
        try
        {
            await repository.DeleteRecipientAsync(specificationId, email, cancellationToken);
            return NoContent();
        }
        catch (InvalidOperationException ex)
        {
            logger.LogError(ex, "Error deleting recipient from email specification with ID {Id}", specificationId);
            return NotFound(Result.FailAsync(ex.Message, ResultStatus.NotFound));
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error deleting recipient from email specification with ID {Id}", specificationId);
            return BadRequest(Result.FailAsync("Error deleting recipient: " + ex.Message));
        }
    }
} 