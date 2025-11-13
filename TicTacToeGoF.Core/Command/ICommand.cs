namespace TicTacToeGoF.Core.Command;

internal interface ICommand
{
    void Execute();
    void Undo();
}
