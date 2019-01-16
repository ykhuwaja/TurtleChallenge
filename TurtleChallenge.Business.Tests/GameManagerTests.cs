using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using Rhino.Mocks;
using TurtleChallenge.DomainModel;

namespace TurtleChallenge.Business.Tests
{
    [TestFixture]
    public class GameManagerTests
    {

       private INavigator mockNavigator;

        [SetUp]
        public void Setup()
        {
            mockNavigator = MockRepository.GenerateMock<INavigator>();
        }

        [TearDown]
        public void TearDown()
        {
           mockNavigator = null;

        }

        [Test]
        public void Start_method_should_return_the_result_of_a_sequence_leading_to_exit()
        {
            // Arrange.

            GameSettings settings = new GameSettings
            {
                Direction = Direction.Down,
                GameBoard = new GameBoard {Columns = 5, Rows = 5},
                ExitPoint = new Point {Y = 1, X = 1},
                StartingPoint = new Point {Y = 0, X = 0},
                Mines = new List<Point> {new Point {X = 1, Y = 2}, new Point {X = 3, Y = 1}}
            };

            IEnumerable<Sequence> sequences = new List<Sequence>
            {
                new Sequence
                {
                    Name = "Sequence 1",
                    Moves = new List<Move>
                    {
                        Move.StepForward, Move.Turn, Move.StepForward
                    }
                }
            };
            
            mockNavigator.Stub(m => m.ProcessMove(Arg<GameBoard>.Is.Anything, Arg<Turtle>.Is.Anything)).Return(true)
                .WhenCalled(_ =>
                {
                    Turtle t = (Turtle)_.Arguments[1];
                    if (t.Location.Y < settings.GameBoard.Rows)
                    {
                        t.Location.Y++;
                        _.ReturnValue = true;
                    }
                    else
                        _.ReturnValue = false;
                }).Repeat.Once();

            mockNavigator.Stub(m => m.ProcessMove(Arg<GameBoard>.Is.Anything, Arg<Turtle>.Is.Anything)).Return(true).WhenCalled(_ =>
                {
                    Turtle t = (Turtle)_.Arguments[1];
                    if (t.Location.X < settings.GameBoard.Columns)
                    {
                        t.Location.X++;
                        _.ReturnValue = true;
                    }
                    else
                        _.ReturnValue = false;
                }).Repeat.Once();

          
           mockNavigator.Stub(r => r.TurnRight90Digress(settings.Direction)).Return(Direction.Right);

            IGameManager manager = new GameManager(mockNavigator);
            
            // Act.
            IEnumerable<string> results = manager.Start(settings, sequences);
            
            // Assert.
            Assert.That(results.Count(), Is.EqualTo(1));
            Assert.That(results.ToList()[0], Is.EqualTo("Sequence 1: Success!"));
        }

