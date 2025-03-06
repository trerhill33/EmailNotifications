using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using EmailNotifications.Api.Models;
using EmailNotifications.Application.Common.Results;
using EmailNotifications.Domain.Entities;
using EmailNotifications.Domain.Enums;
using EmailNotifications.Infrastructure.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace EmailNotifications.Api.Controllers;

/// <summary>
/// Manages email notification specifications
/// </summary>
[ApiController]
[Route("api/[controller]")]
[Produces("application/json")]
[Tags("Email Specifications")]
public class EmailSpecificationsController : ControllerBase
{
    private readonly IEmailSpecificationRepository _repository;
    private readonly ILogger<EmailSpecificationsController> _logger;

    /// <summary>
    /// Initializes a new instance of the <see cref="EmailSpecificationsController"/> class
    /// </summary>
    /// <param name="repository">The email specification repository</param>
    /// <param name="logger">The logger</param>
    public EmailSpecificationsController(
        IEmailSpecificationRepository repository,
        ILogger<EmailSpecificationsController> logger)
    {
        _repository = repository;
        _logger = logger;
    }

    /// <summary>
    /// Gets all email specifications
    /// </summary>
    /// <returns>A list of all email specifications</returns>
    /// <response code="200">Returns the list of email specifications</response>
    [HttpGet]
    [ProducesResponseType(typeof(Result<IEnumerable<EmailSpecification>>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAll(CancellationToken cancellationToken)
    {
        try
        {
            var specifications = await _repository.GetAllAsync(cancellationToken);
            return Ok(Result<IEnumerable<EmailSpecification>>.Success(specifications));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving all email specifications");
            return StatusCode(500, Result.Failure("An error occurred while retrieving email specifications"));
        }
    }

    /// <summary>
    /// Gets an email specification by notification type
    /// </summary>
    /// <param name="notificationType">The notification type</param>
    /// <returns>The email specification if found</returns>
    /// <response code="200">Returns the email specification</response>
    /// <response code="404">If the email specification is not found</response>
    [HttpGet("{notificationType}")]
    [ProducesResponseType(typeof(Result<EmailSpecification>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(Result), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetByNotificationType(NotificationType notificationType, CancellationToken cancellationToken)
    {
        try
        {
            var specification = await _repository.GetByNotificationTypeAsync(notificationType, cancellationToken);
            if (specification == null)
            {
                return NotFound(Result.Failure($"Email specification for notification type {notificationType} not found", ResultStatus.NotFound));
            }
            return Ok(Result<EmailSpecification>.Success(specification));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving email specification for notification type {NotificationType}", notificationType);
            return StatusCode(500, Result.Failure($"An error occurred while retrieving email specification for notification type {notificationType}"));
        }
    }

    /// <summary>
    /// Creates a new email specification
    /// </summary>
    /// <param name="specification">The email specification to create</param>
    /// <returns>The created email specification</returns>
    /// <response code="201">Returns the created email specification</response>
    /// <response code="400">If the specification is invalid</response>
    [HttpPost]
    [ProducesResponseType(typeof(Result<EmailSpecification>), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(Result), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Create(EmailSpecification specification, CancellationToken cancellationToken)
    {
        try
        {
            var result = await _repository.AddAsync(specification, cancellationToken);
            return CreatedAtAction(nameof(GetByNotificationType), new { notificationType = specification.NotificationType }, Result<EmailSpecification>.Success(result));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating email specification");
            return BadRequest(Result.Failure("Error creating email specification: " + ex.Message));
        }
    }

    /// <summary>
    /// Updates an existing email specification
    /// </summary>
    /// <param name="id">The ID of the email specification to update</param>
    /// <param name="specification">The updated email specification</param>
    /// <returns>The updated email specification</returns>
    /// <response code="200">Returns the updated email specification</response>
    /// <response code="400">If the specification is invalid</response>
    /// <response code="404">If the email specification is not found</response>
    [HttpPut("{id}")]
    [ProducesResponseType(typeof(Result<EmailSpecification>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(Result), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(Result), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Update(int id, EmailSpecification specification, CancellationToken cancellationToken)
    {
        if (id != specification.Id)
        {
            return BadRequest(Result.Failure("ID mismatch", ResultStatus.BadRequest));
        }

        try
        {
            var result = await _repository.UpdateAsync(specification, cancellationToken);
            return Ok(Result<EmailSpecification>.Success(result));
        }
        catch (InvalidOperationException ex)
        {
            _logger.LogError(ex, "Error updating email specification with ID {Id}", id);
            return NotFound(Result.Failure(ex.Message, ResultStatus.NotFound));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error updating email specification with ID {Id}", id);
            return BadRequest(Result.Failure("Error updating email specification: " + ex.Message));
        }
    }

    /// <summary>
    /// Deletes an email specification
    /// </summary>
    /// <param name="id">The ID of the email specification to delete</param>
    /// <returns>No content</returns>
    /// <response code="204">If the email specification was deleted successfully</response>
    /// <response code="404">If the email specification is not found</response>
    [HttpDelete("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(Result), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Delete(int id, CancellationToken cancellationToken)
    {
        try
        {
            await _repository.DeleteAsync(id, cancellationToken);
            return NoContent();
        }
        catch (InvalidOperationException ex)
        {
            _logger.LogError(ex, "Error deleting email specification with ID {Id}", id);
            return NotFound(Result.Failure(ex.Message, ResultStatus.NotFound));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error deleting email specification with ID {Id}", id);
            return BadRequest(Result.Failure("Error deleting email specification: " + ex.Message));
        }
    }

    /// <summary>
    /// Gets all recipients for an email specification
    /// </summary>
    /// <param name="specificationId">The ID of the email specification</param>
    /// <returns>A list of all recipients for the email specification</returns>
    /// <response code="200">Returns the list of recipients</response>
    /// <response code="404">If the email specification is not found</response>
    [HttpGet("{specificationId}/recipients")]
    [ProducesResponseType(typeof(Result<IEnumerable<EmailRecipient>>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(Result), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetRecipients(int specificationId, CancellationToken cancellationToken)
    {
        try
        {
            var recipients = await _repository.GetRecipientsAsync(specificationId, cancellationToken);
            return Ok(Result<IEnumerable<EmailRecipient>>.Success(recipients));
        }
        catch (InvalidOperationException ex)
        {
            _logger.LogError(ex, "Error retrieving recipients for email specification with ID {Id}", specificationId);
            return NotFound(Result.Failure(ex.Message, ResultStatus.NotFound));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving recipients for email specification with ID {Id}", specificationId);
            return BadRequest(Result.Failure("Error retrieving recipients: " + ex.Message));
        }
    }

    /// <summary>
    /// Adds a recipient to an email specification
    /// </summary>
    /// <param name="specificationId">The ID of the email specification</param>
    /// <param name="recipient">The recipient to add</param>
    /// <returns>The added recipient</returns>
    /// <response code="201">Returns the added recipient</response>
    /// <response code="400">If the recipient is invalid</response>
    /// <response code="404">If the email specification is not found</response>
    [HttpPost("{specificationId}/recipients")]
    [ProducesResponseType(typeof(Result<EmailRecipient>), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(Result), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(Result), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> AddRecipient(int specificationId, EmailRecipient recipient, CancellationToken cancellationToken)
    {
        try
        {
            var result = await _repository.AddRecipientAsync(specificationId, recipient, cancellationToken);
            return CreatedAtAction(nameof(GetRecipients), new { specificationId }, Result<EmailRecipient>.Success(result));
        }
        catch (InvalidOperationException ex)
        {
            _logger.LogError(ex, "Error adding recipient to email specification with ID {Id}", specificationId);
            return NotFound(Result.Failure(ex.Message, ResultStatus.NotFound));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error adding recipient to email specification with ID {Id}", specificationId);
            return BadRequest(Result.Failure("Error adding recipient: " + ex.Message));
        }
    }

    /// <summary>
    /// Updates a recipient in an email specification
    /// </summary>
    /// <param name="specificationId">The ID of the email specification</param>
    /// <param name="recipient">The updated recipient</param>
    /// <returns>The updated recipient</returns>
    /// <response code="200">Returns the updated recipient</response>
    /// <response code="400">If the recipient is invalid</response>
    /// <response code="404">If the email specification or recipient is not found</response>
    [HttpPut("{specificationId}/recipients")]
    [ProducesResponseType(typeof(Result<EmailRecipient>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(Result), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(Result), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> UpdateRecipient(int specificationId, EmailRecipient recipient, CancellationToken cancellationToken)
    {
        try
        {
            var result = await _repository.UpdateRecipientAsync(specificationId, recipient, cancellationToken);
            return Ok(Result<EmailRecipient>.Success(result));
        }
        catch (InvalidOperationException ex)
        {
            _logger.LogError(ex, "Error updating recipient in email specification with ID {Id}", specificationId);
            return NotFound(Result.Failure(ex.Message, ResultStatus.NotFound));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error updating recipient in email specification with ID {Id}", specificationId);
            return BadRequest(Result.Failure("Error updating recipient: " + ex.Message));
        }
    }

    /// <summary>
    /// Deletes a recipient from an email specification
    /// </summary>
    /// <param name="specificationId">The ID of the email specification</param>
    /// <param name="email">The email of the recipient to delete</param>
    /// <returns>No content</returns>
    /// <response code="204">If the recipient was deleted successfully</response>
    /// <response code="404">If the email specification or recipient is not found</response>
    [HttpDelete("{specificationId}/recipients/{email}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(Result), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> DeleteRecipient(int specificationId, string email, CancellationToken cancellationToken)
    {
        try
        {
            await _repository.DeleteRecipientAsync(specificationId, email, cancellationToken);
            return NoContent();
        }
        catch (InvalidOperationException ex)
        {
            _logger.LogError(ex, "Error deleting recipient from email specification with ID {Id}", specificationId);
            return NotFound(Result.Failure(ex.Message, ResultStatus.NotFound));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error deleting recipient from email specification with ID {Id}", specificationId);
            return BadRequest(Result.Failure("Error deleting recipient: " + ex.Message));
        }
    }
} 