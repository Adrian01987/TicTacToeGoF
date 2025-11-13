using TicTacToeGoF.Core.Models;

namespace TicTacToeGoF.Tests.UnitTests.Models;

public class BoardTests
{
    private readonly Player _playerX = new ComputerPlayer("Test", Symbol.X);

    public BoardTests()
    {
        Board.Instance.Clear();
    }

    [Fact]
    public void CheckForWin_WithHorizontalWin_ReturnsTrue()
    {
        // Arrange
        var board = Board.Instance;
        board.PlaceMark(new ValidMove(_playerX, 0, 0));
        board.PlaceMark(new ValidMove(_playerX, 0, 1));
        board.PlaceMark(new ValidMove(_playerX, 0, 2));

        // Act
        var result = board.CheckForWin(Symbol.X);

        // Assert
        Assert.True(result);
    }

    [Fact]
    public void IsDraw_WhenBoardIsFullWithNoWinner_ReturnsTrue()
    {
        // Arrange
        var board = Board.Instance;
        var playerO = new ComputerPlayer("TestO", Symbol.O);

        // Fill the board in a non-winning way
        board.PlaceMark(new ValidMove(_playerX, 0, 0));
        board.PlaceMark(new ValidMove(playerO, 0, 1));
        board.PlaceMark(new ValidMove(_playerX, 0, 2));
        board.PlaceMark(new ValidMove(playerO, 1, 0));
        board.PlaceMark(new ValidMove(playerO, 1, 1));
        board.PlaceMark(new ValidMove(_playerX, 1, 2));
        board.PlaceMark(new ValidMove(_playerX, 2, 0));
        board.PlaceMark(new ValidMove(_playerX, 2, 1));
        board.PlaceMark(new ValidMove(playerO, 2, 2));

        // Act
        var isDraw = board.IsDraw();
        var hasWinner = board.CheckForWin(Symbol.X) || board.CheckForWin(Symbol.O);

        // Assert
        Assert.True(isDraw);
        Assert.False(hasWinner);
    }
}