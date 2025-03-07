using System.Threading;
using System.Threading.Tasks;

namespace EmailNotifications.Application.Reports.Interfaces;

public interface IDelayedInvoicesReport
{
    Task<bool> SendAsync(CancellationToken cancellationToken = default);
} 