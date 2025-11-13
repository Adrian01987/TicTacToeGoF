using Microsoft.Extensions.DependencyInjection;
using TicTacToeGoF.Core;
using TicTacToeGoF.Core.Abstractions;
using TicTacToeGoF.Core.Factory;
using TicTacToeGoF.Core.Models;
using TicTacToeGoF.UI;
using TicTacToeGoF.UI.Observers;

var services = new ServiceCollection();
services.AddSingleton<IUserInteraction, ConsoleUserInteraction>();
services.AddSingleton<ConsoleBoardDisplay>();
services.AddSingleton<PlayerFactory>();
services.AddTransient<GameFacade>();
var serviceProvider = services.BuildServiceProvider();

var boardDisplay = serviceProvider.GetRequiredService<ConsoleBoardDisplay>();
Board.Instance.Attach(boardDisplay);

var game = serviceProvider.GetRequiredService<GameFacade>();
game.Play();

Board.Instance.Detach(boardDisplay);