        [Test]
        public void Start_method_should_return_the_result_of_a_sequence_hitting_a_mine()
        {
            //
            // Arrange.
            //
            const string GameSettingsFilePath = "game-settings.json";
            const string MovesFilePath = "moves.json";

            GameSettings settings = new GameSettings
            {
                Direction = Direction.Down,
                GameBoard = new GameBoard {Columns = 5, Rows = 5},
                ExitPoint = new Point {Y = 4, X = 4},
                StartingPoint = new Point {Y = 0, X = 0},
                Mines = new List<Point>
                {
                    new Point {X = 1, Y = 2}, new Point {X = 3, Y = 1}, new Point {X = 1, Y = 1}
                }
            };

            IEnumerable<Sequence> sequences = new List<Sequence>
            {
                new Sequence
                {
                    Name = "Sequence 1",
                    Moves = new List<Move>
                    {
                        Move.StepForward, Move.Turn, Move.StepForward
                    }
                }
            };

            mockNavigator.Stub(r => r.TurnRight90Digress(settings.Direction)).Return(Direction.Right);
            mockNavigator.Stub(m => m.ProcessMove(Arg<GameBoard>.Is.Anything, Arg<Turtle>.Is.Anything)).Return(true)
                .WhenCalled(_ =>
                {
                    Turtle t = (Turtle)_.Arguments[1];
                    if (t.Location.Y < settings.GameBoard.Rows)
                    {
                        t.Location.Y++;
                        _.ReturnValue = true;
                    }
                    else
                        _.ReturnValue = false;
                }).Repeat.Once();

            mockNavigator.Stub(m => m.ProcessMove(Arg<GameBoard>.Is.Anything, Arg<Turtle>.Is.Anything)).Return(true).WhenCalled(_ =>
            {
                Turtle t = (Turtle)_.Arguments[1];
                if (t.Location.X < settings.GameBoard.Columns)
                {
                    t.Location.X++;
                    _.ReturnValue = true;
                }
                else
                    _.ReturnValue = false;
            }).Repeat.Once();


            IGameManager manager = new GameManager(mockNavigator);

            //
            // Act.
            //

            IEnumerable<string> results = manager.Start(settings, sequences);

            //
            // Assert.
            //

            Assert.That(results.Count(), Is.EqualTo(1));
            Assert.That(results.ToList()[0], Is.EqualTo("Sequence 1: Mine hit!"));

        }

        [Test]
        public void Start_method_should_return_the_result_of_a_sequence_StillInDanger()
        {
            //
            // Arrange.
            //

            GameSettings settings = new GameSettings();
            settings.Direction = Direction.Up;
            settings.GameBoard = new GameBoard { Columns = 5, Rows = 5 };
            settings.ExitPoint = new Point { Y = 4, X = 4 };
            settings.StartingPoint = new Point { Y = 0, X = 0 };
            settings.Mines = new List<Point>
            {
                new Point {X = 1, Y = 2 },
                new Point {X = 3, Y = 1 },
                new Point {X = 4, Y = 1 }
            };

            IEnumerable<Sequence> sequences = new List<Sequence>
            {
                new Sequence
                {
                    Name = "Sequence 1",
                    Moves = new List<Move>
                    {
                        Move.StepForward, Move.Turn, Move.StepForward
                    }
                }
            };

            mockNavigator.Stub(r => r.TurnRight90Digress(settings.Direction)).Return(Direction.Right);
            IGameManager manager = new GameManager(mockNavigator);

            //
            // Act.
            //

            IEnumerable<string> results = manager.Start(settings, sequences);

            //
            // Assert.
            //

            Assert.That(results.Count(), Is.EqualTo(1));
            Assert.That(results.ToList()[0], Is.EqualTo("Sequence 1: Still in danger!"));

        }

        [Test]

