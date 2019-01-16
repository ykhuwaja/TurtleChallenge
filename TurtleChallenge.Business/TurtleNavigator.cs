using TurtleChallenge.DomainModel;

namespace TurtleChallenge.Business
{
    /// <summary>
    /// Navigates the turtle within the board.
    /// </summary>
    public class TurtleNavigator : INavigator
    {
        /// <summary> Turns the direction to the right at 90 degrees. </summary>
        /// <param name="currentDirection">Direction to change.</param>
        /// <returns>New direction with 90 degrees right.</returns>
        public Direction TurnRight90Digress(Direction currentDirection)
        {
            int currentDirInt = (int)currentDirection;
            return (Direction)(++currentDirInt % 4);
        }

        /// <summary> Moves the turtle forward by 1 step in current direction. </summary>
        /// <param name="board">Game board.</param>
        /// <param name="turtle">Turtle to move forward.</param>
        /// <returns>A value indicating of forward move was done successfully. True as moved successfully, false if turtle hits a wall.</returns>
        public bool ProcessMove(GameBoard board, Turtle turtle)
        {
            bool moveResult = false;
            switch (turtle.Direction)
            {
                case Direction.Up:    // North....

                    if (turtle.Location.Y > 0)
                    {
                        turtle.Location.Y--;
                        moveResult = true;
                    }
                    break;

                case Direction.Down: // South...
                    if (turtle.Location.Y < board.Rows - 1)
                    {
                        turtle.Location.Y++;
                        moveResult = true;
                    }
                    break;

                case Direction.Left:// West...

                    if (turtle.Location.X > 0)
                    {
                        turtle.Location.X--;
                        moveResult = true;
                    }
                    break;

                case Direction.Right:  // East...
                    if (turtle.Location.X < board.Columns - 1)
                    {
                        turtle.Location.X++;
                        moveResult = true;
                    }
                    break;
            }

            return moveResult;
        }
    }
}