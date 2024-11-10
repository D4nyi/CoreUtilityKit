using CoreUtilityKit.EnumAttributionCache.Abstraction;

using Microsoft.Extensions.DependencyInjection;

namespace CoreUtilityKit.EnumAttributionCache.UnitTests;

public sealed class ServiceExtensionsTests
{
    private const EnumAttributeValue AttributeValue = EnumAttributeValue.Description;

    [Fact]
    public void AddEmptyEnumCache()
    {
        // Arrange
        ServiceCollection services = new();

        // Act
        services.AddEnumDescriptionCache(AttributeValue);

        // Assert
        services.Count.Should().Be(1);

        ServiceDescriptor? descriptor = services.FirstOrDefault(x => x.ServiceType == typeof(IEnumAttributeCache));

        descriptor.Should().NotBeNull();
        descriptor!.ImplementationInstance.Should().BeOfType<EnumAttributeCache>();
        descriptor.ImplementationType.Should().BeNull();
        descriptor.Lifetime.Should().Be(ServiceLifetime.Singleton);
    }

    [Fact]
    public void AddPreInitializedEnumCache()
    {
        // Arrange
        ServiceCollection services = new();
        Type[] enums = [typeof(Color)];

        // Act
        services.AddEnumDescriptionCache(AttributeValue, enums);

        // Assert
        services.Count.Should().Be(1);

        ServiceDescriptor? descriptor = services.FirstOrDefault(x => x.ServiceType == typeof(IEnumAttributeCache));

        descriptor.Should().NotBeNull();
        descriptor!.ImplementationInstance.Should().NotBeNull();
        descriptor.ImplementationInstance.Should().BeOfType<EnumAttributeCache>();
        descriptor.Lifetime.Should().Be(ServiceLifetime.Singleton);
    }

    [Fact]
    public void AddReadonlyEnumCache()
    {
        // Arrange
        ServiceCollection services = new();
        Type[] enums = [typeof(Color)];

        // Act
        services.AddReadonlyEnumAttributionCache(AttributeValue, enums);

        // Assert
        services.Count.Should().Be(1);

        ServiceDescriptor? descriptor = services.FirstOrDefault(x => x.ServiceType == typeof(IReadonlyEnumAttributeCache));

        descriptor.Should().NotBeNull();
        descriptor!.ImplementationInstance.Should().NotBeNull();
        descriptor.ImplementationInstance.Should().BeOfType<ReadonlyEnumAttributeCache>();
        descriptor.Lifetime.Should().Be(ServiceLifetime.Singleton);
    }
}