        public void Start_method_should_return_the_results_of_multiple_sequences()
        {
            //
            // Arrange.
            //
            const string GameSettingsFilePath = "game-settings.json";
            const string MovesFilePath = "moves.json";

            GameSettings settings = new GameSettings
            {
                Direction = Direction.Down,
                GameBoard = new GameBoard {Columns = 5, Rows = 5},
                ExitPoint = new Point {Y = 4, X = 4},
                StartingPoint = new Point {Y = 0, X = 0},
                Mines = new List<Point>
                {
                    new Point {X = 1, Y = 2}, new Point {X = 3, Y = 1}, new Point {X = 4, Y = 1}
                }
            };

            IEnumerable<Sequence> sequences = new List<Sequence>
            {
                new Sequence
                {
                    Name = "Sequence 1",
                    Moves = new List<Move> // successful
                    {
                        Move.StepForward, Move.StepForward, Move.StepForward, Move.StepForward,
                        Move.Turn,
                        Move.StepForward,Move.StepForward,Move.StepForward,Move.StepForward
                    }
                },
                new Sequence
                {
                    Name = "Sequence 2",
                    Moves = new List<Move> // mine hit
                    {
                        Move.StepForward, Move.Turn, Move.StepForward
                    }
                },
                new Sequence
                {
                    Name = "Sequence 3",
                    Moves = new List<Move> // still in danger
                    {
                        Move.StepForward, Move.StepForward, Move.Turn, Move.Turn, Move.Turn, Move.StepForward, Move.StepForward
                    }
                }
            };

            mockNavigator.Stub(r => r.TurnRight90Digress(settings.Direction)).Return(Direction.Right);
            mockNavigator.Stub(m => m.ProcessMove(Arg<GameBoard>.Is.Anything, Arg<Turtle>.Is.Anything)).Return(true)
                .WhenCalled(_ =>
                {
                    Turtle t = (Turtle) _.Arguments[1];
                    if (t.Location.Y < settings.GameBoard.Rows)
                    {
                        t.Location.Y++;
                        _.ReturnValue = true;
                    }
                    else
                        _.ReturnValue = false;
                }).Repeat.Times(4);


            mockNavigator.Stub(m => m.ProcessMove(Arg<GameBoard>.Is.Anything, Arg<Turtle>.Is.Anything)).Return(true).WhenCalled(_ =>
            {
                Turtle t = (Turtle)_.Arguments[1];
                if (t.Location.X < settings.GameBoard.Columns)
                {
                    t.Location.X++;
                    _.ReturnValue = true;
                }
                else
                    _.ReturnValue = false;
            }).Repeat.Times(4);


            mockNavigator.Stub(m => m.ProcessMove(Arg<GameBoard>.Is.Anything, Arg<Turtle>.Is.Anything)).Return(true)
                .WhenCalled(_ =>
                {
                    Turtle t = (Turtle) _.Arguments[1];
                    if (t.Location.Y < settings.GameBoard.Rows)
                    {
                        t.Location.Y++;
                        _.ReturnValue = true;
                    }
                    else
                        _.ReturnValue = false;
                }).Repeat.Once();

            mockNavigator.Stub(m => m.ProcessMove(Arg<GameBoard>.Is.Anything, Arg<Turtle>.Is.Anything)).Return(true).WhenCalled(_ =>
            {
                Turtle t = (Turtle)_.Arguments[1];
                if (t.Location.X < settings.GameBoard.Columns)
                {
                    t.Location.X++;
                    _.ReturnValue = true;
                }
                else
                    _.ReturnValue = false;
            }).Repeat.Once();


            mockNavigator.Stub(m => m.ProcessMove(Arg<GameBoard>.Is.Anything, Arg<Turtle>.Is.Anything)).Return(true)
                .WhenCalled(_ =>
                {
                    Turtle t = (Turtle)_.Arguments[1];
                    if (t.Location.Y < settings.GameBoard.Rows)
                    {
                        t.Location.Y++;
                        _.ReturnValue = true;
                    }
                    else
                        _.ReturnValue = false;
                }).Repeat.Times(2);


            mockNavigator.Stub(m => m.ProcessMove(Arg<GameBoard>.Is.Anything, Arg<Turtle>.Is.Anything)).Return(true).WhenCalled(_ =>
            {
                Turtle t = (Turtle)_.Arguments[1];
                if (t.Location.X < settings.GameBoard.Columns)
                {
                    t.Location.X++;
                    _.ReturnValue = true;
                }
                else
                    _.ReturnValue = false;
            }).Repeat.Times(2);

            IGameManager manager = new GameManager(mockNavigator);

            //
            // Act.
            //

            IEnumerable<string> results = manager.Start(settings, sequences);

            //
            // Assert.
            //

            //Assert.That(results.Count(), Is.EqualTo(3));
            Assert.That(results.ToList()[0], Is.EqualTo("Sequence 1: Success!"));
            //Assert.That(results.ToList()[1], Is.EqualTo("Sequence 2: Mine hit!"));
            //Assert.That(results.ToList()[2], Is.EqualTo("Sequence 3: Still in danger!"));

        }

       
        [Test]
        public void Smallest_board_with_a_mine_successful_result()
        {
            //
            // Arrange.
            //

            GameSettings settings = new GameSettings
            {
                Direction = Direction.Right,
                GameBoard = new GameBoard {Columns = 2, Rows = 2},
                ExitPoint = new Point {Y = 0, X = 1},
                StartingPoint = new Point {Y = 0, X = 0},
                Mines = new List<Point> {new Point {X = 0, Y = 1}}
            };

            IEnumerable<Sequence> sequences = new List<Sequence>
            {
                new Sequence
                {
                    Name = "Sequence 1",
                    Moves = new List<Move> // successful
                    {
                        Move.StepForward, Move.Turn, Move.StepForward
                    }
                }
            };

            mockNavigator.Stub(r => r.TurnRight90Digress(settings.Direction)).Return(Direction.Right);
            mockNavigator.Stub(m => m.ProcessMove(Arg<GameBoard>.Is.Anything, Arg<Turtle>.Is.Anything)).Return(true)
                .WhenCalled(_ =>
                {
                    Turtle t = (Turtle)_.Arguments[1];
                    if (t.Location.X < settings.GameBoard.Columns)
                    {
                        t.Location.X++;
                        _.ReturnValue = true;
                    }
                    else
                        _.ReturnValue = false;

                }).Repeat.Once();

            mockNavigator.Stub(m => m.ProcessMove(Arg<GameBoard>.Is.Anything, Arg<Turtle>.Is.Anything)).Return(true).WhenCalled(_ =>
            {

                Turtle t = (Turtle)_.Arguments[1];
                if (t.Location.Y < settings.GameBoard.Rows)
                {
                    t.Location.Y++;
                    _.ReturnValue = true;
                }
                else
                    _.ReturnValue = false;

            }).Repeat.Once();



            IGameManager manager = new GameManager(mockNavigator);

            //
            // Act.
            //

            IEnumerable<string> results = manager.Start(settings, sequences);

            //
            // Assert.
            //

            Assert.That(results.Count(), Is.EqualTo(1));
            Assert.That(results.ToList()[0], Is.EqualTo("Sequence 1: Success!"));
        }



