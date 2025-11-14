using TicTacToeGoF.Core.Command;
using TicTacToeGoF.Core.Models;

namespace TicTacToeGoF.Tests.UnitTests.Command;

public class PlaceMarkCommandTests
{
    public PlaceMarkCommandTests()
    {
        Board.Instance.Clear();
    }

    [Fact]
    public void Undo_AfterExecuting_RestoresBoardToPreviousState()
    {
        // Arrange
        var board = Board.Instance;
        var playerX = new ComputerPlayer("PlayerX", Symbol.X);
        var playerO = new ComputerPlayer("PlayerO", Symbol.O);

        // Set an initial board state
        board.Reset();
        board.PlaceMark(new ValidMove(playerX, 0, 0));

        // The move we will execute and then undo
        var move = new ValidMove(playerO, 1, 1);
        var command = new PlaceMarkCommand(board, move);

        // Act
        command.Execute(); // Board is now X at (0,0) and O at (1,1)
        command.Undo();    // Board should revert to only having X at (0,0)

        // Assert
        board.GetCell(0, 0).Symbol.Should().Be(Symbol.X);
        board.GetCell(1, 1).Symbol.Should().Be(Symbol.None, "the cell should be empty after undoing the move");
    }
}