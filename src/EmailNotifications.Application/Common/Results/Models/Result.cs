using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace EmailNotifications.Application.Common.Results;

public class Result : IResult
{
    public List<string> Messages { get; set; } = new List<string>();
    public bool Succeeded { get; set; }
    public ResultStatus Status { get; set; }

    public static IResult Fail(ResultStatus status = ResultStatus.Error)
    {
        return new Result
        {
            Succeeded = false,
            Status = status
        };
    }

    public static IResult Fail(string message, ResultStatus status = ResultStatus.Error)
    {
        Result obj = new Result
        {
            Succeeded = false
        };
        int num = 1;
        List<string> list = new List<string>(num);
        CollectionsMarshal.SetCount(list, num);
        Span<string> span = CollectionsMarshal.AsSpan(list);
        int num2 = 0;
        span[num2] = message;
        num2++;
        obj.Messages = list;
        obj.Status = status;
        return obj;
    }

    public static IResult Fail(List<string> messages, ResultStatus status = ResultStatus.Error)
    {
        return new Result
        {
            Succeeded = false,
            Messages = messages,
            Status = status
        };
    }

    public static Task<IResult> FailAsync(ResultStatus status = ResultStatus.Error)
    {
        return Task.FromResult(Fail(status));
    }

    public static Task<IResult> FailAsync(string message, ResultStatus status = ResultStatus.Error)
    {
        return Task.FromResult(Fail(message, status));
    }

    public static Task<IResult> FailAsync(List<string> messages, ResultStatus status = ResultStatus.Error)
    {
        return Task.FromResult(Fail(messages, status));
    }

    public static IResult Success(ResultStatus status = ResultStatus.Success)
    {
        return new Result
        {
            Succeeded = true,
            Status = status
        };
    }

    public static IResult Success(string message, ResultStatus status = ResultStatus.Success)
    {
        Result obj = new Result
        {
            Succeeded = true
        };
        int num = 1;
        List<string> list = new List<string>(num);
        CollectionsMarshal.SetCount(list, num);
        Span<string> span = CollectionsMarshal.AsSpan(list);
        int num2 = 0;
        span[num2] = message;
        num2++;
        obj.Messages = list;
        obj.Status = status;
        return obj;
    }

    public static Task<IResult> SuccessAsync(ResultStatus status = ResultStatus.Success)
    {
        return Task.FromResult(Success(status));
    }

    public static Task<IResult> SuccessAsync(string message, ResultStatus status = ResultStatus.Success)
    {
        return Task.FromResult(Success(message, status));
    }
} 