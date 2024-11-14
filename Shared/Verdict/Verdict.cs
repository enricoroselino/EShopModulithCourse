using Microsoft.AspNetCore.Http;

namespace Shared.Verdict;

public class Verdict : IVerdict
{
    protected Verdict(bool isSuccess, IFailure failure)
    {
        if (isSuccess && !Equals(failure, Shared.Verdict.Failure.None) ||
            !isSuccess && Equals(failure, Shared.Verdict.Failure.None))
        {
            throw new VerdictException("Invalid Failure", nameof(failure));
        }

        IsSuccess = isSuccess;
        Failure = failure;
    }

    public bool IsSuccess { get; }
    public bool IsFailure => !IsSuccess;
    public IFailure Failure { get; }
    public static Verdict Success() => new Verdict(true, failure: Shared.Verdict.Failure.None);
    public static Verdict<T> Success<T>(T value) => new Verdict<T>(true, value, Shared.Verdict.Failure.None);
    public static Verdict Failed(IFailure failure) => new Verdict(false, failure: failure);
    public static Verdict<T> Failed<T>(IFailure failure) => new Verdict<T>(false, default, failure);

    public TResult Fold<TResult>(Func<TResult> onSuccess, Func<IFailure, TResult> onFailure)
    {
        return IsSuccess ? onSuccess() : onFailure(Failure);
    }
}

public class Verdict<T> : Verdict, IVerdict<T>
{
    internal Verdict(bool isSuccess, T? data, IFailure failure) : base(isSuccess, failure)
    {
        Data = data;
    }

    public T? Data { get; }

    public TResult Fold<TResult>(Func<T?, TResult> onSuccess, Func<IFailure, TResult> onFailure)
    {
        return IsSuccess ? onSuccess(Data) : onFailure(Failure);
    }
}

public interface IFailure
{
    public FailureType Type { get; }
    public string Message { get; }
}

public sealed class Failure : IEquatable<Failure>, IFailure
{
    private Failure(string message, FailureType type)
    {
        Message = message;
        Type = type;
    }

    public FailureType Type { get; }
    public string Message { get; }

    public static IFailure None => new Failure(string.Empty, FailureType.None);
    public static IFailure BadRequest(string message) => new Failure(message, FailureType.BadRequest);
    public static IFailure Forbidden(string message) => new Failure(message, FailureType.Forbidden);
    public static IFailure NotFound(string message) => new Failure(message, FailureType.NotFound);
    public static IFailure Conflict(string message) => new Failure(message, FailureType.Conflict);
    public static IFailure Server() => new Failure("Something just crashed.", FailureType.Server);

    public bool Equals(Failure? other)
    {
        if (other is null) return false;
        if (ReferenceEquals(this, other)) return true;
        return string.Equals(Message, other.Message, StringComparison.InvariantCultureIgnoreCase) &&
               Type == other.Type;
    }

    public override bool Equals(object? obj)
    {
        return ReferenceEquals(this, obj) || obj is Failure other && Equals(other);
    }

    public override int GetHashCode()
    {
        var hashCode = new HashCode();
        hashCode.Add(Message, StringComparer.InvariantCultureIgnoreCase);
        hashCode.Add((int)Type);
        return hashCode.ToHashCode();
    }

    public static bool operator ==(Failure? left, Failure? right)
    {
        return Equals(left, right);
    }

    public static bool operator !=(Failure? left, Failure? right)
    {
        return !Equals(left, right);
    }
}

public enum FailureType
{
    NotFound,
    Conflict,
    BadRequest,
    Server,
    Forbidden,
    None
}

public class VerdictException : Exception
{
    public VerdictException(string message) : base(message)
    {
    }

    public VerdictException(string message, string details) : base(message)
    {
        Details = details;
    }

    public string? Details { get; }
}

internal static class FailureHelper
{
    public static int GetStatusCode(FailureType errorType) =>
        errorType switch
        {
            FailureType.BadRequest => StatusCodes.Status400BadRequest,
            FailureType.Conflict => StatusCodes.Status409Conflict,
            FailureType.NotFound => StatusCodes.Status404NotFound,
            FailureType.Forbidden => StatusCodes.Status403Forbidden,
            _ => StatusCodes.Status500InternalServerError
        };

    public static string GetTitle(FailureType errorType) =>
        errorType switch
        {
            FailureType.BadRequest => "Bad Request",
            FailureType.Conflict => "Conflict",
            FailureType.NotFound => "Not Found",
            FailureType.Forbidden => "Forbidden",
            FailureType.Server => "Internal Server Error",
            _ => "Unknown Error"
        };
}