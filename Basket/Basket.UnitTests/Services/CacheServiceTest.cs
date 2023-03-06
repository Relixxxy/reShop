using Basket.Host.Configuration;
using Basket.Host.Services.Interfaces;
using Basket.Host.Services;
using Infrastructure.Services.Interfaces;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Moq;
using FluentAssertions;
using StackExchange.Redis;

namespace Basket.UnitTests.Services;

public class CacheServiceTest
{
    private readonly Mock<ILogger<CacheService>> _loggerMock;
    private readonly Mock<IRedisCacheConnectionService> _redisCacheConnectionServiceMock;
    private readonly Mock<IJsonSerializer> _jsonSerializerMock;
    private readonly RedisConfig _redisConfig;

    private readonly CacheService _cacheService;

    public CacheServiceTest()
    {
        _loggerMock = new Mock<ILogger<CacheService>>();
        _redisCacheConnectionServiceMock = new Mock<IRedisCacheConnectionService>();
        _jsonSerializerMock = new Mock<IJsonSerializer>();
        _redisConfig = new RedisConfig
        {
            Host = "localhost",
            CacheTimeout = new System.TimeSpan(0, 5, 0)
        };

        var optionsMock = new Mock<IOptions<RedisConfig>>();
        optionsMock.Setup(x => x.Value).Returns(_redisConfig);

        _cacheService = new CacheService(_loggerMock.Object, _redisCacheConnectionServiceMock.Object, optionsMock.Object, _jsonSerializerMock.Object);
    }

    [Fact]
    public async Task GetAsync_ShouldReturnDeserializedValue_WhenKeyExists()
    {
        // Arrange
        var key = "key";
        var value = "value";
        var serializedValue = "serialized-value";
        var databaseMock = new Mock<IDatabase>();
        _redisCacheConnectionServiceMock.Setup(x => x.Connection.GetDatabase(It.IsAny<int>(), It.IsAny<object>())).Returns(databaseMock.Object);
        databaseMock.Setup(x => x.StringGetAsync(It.IsAny<RedisKey>(), It.IsAny<CommandFlags>())).ReturnsAsync(serializedValue);

        _jsonSerializerMock.Setup(x => x.Deserialize<string>(serializedValue)).Returns(value);

        // Act
        var result = await _cacheService.GetAsync<string>(key);

        // Assert
        result.Should().Be(value);
    }

    [Fact]
    public async Task GetAsync_ShouldReturnDefault_WhenKeyDoesNotExist()
    {
        // Arrange
        var key = "key";
        var databaseMock = new Mock<IDatabase>();
        _redisCacheConnectionServiceMock.Setup(x => x.Connection.GetDatabase(It.IsAny<int>(), It.IsAny<object>())).Returns(databaseMock.Object);
        databaseMock.Setup(x => x.StringGetAsync(It.IsAny<RedisKey>(), It.IsAny<CommandFlags>())).ReturnsAsync(default(RedisValue));

        // Act
        var result = await _cacheService.GetAsync<string>(key);

        // Assert
        result.Should().BeNull();
    }
}
