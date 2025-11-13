namespace TicTacToeGoF.Core.Models;

public abstract record Move;

public record PlayerCreationInfo(PlayerType Type, string Name, Symbol? Symbol = null);

public record ValidMove(Player Player, ushort Row, ushort Column) : Move;

public record InvalidMove(string Reason) : Move;

public record UndoMove : Move;

public record ExitMove : Move;
