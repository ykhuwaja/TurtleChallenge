using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using Rhino.Mocks;
using TurtleChallenge.DomainModel;
using Newtonsoft.Json;

namespace TurtleChallenge.DataAccess.Tests
{
    [TestFixture]
    public class JsonDataProviderTests
    {
        private IFileReadonlyAccess mockFileAccess;
        private IDataProvider provider;

        [SetUp]
        public void Setup()
        {
             mockFileAccess = MockRepository.GenerateMock<IFileReadonlyAccess>();
             provider = new JsonDataProvider(mockFileAccess);
        }

        [TearDown]
        public void TearDown()
        {
             mockFileAccess = null;
             provider = null;
        }
        

        [Test]
        public void GetGameSettings_Should_Successfully_Return_The_GameSettings_From_The_Json_File()
        {
            //
            // Arrange.
            //
            const string GameSettingsJsonContents = "{	\"boardSize\": { 			\"columns\": \"5\", 			\"rows\": \"5\" 		}, 		\"startingPosition\": { 			\"x\": \"0\", 			\"y\": \"0\" 		}, 		\"direction\": \"left\", 		\"exitPoint\": { 			\"x\": \"1\", 			\"y\": \"4\" 		}, 		\"mines\": [{ 				\"x\": \"1\", 				\"y\": \"1\" 			}, 			{ 			\"x\":\"2\", 				\"y\": \"2\" 			}, 			{ 				\"x\": \"3\", 				\"y\": \"3\" 			} 		] 	 }";
            const string FilePath = "game-settings.json";
            mockFileAccess.Stub(fs => fs.ReadAllText(FilePath)).Return(GameSettingsJsonContents);

            //
            // Act.
            //

            GameSettings game = provider.GetGameSettings(FilePath);

            //
            // Assert.
            //

            Assert.That(game.Direction, Is.EqualTo(Direction.Left));
            Assert.That(game.ExitPoint.Y, Is.EqualTo(4));
            Assert.That(game.ExitPoint.X, Is.EqualTo(1));
            Assert.That(game.Mines.Count, Is.EqualTo(3));
        }

        [Test]
        public void GetAllMoves_Should_Return_All_Moves_From_The_Json_File_Successfully()
        {
            //
            // Arrange.
            //
            const string MovesJsonContents = "[{	\"moves\": 		[		 \"m\", \"r\", \"r\", \"r\", \"m\"		],	\"name\": \"Sequence 1\"}, {	\"moves\": 	[	\"m\", \"m\", \"m\", \"m\",\"m\", \"r\"	],	\"name\": \"Sequence 2\"},{	\"moves\": 	[	\"r\", \"r\", \"r\", \"r\", \"r\"	],	\"name\": \"Sequence 3\"}]";
            const string MovesJsonFilePath = "moves.json";
            mockFileAccess.Stub(fs => fs.ReadAllText(MovesJsonFilePath)).Return(MovesJsonContents);

            //
            // Act.
            //

            IEnumerable<Sequence> sequences = provider.GetAllMoves(MovesJsonFilePath);

            //
            // Assert.
            //

            Assert.That(sequences.Count(), Is.EqualTo(3));
            List<Sequence> seqList = sequences.ToList();
            Assert.That(seqList[0].Moves.Count, Is.EqualTo(5));
            Assert.That(seqList[0].Name, Is.EqualTo("Sequence 1"));

        }

        [Test]
        [ExpectedException(typeof(JsonSerializationException))]
        public void GetGameSettings_should_throw_exception_when_any_number_value_is_negative()
        {
            //
            // Arrange.
            //

            const string GameSettingsJsonContents = "{	\"boardSize\": { 			\"columns\": \"-5\", 			\"rows\": \"5\" 		}, 		\"startingPosition\": { 			\"x\": \"0\", 			\"y\": \"0\" 		}, 		\"direction\": \"left\", 		\"exitPoint\": { 			\"x\": \"1\", 			\"y\": \"4\" 		}, 		\"mines\": [{ 				\"x\": \"1\", 				\"y\": \"1\" 			}, 			{ 			\"x\":\"2\", 				\"y\": \"2\" 			}, 			{ 				\"x\": \"3\", 				\"y\": \"3\" 			} 		] 	 }";
            const string FilePath = "game-settings.json";
            mockFileAccess.Stub(fs => fs.ReadAllText(FilePath)).Return(GameSettingsJsonContents);

            //
            // Act.
            //

            GameSettings gs =  provider.GetGameSettings(FilePath);

            //
            // 
            //
        }


        [Test]
        [ExpectedException(typeof(JsonSerializationException))]
        public void GetAllMoves_should_throw_JsonSerializationException_when_an_invalid_value_is_found_for_Move_enum_in_moves_file()
        {
            //
            // Arrange.
            //
            const string MovesJsonContents = "[{	\"moves\": 		[		 \"move\", \"yawar\", \"rotate\", \"rotate\", \"move\"		],	\"name\": \"Sequence 1\"}, {	\"moves\": 	[	\"move\", \"move\", \"move\", \"move\",\"move\", \"rotate\"	],	\"name\": \"Sequence 2\"},{	\"moves\": 	[	\"rotate\", \"rotate\", \"rotate\", \"rotate\", \"rotate\"	],	\"name\": \"Sequence 3\"}]";
            const string MovesJsonFilePath = "moves.json";

            mockFileAccess.Stub(fs => fs.ReadAllText(MovesJsonFilePath)).Return(MovesJsonContents);

            //
            // Act.
            //

            provider.GetAllMoves(MovesJsonFilePath);


        }
    }
}
