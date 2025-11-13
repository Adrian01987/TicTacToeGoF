using TicTacToeGoF.Core.Abstractions;

namespace TicTacToeGoF.Core.Models;

internal class HumanPlayer(string name, Symbol symbol, IUserInteraction userInteraction) : Player(name, symbol , PlayerType.Human)
{
    private readonly IUserInteraction _userInteraction = userInteraction;
    public override Move GetMove(Board board)
    {
        return _userInteraction.GetMove(this, board);
    }
}
