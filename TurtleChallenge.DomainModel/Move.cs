using System.Runtime.Serialization;

namespace TurtleChallenge.DomainModel
{
    /// <summary>
    /// Defines the activity of the turtle.
    /// </summary>
    public enum Move
    {
        /// <summary>
        /// Represents the move of the turtle. StepForward is more meaningful name, hence the mapping in the attributes.
        /// </summary>
        [EnumMember(Value = "m")]
        StepForward = 0,
        
        /// <summary>
        /// Represents the rotation of turtle. Turn is more meaningful name, hence the mapping in the attributes.
        /// </summary>
        [EnumMember(Value = "r")]
        Turn = 1
    }
}