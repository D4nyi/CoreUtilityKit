using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;

namespace CoreUtilityKit.Helpers;

/// <summary>
/// Provides utility methods for <see cref="Task"/>.
/// </summary>
public static class TaskHelpers
{
    /// <summary>
    /// Waits for all of the provided <see cref="Task{T}"/> objects to complete.
    /// This method ignores individual task exceptions during the await but throws the aggregate exception if any task fails.
    /// </summary>
    /// <typeparam name="T">The type of the result of the tasks.</typeparam>
    /// <param name="tasks">The tasks to wait for.</param>
    /// <returns>A task that represents the completion of all of the provided tasks.</returns>
    /// <exception cref="AggregateException">Thrown if one or more of the tasks fail.</exception>
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

    /// <summary>
    /// Gets an awaiter for a tuple of two <see cref="Task{T}"/> objects.
    /// </summary>
    /// <typeparam name="T">The type of the result of the tasks.</typeparam>
    /// <param name="tasksTuple">The tuple of tasks.</param>
    /// <returns>An awaiter for the combined task.</returns>
    public static TaskAwaiter<T[]> GetAwaiter<T>(this (Task<T>, Task<T>) tasksTuple) =>
        WhenAll(tasksTuple.Item1, tasksTuple.Item2).GetAwaiter();

    /// <summary>
    /// Gets an awaiter for a tuple of three <see cref="Task{T}"/> objects.
    /// </summary>
    /// <typeparam name="T">The type of the result of the tasks.</typeparam>
    /// <param name="tasksTuple">The tuple of tasks.</param>
    /// <returns>An awaiter for the combined task.</returns>
    public static TaskAwaiter<T[]> GetAwaiter<T>(this (Task<T>, Task<T>, Task<T>) tasksTuple) =>
        WhenAll(tasksTuple.Item1, tasksTuple.Item2, tasksTuple.Item3).GetAwaiter();

    /// <summary>
    /// Gets an awaiter for a tuple of four <see cref="Task{T}"/> objects.
    /// </summary>
    /// <typeparam name="T">The type of the result of the tasks.</typeparam>
    /// <param name="tasksTuple">The tuple of tasks.</param>
    /// <returns>An awaiter for the combined task.</returns>
    public static TaskAwaiter<T[]> GetAwaiter<T>(this (Task<T>, Task<T>, Task<T>, Task<T>) tasksTuple) =>
        WhenAll(tasksTuple.Item1, tasksTuple.Item2, tasksTuple.Item3, tasksTuple.Item4).GetAwaiter();

    [DoesNotReturn]
    private static void TaskAggregateUnreachable()
    {
        throw new UnreachableException("AggregateException of all tasks is null!");
    }
}
