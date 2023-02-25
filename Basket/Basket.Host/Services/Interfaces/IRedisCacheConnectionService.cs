using StackExchange.Redis;

namespace Basket.Host.Services.Interfaces
{
    public interface IRedisCacheConnectionService
    {
        public IConnectionMultiplexer Connection { get; }
    }
}