using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Practices.Unity;
using TurtleChallenge.Business;
using TurtleChallenge.Business.Utilities;
using TurtleChallenge.DomainModel;
using TurtleChallenge.Execute.Infrastructure;

namespace TurtleChallenge.Execute
{
    class Program
    {
        public static void Main(string[] args)
        {
            try
            {
                /*
                 * Uncomment the following lines to run this project in debug mode with following files.
                 */

                args = new string[2];
                args[0] = "game-settings.json";
                args[1] = "moves.json";
                
                Console.WriteLine("------------------------------------");
                Console.WriteLine("Turtle challenge game launched.");
                Console.WriteLine("------------------------------------");

                // using IoC container to register the dependencies.
                TurtleChallengeIoCConfigurator.ConfigureIoC();
                IGameManager manager = IoCContainer.Instance().Resolve<IGameManager>();

                // assuming the user inputs the game settings file with its extension being json..
                string gameSettingsFile = args[0];

                // assuming the user inputs the moves file with its extension being xml or json.
                string movesFile = args[1];

                var dataManager = IoCContainer.Instance().Resolve<IGameDataManager>();
                var validator = IoCContainer.Instance().Resolve<IGameDataValidator>();

                GameSettings settings = dataManager.GetGameSettings(gameSettingsFile);
                IEnumerable<string> errors = validator.ValidateGameSettings(settings).ToList();
                if (errors.Any())
                {
                    Print(errors);
                    return;
                }

                IEnumerable<Sequence> moves = dataManager.GetMoves(movesFile);
                IEnumerable<string> sequenceResults = manager.Start(settings, moves);

                Print(sequenceResults);

                Console.WriteLine("------------------------------------");
                Console.WriteLine("Turtle challenge game exits.");
                Console.WriteLine("------------------------------------");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"A fatel error occured during the execution of the game. Please check the log file for the detailed error message.\nDetails:{ex}");
            }
        }

        private static void Print(IEnumerable<string> items)
        {
            foreach (string nextItem in items)
            {
                Console.WriteLine(nextItem);
            }
        }
    }
}