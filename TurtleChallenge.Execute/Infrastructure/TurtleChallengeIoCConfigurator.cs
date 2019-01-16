
using Microsoft.Practices.Unity;
using TurtleChallenge.Business;
using TurtleChallenge.Business.Utilities;
using TurtleChallenge.DataAccess;

namespace TurtleChallenge.Execute.Infrastructure
{
    /// <summary>
    /// The IoC configurator for Turtle challenge.
    /// </summary>
    public class TurtleChallengeIoCConfigurator
    {
        public static void ConfigureIoC()
        {
            IUnityContainer container = IoCContainer.Instance();
            container.RegisterType<IDataProvider, JsonDataProvider>(".json"); // for json files
            container.RegisterType<IGameDataManager, GameDataManager>();
            container.RegisterType<IGameManager, GameManager>();
            container.RegisterType<IGameDataValidator, GameDataValidator>();
            container.RegisterType<INavigator, TurtleNavigator>();
            container.RegisterType<IFileReadonlyAccess, FileReadOnlyAccess>();
        }
    }
}