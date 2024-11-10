using CoreUtilityKit.Abstraction;

using Microsoft.Extensions.DependencyInjection;

namespace CoreUtilityKit.UnitTests.Abstraction;

public sealed class AgeHelperExtensionTests
{
    private readonly ServiceCollection _services = new();

    [Fact]
    public void AddAgeHelperWithTimeProvider()
    {
        // Act
        _services.AddAgeHelperWithTimeProvider();

        // Assert
        _services.Count.Should().Be(2);

        ServiceDescriptor? descriptor = _services.FirstOrDefault(x => x.ServiceType == typeof(TimeProvider));

        descriptor.Should().NotBeNull();
        descriptor!.Lifetime.Should().Be(ServiceLifetime.Singleton);

        descriptor = _services.FirstOrDefault(x => x.ServiceType == typeof(IAgeHelper));

        descriptor.Should().NotBeNull();
        descriptor!.ImplementationType.Should().Be<AgeHelper>();
        descriptor.Lifetime.Should().Be(ServiceLifetime.Singleton);
    }

    [Fact]
    public void AddAgeHelper()
    {
        // Act
        _services.AddAgeHelper();

        // Assert
        _services.Count.Should().Be(1);

        ServiceDescriptor? descriptor = _services.FirstOrDefault(x => x.ServiceType == typeof(IAgeHelper));

        descriptor.Should().NotBeNull();
        descriptor!.ImplementationInstance.Should().BeOfType<AgeHelper>();
        descriptor.Lifetime.Should().Be(ServiceLifetime.Singleton);
    }
}
