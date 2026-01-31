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
        services.AddEnumAttributeCache(AttributeValue);

        // Assert
        services.Count.ShouldBe(1);

        ServiceDescriptor? descriptor = services.FirstOrDefault(x => x.ServiceType == typeof(IEnumAttributeCache));

        descriptor.ShouldNotBeNull();
        descriptor!.ImplementationInstance.ShouldBeOfType<EnumAttributeCache>();
        descriptor.ImplementationType.ShouldBeNull();
        descriptor.Lifetime.ShouldBe(ServiceLifetime.Singleton);
    }

    [Fact]
    public void AddPreInitializedEnumCache()
    {
        // Arrange
        ServiceCollection services = new();
        Type[] enums = [typeof(Color)];

        // Act
        services.AddEnumAttributeCache(AttributeValue, enums);

        // Assert
        services.Count.ShouldBe(1);

        ServiceDescriptor? descriptor = services.FirstOrDefault(x => x.ServiceType == typeof(IEnumAttributeCache));

        descriptor.ShouldNotBeNull();
        descriptor!.ImplementationInstance.ShouldNotBeNull();
        descriptor.ImplementationInstance.ShouldBeOfType<EnumAttributeCache>();
        descriptor.Lifetime.ShouldBe(ServiceLifetime.Singleton);
    }

    [Fact]
    public void AddReadonlyEnumCache()
    {
        // Arrange
        ServiceCollection services = new();
        Type[] enums = [typeof(Color)];

        // Act
        services.AddReadonlyEnumAttributeCache(AttributeValue, enums);

        // Assert
        services.Count.ShouldBe(1);

        ServiceDescriptor? descriptor = services.FirstOrDefault(x => x.ServiceType == typeof(IReadonlyEnumAttributeCache));

        descriptor.ShouldNotBeNull();
        descriptor!.ImplementationInstance.ShouldNotBeNull();
        descriptor.ImplementationInstance.ShouldBeOfType<ReadonlyEnumAttributeCache>();
        descriptor.Lifetime.ShouldBe(ServiceLifetime.Singleton);
    }
}
