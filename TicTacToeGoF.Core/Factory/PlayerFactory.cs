using TicTacToeGoF.Core.Abstractions;
using TicTacToeGoF.Core.Models;

namespace TicTacToeGoF.Core.Factory;

public class PlayerFactory(IUserInteraction userInteraction)
{
    private readonly IUserInteraction _userInteraction = userInteraction;

    public virtual Player CreatePlayer(string name, Symbol symbol, PlayerType type)
    {
        return type switch
        {
            PlayerType.Human => new HumanPlayer(name, symbol, _userInteraction),
            PlayerType.Computer => new ComputerPlayer(name, symbol),
            _ => throw new ArgumentException("Invalid player type", nameof(type)),
        };
    }
}
