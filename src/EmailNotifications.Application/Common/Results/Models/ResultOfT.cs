using System.Runtime.InteropServices;

namespace EmailNotifications.Application.Common.Results;

public class Result<T> : Result, IResult<T>, IResult
{
    public T? Data { get; set; }

    private new static Result<T> Fail(ResultStatus status = ResultStatus.Error) =>
        new()
        {
            Succeeded = false,
            Status = status
        };

    public new static Result<T> Fail(string message, ResultStatus status = ResultStatus.Error)
    {
        var obj = new Result<T>
        {
            Succeeded = false
        };
        var num = 1;
        var list = new List<string>(num);
        CollectionsMarshal.SetCount(list, num);
        var span = CollectionsMarshal.AsSpan(list);
        var num2 = 0;
        span[num2] = message;
        num2++;
        obj.Messages = list;
        obj.Status = status;
        return obj;
    }

    public new static Result<T> Fail(List<string> messages, ResultStatus status = ResultStatus.Error) =>
        new()
        {
            Succeeded = false,
            Messages = messages,
            Status = status
        };

    public new static Task<Result<T>> FailAsync(ResultStatus status = ResultStatus.Error) => Task.FromResult(Fail(status));

    public new static Task<Result<T>> FailAsync(string message, ResultStatus status = ResultStatus.Error) => Task.FromResult(Fail(message, status));

    public new static Task<Result<T>> FailAsync(List<string> messages, ResultStatus status = ResultStatus.Error) => Task.FromResult(Fail(messages, status));

    public new static Result<T> Success(ResultStatus status = ResultStatus.Success) =>
        new()
        {
            Succeeded = true,
            Status = status
        };

    public new static Result<T> Success(string message, ResultStatus status = ResultStatus.Success)
    {
        var obj = new Result<T>
        {
            Succeeded = true
        };
        var num = 1;
        var list = new List<string>(num);
        CollectionsMarshal.SetCount(list, num);
        var span = CollectionsMarshal.AsSpan(list);
        var num2 = 0;
        span[num2] = message;
        num2++;
        obj.Messages = list;
        obj.Status = status;
        return obj;
    }

    public static Result<T> Success(T data, ResultStatus status = ResultStatus.Success) =>
        new()
        {
            Succeeded = true,
            Data = data,
            Status = status
        };

    public static Result<T> Success(T data, string message, ResultStatus status = ResultStatus.Success)
    {
        var obj = new Result<T>
        {
            Succeeded = true,
            Data = data
        };
        var num = 1;
        var list = new List<string>(num);
        CollectionsMarshal.SetCount(list, num);
        var span = CollectionsMarshal.AsSpan(list);
        var num2 = 0;
        span[num2] = message;
        num2++;
        obj.Messages = list;
        obj.Status = status;
        return obj;
    }

    public static Result<T> Success(T data, List<string> messages, ResultStatus status = ResultStatus.Success) =>
        new()
        {
            Succeeded = true,
            Data = data,
            Messages = messages,
            Status = status
        };

    public new static Task<Result<T>> SuccessAsync(ResultStatus status = ResultStatus.Success) => Task.FromResult(Success(status));

    public new static Task<Result<T>> SuccessAsync(string message, ResultStatus status = ResultStatus.Success) => Task.FromResult(Success(message, status));

    public static Task<Result<T>> SuccessAsync(T data, ResultStatus status = ResultStatus.Success) => Task.FromResult(Success(data, status));

    public static Task<Result<T>> SuccessAsync(T data, string message, ResultStatus status = ResultStatus.Success) => Task.FromResult(Success(data, message, status));
} 