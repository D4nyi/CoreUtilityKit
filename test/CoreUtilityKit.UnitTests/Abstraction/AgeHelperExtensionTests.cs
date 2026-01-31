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
        _services.Count.ShouldBe(2);

        ServiceDescriptor? descriptor = _services.FirstOrDefault(x => x.ServiceType == typeof(TimeProvider));

        descriptor.ShouldNotBeNull();
        descriptor.Lifetime.ShouldBe(ServiceLifetime.Singleton);

        descriptor = _services.FirstOrDefault(x => x.ServiceType == typeof(IAgeHelper));

        descriptor.ShouldNotBeNull();
        descriptor.ImplementationType.ShouldBeOfType<AgeHelper>();
        descriptor.Lifetime.ShouldBe(ServiceLifetime.Singleton);
    }

    [Fact]
    public void AddAgeHelper()
    {
        // Act
        _services.AddAgeHelper();

        // Assert
        _services.Count.ShouldBe(1);

        ServiceDescriptor? descriptor = _services.FirstOrDefault(x => x.ServiceType == typeof(IAgeHelper));

        descriptor.ShouldNotBeNull();
        descriptor.ImplementationInstance.ShouldBeOfType<AgeHelper>();
        descriptor.Lifetime.ShouldBe(ServiceLifetime.Singleton);
    }
}
