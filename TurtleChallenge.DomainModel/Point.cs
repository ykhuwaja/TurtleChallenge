using System;
using System.Xml.Serialization;
using Newtonsoft.Json;

namespace TurtleChallenge.DomainModel
{
    /// <summary> Represents a location on the game board. </summary>
    public class Point : IEquatable<Point>
    {
        /// <summary>
        /// Gets or sets the X coordinate value of the game board. This value cannot be a negative number.
        /// </summary>
        [JsonProperty("x")]
        public uint X { get; set; }

        /// <summary> Gets or sets the Y coordinate value of the game board. This value cannot be a negative number. </summary>
        [JsonProperty("y")]
        public uint Y { get; set; }

        public bool Equals(Point other)
        {
            return other != null && Y == other.Y && X == other.X;
        }
    }
}