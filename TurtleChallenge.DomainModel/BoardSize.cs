using Newtonsoft.Json;

namespace TurtleChallenge.DomainModel
{
    /// <summary> Represents the board of square of rows and columns on which the turtle will navigate. </summary>
    public class GameBoard
    {
        /// <summary>
        /// Gets or sets the number of columns of this game board. This value cannot be a negative number.
        /// </summary>
        [JsonProperty("columns")]
        public uint Columns { get; set; }

        /// <summary>
        /// Represents the number of rows of this game board. This value cannot be a negative number.
        /// </summary>
        [JsonProperty("rows")]
        public uint Rows { get; set; }

    }
}