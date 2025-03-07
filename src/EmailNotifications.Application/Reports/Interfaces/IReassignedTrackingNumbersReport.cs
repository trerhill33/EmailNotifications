using System.Threading;
using System.Threading.Tasks;

namespace EmailNotifications.Application.Reports.Interfaces;

public interface IReassignedTrackingNumbersReport
{
    Task<bool> SendAsync(CancellationToken cancellationToken = default);
} 