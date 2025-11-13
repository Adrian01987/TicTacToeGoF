using System.Text;
using TicTacToeGoF.Core.Models;
using TicTacToeGoF.Core.Observers;

namespace TicTacToeGoF.UI.Observers;

public class ConsoleBoardDisplay : IObserver
{
    public void Update(ISubject subject)
    {
        if (subject is Board board)
        {
            Console.WriteLine();
            StringBuilder sb = new();
            sb.AppendLine("  1 2 3");
            for (int i = 0; i < 3; i++)
            {
                sb.Append($"{i + 1} ");
                for (int j = 0; j < 3; j++)
                {
                    var cell = board.GetCell(i, j);
                    var symbol = cell.Symbol switch
                    {
                        Symbol.X => "X",
                        Symbol.O => "O",
                        _ => "-",
                    };
                    sb.Append($"{symbol} ");
                }
                sb.AppendLine();
            }
            Console.WriteLine(sb.ToString());
        }
    }
}
