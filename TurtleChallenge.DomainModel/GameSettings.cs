using System.Collections.Generic;
using Newtonsoft.Json;

namespace TurtleChallenge.DomainModel
{
    /// <summary> Represents the initial settings of the game. </summary>
    public class GameSettings
    {
        /// <summary> Gets or sets the board of the game. </summary>
        [JsonProperty("boardSize")]
        public GameBoard GameBoard { get; set; }

        /// <summary>
        /// Gets or sets the starting point of turtle.
        /// </summary>
        [JsonProperty("startingPosition")]
        public Point StartingPoint { get; set; }
        
        /// <summary> Gets or sets the direction of the turtle. </summary>
        [JsonProperty("direction")]
        public Direction Direction { get; set; }

        /// <summary>
        /// Gets or sets the exit point of the turtle.
        /// </summary>
        [JsonProperty("exitPoint")]
        public Point ExitPoint { get; set; }

        /// <summary> Gets or sets the list of mines on the board. </summary>
        [JsonProperty("mines")]
        public List<Point> Mines { get; set; }

    }
}