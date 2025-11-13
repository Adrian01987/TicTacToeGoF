namespace TicTacToeGoF.Core.Models;

public abstract class Player(string name, Symbol symbol, PlayerType type)
{
    public string Name { get; } = name;
    public Symbol Symbol { get; } = symbol;
    public PlayerType Type { get; } = type;

    public abstract Move GetMove(Board board);
    
}
