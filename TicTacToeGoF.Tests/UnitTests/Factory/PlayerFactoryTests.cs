using Moq;
using TicTacToeGoF.Core.Abstractions;
using TicTacToeGoF.Core.Factory;
using TicTacToeGoF.Core.Models;

namespace TicTacToeGoF.Tests.UnitTests.Factory;

public class PlayerFactoryTests
{
    private readonly PlayerFactory _factory;
    private readonly Mock<IUserInteraction> _mockUI;

    public PlayerFactoryTests()
    {
        Board.Instance.Clear();
        _mockUI = new Mock<IUserInteraction>();
        _factory = new PlayerFactory(_mockUI.Object);
    }

    [Fact]
    public void CreatePlayer_WhenTypeIsHuman_ReturnsHumanPlayer()
    {
        // Act
        var player = _factory.CreatePlayer("Alice", Symbol.X, PlayerType.Human);

        // Assert
        Assert.IsType<HumanPlayer>(player);
    }

    [Fact]
    public void CreatePlayer_WhenTypeIsComputer_ReturnsComputerPlayer()
    {
        // Act
        var player = _factory.CreatePlayer("Bob", Symbol.O, PlayerType.Computer);

        // Assert
        Assert.IsType<ComputerPlayer>(player);
    }
}
