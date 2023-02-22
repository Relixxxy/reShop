using System.Threading;
using Infrastructure.UnitTests.Mocks;

namespace Infrastructure.UnitTests.Services;

public class BaseDataServiceTest
{
    private readonly Mock<IDbContextTransaction> _dbContextTransaction;
    private readonly Mock<ILogger<MockService>> _logger;
    private readonly MockService _mockService;

    public BaseDataServiceTest()
    {
        var dbContextWrapper = new Mock<IDbContextWrapper<MockDbContext>>();
        _dbContextTransaction = new Mock<IDbContextTransaction>();
        _logger = new Mock<ILogger<MockService>>();

        dbContextWrapper.Setup(s => s.BeginTransactionAsync(It.IsAny<CancellationToken>())).ReturnsAsync(_dbContextTransaction.Object);

        _mockService = new MockService(dbContextWrapper.Object, _logger.Object);
    }

    [Fact]
    public async Task ExecuteSafe_Success()
    {
        // arrange

        // act
        await _mockService.RunWithoutException();

        // assert
        _dbContextTransaction.Verify(t => t.CommitAsync(CancellationToken.None), Times.Once);
        _dbContextTransaction.Verify(t => t.RollbackAsync(CancellationToken.None), Times.Never);
    }

    [Fact]
    public async Task ExecuteSafe_Failed()
    {
        // arrange

        // act
        await _mockService.RunWithException();

        // assert
        _dbContextTransaction.Verify(t => t.CommitAsync(CancellationToken.None), Times.Never);
        _dbContextTransaction.Verify(t => t.RollbackAsync(CancellationToken.None), Times.Once);

        _logger.Verify(
            x => x.Log(
                LogLevel.Error,
                It.IsAny<EventId>(),
                It.Is<It.IsAnyType>((o, t) => o.ToString() !
                    .Contains($"transaction is rollbacked")),
                It.IsAny<Exception>(),
                It.IsAny<Func<It.IsAnyType, Exception, string>>() !),
            Times.Once);
    }
}