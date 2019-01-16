using TurtleChallenge.DomainModel;

namespace TurtleChallenge.Business
{
    public interface INavigator
    {
        Direction TurnRight90Digress(Direction currentDirection);
        bool ProcessMove(GameBoard board, Turtle turtle);
    }
}