using TicTacToeGoF.Core.Models;

namespace TicTacToeGoF.Core.Abstractions;

public interface IUserInteraction
{
    void DisplayWelcomeMessage();
    PlayerCreationInfo GetPlayerDetails(string playerPrompt);
    Move GetMove(Player player, Board board);
    void DisplayWinner(string winnerName);
    void DisplayDraw();
    bool GetPlayAgain();
    void DisplayMessage(MessageSeverity severity, string message);
    void Clear();
}
