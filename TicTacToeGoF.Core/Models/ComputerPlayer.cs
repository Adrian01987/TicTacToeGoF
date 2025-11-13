namespace TicTacToeGoF.Core.Models;

internal class ComputerPlayer(string name, Symbol symbol) : Player(name, symbol, PlayerType.Computer)
{
    private static readonly Random _random = new();

    public override Move GetMove(Board board)
    {
        var availableCells = new List<(ushort, ushort)>();
        for (ushort i = 0; i < 3; i++)
        {
            for (ushort j = 0; j < 3; j++)
            {
                if (board.GetCell(i, j).Symbol == Symbol.None)
                {
                    availableCells.Add((i, j));
                }
            }
        }

        if (availableCells.Count == 0)
        {
            return new InvalidMove("the board is already complete");
        }

        // Add a small delay to simulate thinking
        Thread.Sleep(500);

        var (row, col) = availableCells[_random.Next(availableCells.Count)];
        return new ValidMove(this, row, col);
    }
}
