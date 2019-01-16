using System.Collections.Generic;
using TurtleChallenge.DomainModel;

namespace TurtleChallenge.Business
{
    /// <summary> Allows the game to start with game settings and moves from the files. </summary>
    public class GameManager : IGameManager
    {
        private readonly INavigator turtleNavigator;
        public GameManager(INavigator turtleNavigator)
        {
            this.turtleNavigator = turtleNavigator;
        }

        /// <summary> Starts the game. </summary>
        /// <param name="settings">Game's startup settings.</param>
        /// <param name="moves">Move sequences of turtle.</param>
        /// <returns>Collection of strings containing the outcome for each sequence in the moves file.</returns>
        public IEnumerable<string> Start(GameSettings settings, IEnumerable<Sequence> moves)
        {
            IList<string> results = new List<string>();
            Turtle turtle = new Turtle();
            foreach (Sequence sequence in moves)
            {
                // assumption made: for every new sequence reset the turtle's direction and current position 
                // to the initial settings from the gameSettings.
                turtle.Location = new Point
                {
                    Y = settings.StartingPoint.Y,
                    X = settings.StartingPoint.X
                };

                turtle.Direction = settings.Direction;
                results.Add(ProcessSequence(sequence, turtle, settings));
            }

            return results;
        }
        
        /// <summary> Processes the sequence of moves of the turtle on the board. Each sequence starts with the starting point and direction from the game-settings. </summary>
        /// <param name="sequence">Sequence of the moves.</param>
        /// <param name="turtle">Turtle to navigate.</param>
        /// <param name="gameSettings">Game settings.</param>
        /// <returns>Result of the sequence.</returns>
        private string ProcessSequence(Sequence sequence, Turtle turtle, GameSettings gameSettings)
        {
            string sequenceResult = sequence.Name;
            foreach (Move nextMove in sequence.Moves)
            {
                switch (nextMove)
                {
                    case Move.StepForward:
                        bool moveSuccess = turtleNavigator.ProcessMove(gameSettings.GameBoard, turtle);

                        // Because its not clear in the question about the condition when turtle hits the border.
                        // I have commented the code below which allows the current sequence of moves to stop
                        // and return border hit message and process the next sequence if any.
                        /*
                        if (!moveSuccess)
                        {
                            sequenceResult += ": " + turtle.Direction + " border hit!";
                            return sequenceResult;
                        }
                        */
                        
                        if (gameSettings.Mines.Exists(p => p.Y == turtle.Location.Y && p.X == turtle.Location.X))
                        {
                            sequenceResult += ": Mine hit!";
                            return sequenceResult;
                        }

                        if (gameSettings.ExitPoint.Y == turtle.Location.Y && gameSettings.ExitPoint.X == turtle.Location.X)
                        {
                            sequenceResult += ": Success!";
                            return sequenceResult;
                        }

                        break;

                    case Move.Turn:
                        turtle.Direction = turtleNavigator.TurnRight90Digress(turtle.Direction);
                        break;
                }
            }

            return sequenceResult + ": Still in danger!";
        }
    }
}
