namespace CoreUtilityKit.UnitTests.Helpers;

public sealed class TaskExtensionsTest
{
    private static readonly int[] _expected2Element = [0, 1];
    private static readonly int[] _expected3Element = [0, 1, 2];
    private static readonly int[] _expected4Element = [0, 1, 2, 4];

    [Fact]
    public async Task WhenAll_ReturnsResult_HappyPath()
    {
        // Arrange
        Task<int>[] tasks = _expected3Element
            .Select(Task.FromResult)
            .ToArray();

        // Act
        int[] results = await TaskHelpers.WhenAll(tasks);

        // Assert
        results.Should().BeEquivalentTo(_expected3Element);
    }

    [Fact]
    public async Task WhenAll_ShouldThrow_WhenOneOfTheTasksThrows()
    {
        // Arrange
        Task<int>[] tasks =
        [
            Task.FromResult(0),
            Task.FromException<int>(new InvalidOperationException("ERROR!")),
            Task.FromResult(2),
        ];

        // Act
        Func<Task> action = () => TaskHelpers.WhenAll(tasks);

        // Assert
        await action.Should()
            .ThrowAsync<InvalidOperationException>()
            .WithMessage("ERROR!");
    }

    [Fact]
    public async Task WhenAll_ShouldThrow_WhenCancelled()
    {
        // Arrange
        CancellationTokenSource cts = new(1_000);
        Task<int>[] tasks =
        [
            Run(cts.Token),
            Run(cts.Token),
            Run(cts.Token),
        ];

        // Act
        Func<Task> action = () => TaskHelpers.WhenAll(tasks);

        // Assert
        await action.Should().ThrowAsync<System.Diagnostics.UnreachableException>();

        // Cleanup
        cts.Dispose();

        static async Task<int> Run(CancellationToken ct)
        {
            await Task.Delay(5_000, ct);
            return 0;
        }
    }

    [Fact]
    public async Task TupleOf2_ReturnsResult_HappyPath()
    {
        // Arrange
        (Task<int>, Task<int>) tasks = (Task.FromResult(_expected2Element[0]), Task.FromResult(_expected2Element[1]));

        // Act
        int[] results = await tasks;

        // Assert
        results.Should().BeEquivalentTo(_expected2Element);
    }

    [Fact]
    public async Task TupleOf3_ReturnsResult_HappyPath()
    {
        // Arrange
        (Task<int>, Task<int>, Task<int>) tasks = (Task.FromResult(_expected3Element[0]), Task.FromResult(_expected3Element[1]), Task.FromResult(_expected3Element[2]));

        // Act
        int[] results = await tasks;

        // Assert
        results.Should().BeEquivalentTo(_expected3Element);
    }

    [Fact]
    public async Task TupleOf4_ReturnsResult_HappyPath()
    {
        // Arrange
        (Task<int>, Task<int>, Task<int>, Task<int>) tasks = (Task.FromResult(_expected4Element[0]), Task.FromResult(_expected4Element[1]), Task.FromResult(_expected4Element[2]), Task.FromResult(_expected4Element[3]));

        // Act
        int[] results = await tasks;

        // Assert
        results.Should().BeEquivalentTo(_expected4Element);
    }
}
