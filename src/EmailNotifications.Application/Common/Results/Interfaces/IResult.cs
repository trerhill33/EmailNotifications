using System.Collections.Generic;

namespace EmailNotifications.Application.Common.Results;

public interface IResult
{
    List<string> Messages { get; set; }
    bool Succeeded { get; set; }
    ResultStatus Status { get; set; }
} 