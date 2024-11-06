using System;
using System.Collections.Generic;

public readonly struct Result<T> : IEquatable<Result<T>>
{
	public static readonly Result<T> Bottom = new();
	internal readonly ResultState State;
	internal readonly T Value;
	internal readonly Exception Error;

	public Result(T value)
	{
		State = ResultState.Success;
		Value = value;
		Error = null;
	}

	public Result(Exception e)
	{
		State = ResultState.Faulted;
		Value = default;
		Error = e;
	}

	public bool IsSuccess => State == ResultState.Success;
	public bool IsFaulted => State == ResultState.Faulted;

	public void Match(Action<T> success, Action<Exception> failure)
	{
		if (IsSuccess)
			success(Value);
		else
			failure(Error);
	}

	public TResult Match<TResult>(Func<T, TResult> success, Func<Exception, TResult> failure)
	{
		return IsSuccess ? success(Value) : failure(Error);
	}

	public Result<TResult> Map<TResult>(Func<T, TResult> mapper)
	{
		if (IsFaulted)
			return new Result<TResult>(Error);
		
		try
		{
			return new Result<TResult>(mapper(Value));
		}
		catch (Exception e)
		{
			return new Result<TResult>(e);
		}
	}

	public bool Equals(Result<T> other)
	{
		if (State != other.State)
			return false;

		if (IsFaulted)
			return true;  // All faulted states are considered equal

		return EqualityComparer<T>.Default.Equals(Value, other.Value);
	}

	public override bool Equals(object obj)
	{
		return obj is Result<T> other && Equals(other);
	}

	public override int GetHashCode()
	{
		if (IsFaulted)
			return State.GetHashCode();

		return HashCode.Combine(State, Value);
	}

	public static bool operator ==(Result<T> left, Result<T> right)
	{
		return left.Equals(right);
	}

	public static bool operator !=(Result<T> left, Result<T> right)
	{
		return !(left == right);
	}

	internal enum ResultState
	{
		Success,
		Faulted
	}
}

public static class Result
{
	public static Result<T> Success<T>(T value) => new(value);
	public static Result<T> Failure<T>(Exception error) => new(error);
	public static Result<T> From<T>(Func<T> func)
	{
		try
		{
			return new Result<T>(func());
		}
		catch (Exception e)
		{
			return new Result<T>(e);
		}
	}
}