using TicTacToeGoF.Core.Memento;
using TicTacToeGoF.Core.Models;

namespace TicTacToeGoF.Core.Command;

internal class PlaceMarkCommand(Board board, ValidMove move) : ICommand
{
    private readonly Board _board = board;
    private readonly ValidMove _move = move;
    private BoardMemento? _memento;

    public void Execute()
    {
        _memento = _board.CreateMemento();
        _board.PlaceMark(_move);
    }

    public void Undo()
    {
        if (_memento is not null)
        {
            _board.RestoreMemento(_memento);
        }
    }
}
