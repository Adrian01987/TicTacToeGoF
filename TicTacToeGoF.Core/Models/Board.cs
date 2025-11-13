using TicTacToeGoF.Core.Memento;
using TicTacToeGoF.Core.Observers;

namespace TicTacToeGoF.Core.Models;

public class Board : ISubject
{
    private static readonly Lazy<Board> _instance = new(() => new Board());
    private readonly Cell[,] _cells = new Cell[3, 3];
    private readonly List<IObserver> _observers = [];


    private Board()
    {
        for (int i = 0; i < 3; i++)
            for (int j = 0; j < 3; j++)
                _cells[i, j] = new Cell();
    }

    public static Board Instance => _instance.Value;

    public Cell GetCell(int row, int col)
    {
        if (row < 0 || row >= 3)
            throw new ArgumentOutOfRangeException(nameof(row), "Row must be between 0 and 2.");

        if (col < 0 || col >= 3)
            throw new ArgumentOutOfRangeException(nameof(col), "Column must be between 0 and 2.");

        return _cells[row, col];
    }

    internal void PlaceMark(ValidMove move)
    {
        GetCell(move.Row, move.Column).SetCell(move.Player.Symbol);
        Notify();
    }

    internal void Reset()
    {
        foreach (var cell in _cells)
            cell.RestoreCell(Symbol.None);

        Notify();
    }

    internal void Clear()
    {
        _observers.Clear();
        Reset();
    }

    internal bool CheckForWin(Symbol symbol)
    {
        for (int i = 0; i < 3; i++)
        {
            if ((_cells[i, 0].Symbol == symbol
                && _cells[i, 1].Symbol == symbol
                && _cells[i, 2].Symbol == symbol)
                ||
                (_cells[0, i].Symbol == symbol
                && _cells[1, i].Symbol == symbol
                && _cells[2, i].Symbol == symbol))
                return true;
        }

        if ((_cells[0, 0].Symbol == symbol
            && _cells[1, 1].Symbol == symbol
            && _cells[2, 2].Symbol == symbol)
            ||
            (_cells[0, 2].Symbol == symbol
            && _cells[1, 1].Symbol == symbol
            && _cells[2, 0].Symbol == symbol))
            return true;

        return false;
    }

    internal bool IsDraw()
    {
        for (int i = 0; i < 3; i++)
            for (int j = 0; j < 3; j++)
                if (_cells[i, j].Symbol == Symbol.None)
                    return false;

        return true;
    }

    internal BoardMemento CreateMemento() 
    {
        var state = new Symbol[3, 3];
        for(int i = 0; i<3; i++)
            for(int j = 0; j<3; j++)
                state[i, j] = _cells[i, j].Symbol;

        return new BoardMemento(state);
    }

    internal void RestoreMemento(BoardMemento memento) 
    {
        var state = memento.GetState();
        for(int i = 0; i<3; i++)
            for(int j = 0; j<3; j++)
                _cells[i, j].RestoreCell(state[i, j]);

        Notify();
    }

   
    public void Attach(IObserver observer) => _observers.Add(observer);
    
    public void Detach(IObserver observer) => _observers.Remove(observer);
    
    public void Notify()
    {
        foreach (var observer in _observers)
            observer.Update(this);
        
    }
}
