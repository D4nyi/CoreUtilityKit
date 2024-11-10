using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;

namespace CoreUtilityKit.Helpers;

public static class TaskHelpers
{
    public static async Task<T[]> WhenAll<T>(params Task<T>[] tasks)
    {
        Task<T[]> allTasks = Task.WhenAll(tasks);

#pragma warning disable CA1031 // Do not catch general exception types
        try
        {
            return await allTasks;
        }
        catch
        {
            // ignore
        }
#pragma warning restore CA1031 // Do not catch general exception types

        if (allTasks.Exception is null)
            TaskAggregateUnreachable();

        throw allTasks.Exception;
    }

    public static TaskAwaiter<T[]> GetAwaiter<T>(this (Task<T>, Task<T>) tasksTuple) =>
        WhenAll(tasksTuple.Item1, tasksTuple.Item2).GetAwaiter();

    public static TaskAwaiter<T[]> GetAwaiter<T>(this (Task<T>, Task<T>, Task<T>) tasksTuple) =>
        WhenAll(tasksTuple.Item1, tasksTuple.Item2, tasksTuple.Item3).GetAwaiter();

    public static TaskAwaiter<T[]> GetAwaiter<T>(this (Task<T>, Task<T>, Task<T>, Task<T>) tasksTuple) =>
        WhenAll(tasksTuple.Item1, tasksTuple.Item2, tasksTuple.Item3, tasksTuple.Item4).GetAwaiter();

    [DoesNotReturn]
    private static void TaskAggregateUnreachable()
    {
        throw new UnreachableException("AggregateException of all tasks is null!");
    }
}
