namespace EmailNotifications.Application.Common.Results;

public interface IResult<out T> : IResult
{
    T? Data { get; }
} 