using Akka.Actor;
using AkkaLifecycleAndSupervision.Actors;

namespace AkkaLifecycleAndSupervision
{
    internal partial class Program
    {
        static void Main(string[] args)
        {
            var actorSystem = ActorSystem.Create("AkkaLifecycleAndSupervisionActorSystem", Program.GetAkkaConfigurationFromHoconFile());

            var consoleWriter = actorSystem.ActorOf<ConsoleWriterActor>();
            
            actorSystem.ActorOf(Props.Create(() => new MainActor(consoleWriter)), "mainUserActor");
            
            actorSystem.WhenTerminated.Wait();
        }
    }
}