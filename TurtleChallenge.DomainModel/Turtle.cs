namespace TurtleChallenge.DomainModel
{
    /// <summary>
    /// Represents the Turtle object that will move on the game board.
    /// </summary>
    public class Turtle
    {
        /// <summary>
        /// Gets or sets the location of this turtle.
        /// </summary>
        public Point Location { get; set; }

        /// <summary>
        /// Gets or sets the direction of this turtle.
        /// </summary>
        public Direction Direction { get; set; }
    }
}