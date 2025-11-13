namespace TicTacToeGoF.Core.Models;

public class Cell
{
    public Symbol Symbol { get; private set; } = Symbol.None;

    internal void SetCell(Symbol symbol)
    {
        if (symbol != Symbol.None && Symbol != Symbol.None)
        {
            throw new InvalidOperationException("Cell is already occupied.");
        }
        Symbol = symbol;
    }

    internal void RestoreCell(Symbol symbol)
    {
        Symbol = symbol;
    }
}
