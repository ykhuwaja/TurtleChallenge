using System.Collections.Generic;
using TurtleChallenge.DomainModel;

namespace TurtleChallenge.Business
{
    /// <summary> Allows the game to start with game settings and moves from the files. </summary>
    public interface IGameManager
    {
        /// <summary> Starts the game with given settings and sequence of movies. </summary>
        /// <param name="settings">Game's startup settings.</param>
        /// <param name="moves">Move sequences of turtle.</param>
        /// <returns>Collection of strings containing the outcome for each sequence in the moves file.</returns>
        IEnumerable<string> Start(GameSettings settings, IEnumerable<Sequence> moves);
    }
}