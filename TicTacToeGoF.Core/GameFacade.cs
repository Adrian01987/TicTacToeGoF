using TicTacToeGoF.Core.Abstractions;
using TicTacToeGoF.Core.Command;
using TicTacToeGoF.Core.Factory;
using TicTacToeGoF.Core.Models;

namespace TicTacToeGoF.Core;

public class GameFacade(IUserInteraction userInteraction, PlayerFactory factory)
{
    private readonly IUserInteraction _userInteraction = userInteraction;
    private readonly PlayerFactory _playerFactory = factory;
    private Player _player1 = null!;
    private Player _player2 = null!;
    private Player _currentPlayer = null!;
    private readonly Board _board = Board.Instance;
    private readonly Stack<ICommand> _commands = new();
    private readonly Stack<Player> _playerTurnHistory = new();

    public void Play()
    {
        do
        {
            RunGame();
        } while (_userInteraction.GetPlayAgain());
    }

    private void RunGame()
    {
        _userInteraction.DisplayWelcomeMessage();
        
        if (!SetupPlayers())
            return;

        _board.Reset();
        _commands.Clear();
        _playerTurnHistory.Clear();

        while (true)
        {
            _playerTurnHistory.Push(_currentPlayer);
            var move = _currentPlayer.GetMove(_board);

            switch (move)
            {
                case ValidMove validMove:
                    if (_currentPlayer.Type == PlayerType.Computer)
                    {
                        _userInteraction.DisplayMessage(MessageSeverity.Info, $"{_currentPlayer.Name} chose cell ({validMove.Row + 1}, {validMove.Column + 1}).");
                    }

                    PlaceMarkCommand command = new(_board, validMove);
                    command.Execute();
                    _commands.Push(command);

                    if (_board.CheckForWin(_currentPlayer.Symbol))
                    {
                        _userInteraction.DisplayWinner(_currentPlayer.Name);
                        return;
                    }
                    if (_board.IsDraw())
                    {
                        _userInteraction.DisplayDraw();
                        return;
                    }
                    SwitchPlayer();
                    break;
                case UndoMove:
                    UndoLastMove();
                    break;
                case ExitMove:
                    _userInteraction.DisplayMessage(MessageSeverity.Info, "Game exited.");
                    return;
                case InvalidMove invalidMove:
                    _userInteraction.DisplayMessage(MessageSeverity.Error, $"An invalid move was attempted: {invalidMove.Reason}");
                    _playerTurnHistory.Pop();
                    break;
            }
        }
    }

    private void UndoLastMove()
    {
        if (_commands.Count == 0)
        {
            _userInteraction.DisplayMessage(MessageSeverity.Warning, "No moves to undo.");
            return;
        }

        // Determine if the last move was by a computer.
        var lastPlayer = _playerTurnHistory.Peek();
        bool wasComputerMove = lastPlayer.Type == PlayerType.Computer;

        // Pop the last move
        _commands.Pop().Undo();
        _playerTurnHistory.Pop();

        // If the move was by a computer, we want to undo the preceding human move as well
        // to give the turn back to the human player.
        if (wasComputerMove && _commands.Count > 0)
        {
            _commands.Pop().Undo();
            _playerTurnHistory.Pop();
            _userInteraction.DisplayMessage(MessageSeverity.Warning, "Your move and the computer's counter-move have been undone.");
        }
        else
        {
            _userInteraction.DisplayMessage(MessageSeverity.Warning, "Last move has been undone.");
        }

        // The current player is now the one who made the move we just undid (or the one before that).
        if (_playerTurnHistory.Count > 0)
        {
            _currentPlayer = _playerTurnHistory.Peek();
        }
        else
        {
            _currentPlayer = _player1; // Back to the start
        }
    }

    private bool SetupPlayers()
    {
        while (true)
        {
            PlayerCreationInfo p1Info;
            p1Info = _userInteraction.GetPlayerDetails("Enter Player 1 details (e.g., 'Human X Alice'):");
            if (p1Info.Symbol.HasValue && p1Info.Symbol != Symbol.None)
            {
                _player1 = _playerFactory.CreatePlayer(p1Info.Name, p1Info.Symbol.Value, p1Info.Type);
                break;
            }
            _userInteraction.DisplayMessage(MessageSeverity.Error,"Invalid details. Format: <Type> <Symbol> <Name> (e.g., 'Human X Alice')");
        }

        while (true)
        {
            var p2Symbol = _player1.Symbol == Symbol.X ? Symbol.O : Symbol.X;
            var p2Info = _userInteraction.GetPlayerDetails($"Enter Player 2 details (e.g., 'Computer Bob'). Symbol will be {p2Symbol}:");

            if (p2Info.Name != _player1.Name)
            {
                _player2 = _playerFactory.CreatePlayer(p2Info.Name, p2Symbol, p2Info.Type);
                break;
            }
            _userInteraction.DisplayMessage(MessageSeverity.Error, $"Invalid details. Name must be different from '{_player1.Name}'.");
        }

        _currentPlayer = _player1;
        return true;
    }

    private void SwitchPlayer() =>
        _currentPlayer = (_currentPlayer == _player1) ? _player2 : _player1;
}