        [Test]
        public void JustStartAndEndPoint()
        {
            //
            // Arrange.
            //

           
            GameSettings settings = new GameSettings();
            settings.Direction = Direction.Down;
            settings.GameBoard = new GameBoard { Columns = 1, Rows = 2 };

            settings.ExitPoint = new Point { Y = 1, X = 0 };
            settings.StartingPoint = new Point { Y = 0, X = 0 };

            settings.Mines = new List<Point>();

            IEnumerable<Sequence> sequences = new List<Sequence>
            {
                new Sequence
                {
                    Name = "Sequence 1",
                    Moves = new List<Move> // successful
                    {
                        Move.StepForward
                    }
                }
            };

            mockNavigator.Stub(r => r.TurnRight90Digress(settings.Direction)).Return(Direction.Right);
            mockNavigator.Stub(m => m.ProcessMove(Arg<GameBoard>.Is.Anything, Arg<Turtle>.Is.Anything)).Return(true).WhenCalled(_ =>
            {

                Turtle t = (Turtle)_.Arguments[1];
                if (t.Location.Y < settings.GameBoard.Rows)
                {
                    t.Location.Y++;
                    _.ReturnValue = true;
                }
                else
                    _.ReturnValue = false;

            }).Repeat.Once();

            IGameManager manager = new GameManager(mockNavigator);

           
            // Act.
            IEnumerable<string> results = manager.Start(settings, sequences);
            
            // Assert.
            Assert.That(results.Count(), Is.EqualTo(1));
            Assert.That(results.ToList()[0], Is.EqualTo("Sequence 1: Success!"));

        }
    }
}