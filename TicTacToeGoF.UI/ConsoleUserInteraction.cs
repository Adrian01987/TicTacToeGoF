using TicTacToeGoF.Core.Abstractions;
using TicTacToeGoF.Core.Models;

namespace TicTacToeGoF.UI;

public class ConsoleUserInteraction : IUserInteraction
{
    public void DisplayWelcomeMessage()
    {
        Console.Clear();
        Console.ForegroundColor = ConsoleColor.Gray;
        Console.WriteLine("Welcome to Tic-Tac-Toe!");
        Console.WriteLine("========================");
        Console.WriteLine("Players take turns marking a space in a 3x3 grid.");
        Console.WriteLine("The first player to get 3 of their marks in a row is the winner.");
        Console.WriteLine("\nTo make a move, enter the row and column (1-3), e.g., '2 3'.");
        Console.WriteLine("Type 'undo' to take back the last move.");
        Console.WriteLine("Type 'exit' to quit the game.");
        Console.WriteLine();
    }
    public PlayerCreationInfo GetPlayerDetails(string playerPrompt)
    {
        while (true)
        {
            Console.ForegroundColor = ConsoleColor.Gray;
            Console.WriteLine(playerPrompt);
            Console.Write("> ");
            var input = Console.ReadLine() ?? "";

            var parts = input.Split(' ', StringSplitOptions.RemoveEmptyEntries);
            if (parts.Length < 2)
            {
                DisplayMessage(MessageSeverity.Error, "Invalid input. Please provide at least a type and a name.");
                continue;
            }

            if (!Enum.TryParse<PlayerType>(parts[0], true, out var playerType))
            {
                DisplayMessage(MessageSeverity.Error, "Invalid player type. Use 'Human' or 'Computer'.");
                continue;
            }

            if (parts.Length == 3)
            {
                if (!Enum.TryParse<Symbol>(parts[1], true, out var symbol) || symbol == Symbol.None)
                {
                    DisplayMessage(MessageSeverity.Error, "Invalid symbol. Use 'X' or 'O'.");
                    continue;
                }
                return new PlayerCreationInfo(playerType, parts[2], symbol);
            }
            else
            {
                return new PlayerCreationInfo(playerType, parts[1]);
            }
        }
    }

    public Move GetMove(Player player, Board board)
    {
        while (true)
        {
            Console.ForegroundColor = ConsoleColor.Gray;
            Console.WriteLine($"{player.Name} ({player.Symbol}), enter your move (row column):");
            Console.Write("> ");
            var input = Console.ReadLine() ?? "";

            if (input.Equals("exit", StringComparison.OrdinalIgnoreCase))
            {
                return new ExitMove();
            }

            if (input.Equals("undo", StringComparison.OrdinalIgnoreCase))
            {
                return new UndoMove();
            }

            if (TryParseMove(input, out ushort row, out ushort col))
            {
                if (board.GetCell(row, col).Symbol == Symbol.None)
                {
                    return new ValidMove(player, row, col);
                }
                else
                {
                    DisplayMessage(MessageSeverity.Error, "That cell is already occupied. Please try again.");
                }
            }
            else
            {
                DisplayMessage(MessageSeverity.Error, "Invalid format. Please enter row and column as two numbers (e.g., '1 2').");
            }
        }
    }

    private static bool TryParseMove(string input, out ushort row, out ushort col)
    {
        row = col = 0;
        var parts = input.Split(' ', StringSplitOptions.RemoveEmptyEntries);
        if (parts.Length != 2) return false;

        if (!ushort.TryParse(parts[0], out ushort r) || !ushort.TryParse(parts[1], out ushort c)) return false;
        if (r < 1 || r > 3 || c < 1 || c > 3) return false;

        row = (ushort)(r - 1);
        col = (ushort)(c - 1);
        return true;
    }

    public void DisplayWinner(string winnerName)
    {
        DisplayMessage(MessageSeverity.Info, $"\nCongratulations, {winnerName}! You have won the game!");
    }

    public void DisplayDraw()
    {
        DisplayMessage(MessageSeverity.Info, "\nThe game is a draw!");
    }

    public bool GetPlayAgain()
    {
        Console.Write("\nPlay again? (y/n): ");
        var input = Console.ReadLine()?.ToLower();
        return input == "y";
    }

    public void DisplayMessage(MessageSeverity severety, string message)
    {
        switch (severety)
        {
            case MessageSeverity.Debug:
                Console.ForegroundColor = ConsoleColor.Gray;
                break;
            case MessageSeverity.Info:
                Console.ForegroundColor = ConsoleColor.Green;
                break;
            case MessageSeverity.Warning:
                Console.ForegroundColor = ConsoleColor.Yellow;
                break;
            case MessageSeverity.Error:
                Console.ForegroundColor = ConsoleColor.Red;
                break;
            case MessageSeverity.Apocalypse:
                Console.ForegroundColor = ConsoleColor.Magenta;
                break;
        }
        Console.WriteLine(message);
    }

    public void Clear()
    {
        Console.Clear();
    }
}
