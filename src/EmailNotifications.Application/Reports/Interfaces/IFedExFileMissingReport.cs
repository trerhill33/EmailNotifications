using System.Threading;
using System.Threading.Tasks;

namespace EmailNotifications.Application.Reports.Interfaces;

public interface IFedExFileMissingReport
{
    Task<bool> SendAsync(CancellationToken cancellationToken = default);
} 