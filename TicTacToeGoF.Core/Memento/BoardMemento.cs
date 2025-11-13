using TicTacToeGoF.Core.Models;

namespace TicTacToeGoF.Core.Memento;

internal class BoardMemento(Symbol[,] state)
{
    private readonly Symbol[,] _state = state;

    internal Symbol[,] GetState() => _state;

}
