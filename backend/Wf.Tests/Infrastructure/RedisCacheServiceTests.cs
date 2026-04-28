using System.Text;
using System.Text.Json;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Logging;
using Moq;
using Wf.Infrastructure.WeatherPersistanceCache;
using Xunit;

namespace Wf.Tests;

public class RedisCacheServiceTests
{
    private readonly Mock<IDistributedCache> _distributedCacheMock;
    private readonly Mock<ILogger<RedisCacheService>> _loggerMock;
    private readonly RedisCacheService _service;

    public RedisCacheServiceTests()
    {
        _distributedCacheMock = new Mock<IDistributedCache>();
        _loggerMock = new Mock<ILogger<RedisCacheService>>();
        _service = new RedisCacheService(_distributedCacheMock.Object, _loggerMock.Object);
    }

    [Fact]
    public async Task GetAsync_CacheHit_ReturnsDeserializedObject()
    {
        // Arrange
        var key = "test_key";
        var cancellationToken = CancellationToken.None;
        var testObject = new TestClass { Id = 1, Name = "Test" };
        var serialized = JsonSerializer.Serialize(testObject);
        var bytes = Encoding.UTF8.GetBytes(serialized);

        _distributedCacheMock
            .Setup(c => c.GetAsync(key, cancellationToken))
            .ReturnsAsync(bytes);

        // Act
        var result = await _service.GetAsync<TestClass>(key, cancellationToken);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(testObject.Id, result.Id);
        Assert.Equal(testObject.Name, result.Name);
        _distributedCacheMock.Verify(c => c.GetAsync(key, cancellationToken), Times.Once);
        _loggerMock.Verify(
            x => x.Log(
                LogLevel.Debug,
                It.IsAny<EventId>(),
                It.Is<It.IsAnyType>((v, t) => v.ToString().Contains("Кэш-попадание")),
                It.IsAny<Exception?>(),
                It.IsAny<Func<It.IsAnyType, Exception?, string>>()),
            Times.Once);
    }

    [Fact]
    public async Task GetAsync_CacheMiss_ReturnsNull()
    {
        // Arrange
        var key = "test_key";
        var cancellationToken = CancellationToken.None;

        _distributedCacheMock
            .Setup(c => c.GetAsync(key, cancellationToken))
            .ReturnsAsync((byte[]?)null);

        // Act
        var result = await _service.GetAsync<TestClass>(key, cancellationToken);

        // Assert
        Assert.Null(result);
        _distributedCacheMock.Verify(c => c.GetAsync(key, cancellationToken), Times.Once);
        _loggerMock.Verify(
            x => x.Log(
                LogLevel.Debug,
                It.IsAny<EventId>(),
                It.Is<It.IsAnyType>((v, t) => v.ToString().Contains("Кэш-промах")),
                It.IsAny<Exception?>(),
                It.IsAny<Func<It.IsAnyType, Exception?, string>>()),
            Times.Once);
    }

    [Fact]
    public async Task GetAsync_EmptyString_ReturnsNull()
    {
        // Arrange
        var key = "test_key";
        var cancellationToken = CancellationToken.None;

        _distributedCacheMock
            .Setup(c => c.GetAsync(key, cancellationToken))
            .ReturnsAsync(Array.Empty<byte>());

        // Act
        var result = await _service.GetAsync<TestClass>(key, cancellationToken);

        // Assert
        Assert.Null(result);
        _distributedCacheMock.Verify(c => c.GetAsync(key, cancellationToken), Times.Once);
        _loggerMock.Verify(
            x => x.Log(
                LogLevel.Debug,
                It.IsAny<EventId>(),
                It.Is<It.IsAnyType>((v, t) => v.ToString().Contains("Кэш-промах")),
                It.IsAny<Exception?>(),
                It.IsAny<Func<It.IsAnyType, Exception?, string>>()),
            Times.Once);
    }

    private class TestClass
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
    }
}