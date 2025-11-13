using Moq;
using TicTacToeGoF.Core;
using TicTacToeGoF.Core.Abstractions;
using TicTacToeGoF.Core.Factory;
using TicTacToeGoF.Core.Models;

namespace TicTacToeGoF.Tests.IntegrationTests;

public class GameFacadeTests
{
    public GameFacadeTests()
    {
        Board.Instance.Clear();
    }

    [Fact]
    public void Play_WhenGameIsWon_DisplaysWinner()
    {
        // Arrange
        var mockUI = new Mock<IUserInteraction>();
        var mockFactory = new Mock<PlayerFactory>(mockUI.Object);

        // 1. Create a real HumanPlayer for the test
        var humanPlayer = new HumanPlayer("Alice", Symbol.X, mockUI.Object);

        // 2. Create a MOCK ComputerPlayer using Moq
        var mockComputer = new Mock<Player>("Bob", Symbol.O, PlayerType.Computer);

        // 3. Script the mock computer's moves to be predictable and non-blocking
        mockComputer.SetupSequence(p => p.GetMove(It.IsAny<Board>()))
            .Returns(new ValidMove(mockComputer.Object, 2, 0))
            .Returns(new ValidMove(mockComputer.Object, 2, 1));

        // 4. Setup the factory to return our specific player instances
        mockFactory.Setup(f => f.CreatePlayer("Alice", Symbol.X, PlayerType.Human)).Returns(humanPlayer);
        mockFactory.Setup(f => f.CreatePlayer("Bob", Symbol.O, PlayerType.Computer)).Returns(mockComputer.Object);
        
        // 5. Setup the user interaction to return the new structured objects
        mockUI.SetupSequence(ui => ui.GetPlayerDetails(It.IsAny<string>()))
            .Returns(new PlayerCreationInfo(PlayerType.Human, "Alice", Symbol.X))
            .Returns(new PlayerCreationInfo(PlayerType.Computer, "Bob"));

        // The HumanPlayer now asks the UI for a Move object directly
        mockUI.SetupSequence(ui => ui.GetMove(humanPlayer, It.IsAny<Board>()))
            .Returns(new ValidMove(humanPlayer, 0, 0)) // Alice's move
            .Returns(new ValidMove(humanPlayer, 0, 1)) // Alice's move
            .Returns(new ValidMove(humanPlayer, 0, 2)); // Alice's winning move

        mockUI.Setup(ui => ui.GetPlayAgain()).Returns(false);

        // 6. Inject the mocked factory
        var game = new GameFacade(mockUI.Object, mockFactory.Object);

        // Act
        game.Play();

        // Assert
        mockUI.Verify(ui => ui.DisplayWinner("Alice"), Times.Once());

    }
}
